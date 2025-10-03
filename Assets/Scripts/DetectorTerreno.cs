using UnityEngine;

public class DetectorTerreno : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    public float velocidadNormal = 1f;
    public float velocidadActual = 1f;
    
    [Header("Configuración de Detección")]
    public float radioDeteccion = 0.3f;
    public LayerMask capaFriccion;
    public LayerMask capaAgua;
    
    [Header("Estado Actual (Solo Lectura)")]
    public bool enAgua = false;
    public bool enFriccion = false;
    public bool conejosPuedenCruzar = false;
    
    private float factorFriccionActual = 1f;
    private TextMesh textoDebug;

    void Start()
    {
        CrearTextoDebug();
    }

    void Update()
    {
        DetectarTerreno();
        ActualizarVelocidad();
        ActualizarTextoDebug();
    }

    void CrearTextoDebug()
    {
        // Crear objeto para texto debug
        GameObject textoObj = new GameObject("DebugText");
        textoObj.transform.SetParent(transform);
        textoObj.transform.localPosition = new Vector3(0, 0.8f, 0);
        
        textoDebug = textoObj.AddComponent<TextMesh>();
        textoDebug.characterSize = 0.1f;
        textoDebug.fontSize = 20;
        textoDebug.alignment = TextAlignment.Center;
        textoDebug.anchor = TextAnchor.MiddleCenter;
        textoDebug.color = Color.black;
    }

    void ActualizarTextoDebug()
    {
        if (textoDebug != null)
        {
            string estado = "Normal";
            Color color = Color.green;

            if (enAgua)
            {
                estado = "AGUA";
                color = Color.blue;
                if (!conejosPuedenCruzar && gameObject.CompareTag("Bunny"))
                {
                    estado = "BLOQUEADO";
                    color = Color.red;
                }
            }
            else if (enFriccion)
            {
                estado = $"LENTO ({factorFriccionActual:F1}x)";
                color = Color.yellow;
            }

            textoDebug.text = $"{estado}\nVel: {velocidadActual:F1}";
            textoDebug.color = color;
        }
    }

    void DetectarTerreno()
    {
        // Detectar agua
        Collider2D hitAgua = Physics2D.OverlapCircle(transform.position, radioDeteccion, capaAgua);
        bool estabaEnAgua = enAgua;
        enAgua = hitAgua != null;

        if (hitAgua != null)
        {
            CuerpoAgua cuerpoAgua = hitAgua.GetComponent<CuerpoAgua>();
            if (cuerpoAgua != null)
            {
                conejosPuedenCruzar = cuerpoAgua.conejosPuedenCruzar;
                
                if (enAgua && !estabaEnAgua && gameObject.CompareTag("Bunny") && !cuerpoAgua.conejosPuedenCruzar)
                {
                    CambiarDireccionLejosDeAgua(hitAgua.transform.position);
                }
            }
        }

        // Detectar fricción
        Collider2D hitFriccion = Physics2D.OverlapCircle(transform.position, radioDeteccion, capaFriccion);
        enFriccion = hitFriccion != null;
        
        if (hitFriccion != null)
        {
            ZonaFriccion zona = hitFriccion.GetComponent<ZonaFriccion>();
            if (zona != null)
            {
                factorFriccionActual = zona.factorFriccion;
            }
        }
        else
        {
            factorFriccionActual = 1f;
        }
    }

    void ActualizarVelocidad()
    {
        if (enAgua && gameObject.CompareTag("Bunny") && !conejosPuedenCruzar)
        {
            velocidadActual = 0f;
            return;
        }
        else if (enAgua && gameObject.CompareTag("Predator"))
        {
            Collider2D hitAgua = Physics2D.OverlapCircle(transform.position, radioDeteccion, capaAgua);
            if (hitAgua != null)
            {
                CuerpoAgua cuerpoAgua = hitAgua.GetComponent<CuerpoAgua>();
                if (cuerpoAgua != null && cuerpoAgua.depredadoresPuedenCruzar)
                {
                    velocidadActual = velocidadNormal * cuerpoAgua.factorVelocidadDepredadores;
                    return;
                }
            }
        }

        velocidadActual = enFriccion ? velocidadNormal * factorFriccionActual : velocidadNormal;
    }

    void CambiarDireccionLejosDeAgua(Vector3 posicionAgua)
    {
        Bunny bunny = GetComponent<Bunny>();
        if (bunny != null)
        {
            System.Reflection.FieldInfo field = typeof(Bunny).GetField("destination", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                Vector3 direccionSegura = (transform.position - posicionAgua).normalized;
                Vector3 nuevoDestino = transform.position + direccionSegura * 3f;
                field.SetValue(bunny, nuevoDestino);
            }
        }
    }

    public float ObtenerVelocidad()
    {
        return velocidadActual;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = enAgua ? Color.blue : (enFriccion ? Color.yellow : Color.green);
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
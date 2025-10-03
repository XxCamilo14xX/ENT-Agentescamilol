using UnityEngine;

public class DebugVisual : MonoBehaviour
{
    [Header("Debug Visual")]
    public bool mostrarEstado = true;
    public Color colorNormal = Color.green;
    public Color colorFriccion = Color.yellow;
    public Color colorAgua = Color.blue;
    public Color colorBloqueado = Color.red;

    private DetectorTerreno detector;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        detector = GetComponent<DetectorTerreno>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (!mostrarEstado || detector == null) return;

        // Cambiar color según el estado del terreno
        if (detector.enAgua && !detector.conejosPuedenCruzar && gameObject.CompareTag("Bunny"))
        {
            spriteRenderer.color = colorBloqueado; // ROJO cuando está bloqueado
        }
        else if (detector.enAgua)
        {
            spriteRenderer.color = colorAgua; // AZUL en agua
        }
        else if (detector.enFriccion)
        {
            spriteRenderer.color = colorFriccion; // AMARILLO en fricción
        }
        else
        {
            spriteRenderer.color = colorNormal; // VERDE normal
        }
    }
}
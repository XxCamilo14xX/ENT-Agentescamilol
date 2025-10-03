using UnityEngine;

public class CuerpoAgua : MonoBehaviour
{
    [Header("Configuración del Agua")]
    public bool conejosPuedenCruzar = false;
    public bool depredadoresPuedenCruzar = true;
    public float factorVelocidadDepredadores = 0.3f;

    [Header("Configuración Visual")]
    public Color colorAgua = new Color(0.2f, 0.4f, 0.8f, 0.6f);
    public Vector2 tamaño = new Vector2(1, 1); // ← NUEVO: Control de tamaño
    
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    void Start()
    {
        // Obtener o agregar componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        // Aplicar tamaño
        AplicarTamaño();
    }

    void AplicarTamaño()
    {
        // Ajustar collider
        boxCollider.size = tamaño;
        
        // Ajustar sprite
        spriteRenderer.sprite = CrearSpriteCuadrado();
        spriteRenderer.color = colorAgua;
        spriteRenderer.sortingOrder = -1;
        
        // Escalar el sprite para que coincida con el collider
        transform.localScale = new Vector3(tamaño.x, tamaño.y, 1);
    }

    Sprite CrearSpriteCuadrado()
    {
        Texture2D texture = new Texture2D(256, 256);
        Color[] pixels = new Color[256 * 256];
        
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 256, 256), Vector2.one * 0.5f);
    }

    // Para actualizar en tiempo de edición
    private void OnValidate()
    {
        if (Application.isPlaying && spriteRenderer != null && boxCollider != null)
        {
            AplicarTamaño();
        }
    }
}
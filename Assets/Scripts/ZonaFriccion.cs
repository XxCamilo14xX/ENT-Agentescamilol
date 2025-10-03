using UnityEngine;

public class ZonaFriccion : MonoBehaviour
{
    [Header("Configuración de Fricción")]
    [Range(0.1f, 0.9f)]
    public float factorFriccion = 0.5f;

    [Header("Configuración Visual")]
    public Color colorVisual = new Color(0.8f, 0.6f, 0.2f, 0.6f);
    public Vector2 tamaño = new Vector2(1, 1); // ← NUEVO
    
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    void Start()
    {
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

        AplicarTamaño();
    }

    void AplicarTamaño()
    {
        boxCollider.size = tamaño;
        spriteRenderer.sprite = CrearSpriteCuadrado();
        spriteRenderer.color = colorVisual;
        spriteRenderer.sortingOrder = -1;
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

    private void OnValidate()
    {
        if (Application.isPlaying && spriteRenderer != null && boxCollider != null)
        {
            AplicarTamaño();
        }
    }
}
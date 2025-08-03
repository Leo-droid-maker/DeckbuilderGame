using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridCellHighlighter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color highLightColor = Color.yellow;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = highLightColor;
    }

    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }
}

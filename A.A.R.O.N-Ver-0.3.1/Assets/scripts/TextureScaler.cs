using UnityEngine;

public class TextureScaler : MonoBehaviour
{
    BoxCollider2D col;
    SpriteRenderer ren;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        ren = GetComponent<SpriteRenderer>();
        if (col != null && ren != null)
        {
            col.size = ren.bounds.size;
        }
    }

   
}

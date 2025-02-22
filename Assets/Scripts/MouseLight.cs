using UnityEngine;

public class MouseLight : MonoBehaviour
{
    private Material mat;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        // Mouse pozisyonunu dünya koordinatlarına çevir
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Sprite'ın boyutunu al
        Vector2 spriteSize = GetComponent<SpriteRenderer>().bounds.size;

        // Sprite'ın dünya koordinatlarındaki merkezini al
        Vector3 spriteCenter = transform.position;

        // Mouse pozisyonunu sprite'ın yerel koordinatlarına çevir
        Vector3 mouseLocalPos = mouseWorldPos - spriteCenter;

        // Mouse pozisyonunu 0-1 aralığında normalize et
        float normalizedX = (mouseLocalPos.x / spriteSize.x) + 0.5f;
        float normalizedY = (mouseLocalPos.y / spriteSize.y) + 0.5f;

        // UV koordinatlarını sınırların içinde tut
        normalizedX = Mathf.Clamp01(normalizedX);
        normalizedY = Mathf.Clamp01(normalizedY);

        Vector2 mouseUV = new Vector2(normalizedX, normalizedY);

        // Shader'a mouse pozisyonunu gönder
        mat.SetVector("_MousePos", mouseUV);

        // Deliğin boyutunu değiştirmek için
        mat.SetFloat("_Radius", 0.08f); // Küçültmek için 0.05, büyütmek için 0.15 gibi değerler kullan
    }
}
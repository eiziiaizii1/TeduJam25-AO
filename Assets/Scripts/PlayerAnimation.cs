using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Vector2 movement;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Hareket girişlerini al
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Eğer karakter hareket etmiyorsa Idle animasyonu oynat
        if (movement == Vector2.zero)
        {
            anim.Play("idle");
        }
        else
        {
            // Yön belirleme
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // Sağa veya sola hareket
                anim.Play("run_right");
                transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1); // Sağ/sol flip
            }
            else
            {
                // Yukarı veya aşağı hareket
                if (movement.y > 0)
                    anim.Play("run_back");
                else
                    anim.Play("run_front");
            }
        }
    }
}

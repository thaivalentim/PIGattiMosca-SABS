using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlePrincesa : MonoBehaviour
{
    public float velocidade = 10f;
    private Rigidbody2D rb;
    private Vector2 movimento;

    public SpriteRenderer idleSprite;

    public SpriteRenderer moveLadoAnda;
    public SpriteRenderer moveCimaAnda;
    public SpriteRenderer moveBaixoAnda;

    public SpriteRenderer moveLadoCorre;
    public SpriteRenderer moveCimaCorre;
    public SpriteRenderer moveBaixoCorre;

    public SpriteRenderer morreSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DesativarTudo();
        idleSprite.enabled = true;
    }

    void Update()
    {
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");
        movimento.Normalize();

        bool isMoving = movimento.magnitude > 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        velocidade = isRunning ? 20f : 10f;

        AtualizarAnims(isMoving, isRunning);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimento * velocidade * Time.fixedDeltaTime);
    }

    void DesativarTudo()
    {
        idleSprite.enabled = false;

        moveLadoAnda.enabled = false;
        moveCimaAnda.enabled = false;
        moveBaixoAnda.enabled = false;

        moveLadoCorre.enabled = false;
        moveCimaCorre.enabled = false;
        moveBaixoCorre.enabled = false;

        morreSprite.enabled = false;
    }

    void AtualizarAnims(bool isMoving, bool isRunning)
    {
        DesativarTudo();

        if (!isMoving)
        {
            idleSprite.enabled = true;
            return;
        }
        if (movimento.x < 0)
        {
            moveLadoAnda.flipX = false;
            moveLadoCorre.flipX = false;
        }
        else if (movimento.x > 0)
        {
            moveLadoAnda.flipX = true;
            moveLadoCorre.flipX = true;
        }
        if (isRunning)
        {
            if (Mathf.Abs(movimento.x) > 0)
                moveLadoCorre.enabled = true;
            else if (movimento.y > 0)
                moveCimaCorre.enabled = true;
            else if (movimento.y < 0)
                moveBaixoCorre.enabled = true;

            return;
        }
        if (Mathf.Abs(movimento.x) > 0)
            moveLadoAnda.enabled = true;
        else if (movimento.y > 0)
            moveCimaAnda.enabled = true;
        else if (movimento.y < 0)
            moveBaixoAnda.enabled = true;
    }
}

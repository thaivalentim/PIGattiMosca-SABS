using System.Collections;
using Unity.VisualScripting;
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

    public SpriteRenderer ataqueSprite;
    public GameObject colisaoAtaque;

    public SpriteRenderer morreSprite;
    public float health = 600f;

    private bool isAttacking = false;

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

        if (!isAttacking)
            AtualizarAnims(isMoving, isRunning);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Atacar();
    }

    void Atacar()
    {
        isAttacking = true;

        DesativarTudo();
        ataqueSprite.enabled = true;

        if (movimento.x < 0)
        {
            ataqueSprite.flipX = false;
            colisaoAtaque.transform.localPosition = new Vector3(-0.5f, 0, 0);
        }
        else if (movimento.x > 0)
        {
            ataqueSprite.flipX = true;
            colisaoAtaque.transform.localPosition = new Vector3(14.5f, 0, 0);
        }

        colisaoAtaque.SetActive(true);
        StartCoroutine(PararAtaque());
    }

    IEnumerator PararAtaque()
    {
        yield return new WaitForSeconds(0.5f);

        colisaoAtaque.SetActive(false);
        ataqueSprite.enabled = false;

        isAttacking = false;
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

        ataqueSprite.enabled = false;

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

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Princesa Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DesativarTudo();
        morreSprite.enabled = true;
        Destroy(gameObject, 1f);
    }
}

using UnityEngine;

public class MovimentoJogador : MonoBehaviour
{
    // === 1. PAR�METROS DE SA�DE ===
    [Header("Health Parameters")]
    public float health = 600f;

    // === 2. PAR�METROS DE CONTROLE ===
    [Header("Movement Parameters")]
    public float moveSpeed = 5f; // Ajuste a velocidade no Inspector

    // === VARI�VEIS INTERNAS ===
    private Rigidbody2D rb;
    private Vector2 movement; // Armazena a dire��o do input

    void Start()
    {
        // Pega o Rigidbody2D (essencial para movimento baseado em f�sica)
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("PrincessHealth precisa de um componente Rigidbody2D no objeto!");
            enabled = false;
            return;
        }

        // Garante que o Rigidbody n�o gira e ignora a gravidade
        rb.freezeRotation = true;
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Update � usado para coletar o input
    void Update()
    {
        // Pega o input dos eixos Horizontal e Vertical (WASD, Setas)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normaliza o vetor para que a velocidade diagonal n�o seja maior
        movement.Normalize();
    }

    // FixedUpdate � usado para aplicar o movimento ao Rigidbody
    void FixedUpdate()
    {
        // Move o Rigidbody para a nova posi��o
        rb.velocity = movement * moveSpeed;
    }

    // === 3. FUN��ES DE DANO (Chamada pelo Minotauro) ===

    // A fun��o deve ser p�blica para ser chamada de outro script (MinotaurAI_2D)
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
        Debug.Log("A Princesa morreu!");
        // Destrua ou desative o objeto da princesa aqui
        Destroy(gameObject);
    }
}
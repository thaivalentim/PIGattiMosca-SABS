using UnityEngine;

public class Princesateste : MonoBehaviour
{
    // === 1. PARÂMETROS DE SAÚDE ===
    [Header("Health Parameters")]
    public float health = 600f;

    // === 2. PARÂMETROS DE CONTROLE ===
    [Header("Movement Parameters")]
    public float moveSpeed = 5f; // Ajuste a velocidade no Inspector

    // === VARIÁVEIS INTERNAS ===
    private Rigidbody2D rb;
    private Vector2 movement; // Armazena a direção do input

    void Start()
    {
        // Pega o Rigidbody2D (essencial para movimento baseado em física)
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("PrincessHealth precisa de um componente Rigidbody2D no objeto!");
            enabled = false;
            return;
        }

        // Garante que o Rigidbody não gira e ignora a gravidade
        rb.freezeRotation = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // Update é usado para coletar o input
    void Update()
    {
        // Pega o input dos eixos Horizontal e Vertical (WASD, Setas)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normaliza o vetor para que a velocidade diagonal não seja maior
        movement.Normalize();
    }

    // FixedUpdate é usado para aplicar o movimento ao Rigidbody
    void FixedUpdate()
    {
        // Move o Rigidbody para a nova posição
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // === 3. FUNÇÕES DE DANO (Chamada pelo Minotauro) ===

    // A função deve ser pública para ser chamada de outro script (MinotaurAI_2D)
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
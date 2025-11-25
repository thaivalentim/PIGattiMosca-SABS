using UnityEngine;

public class Porta : MonoBehaviour
{
    [Header("Configuração da Porta")]
    public int numeroPorta; // 0 para primeira porta, 1 para segunda porta
    
    private GerenciadorSalas gerenciador;
    private bool jaUsada = false; // Evitar múltiplas colisões
    
    void Start()
    {
        // Encontrar o gerenciador de salas na cena
        gerenciador = FindObjectOfType<GerenciadorSalas>();
    }
    
    // Quando o jogador clicar na porta (ou colidir, como preferir)
    void OnMouseDown()
    {
        Debug.Log("Clicou na porta: " + numeroPorta);
        // Avisar o gerenciador que essa porta foi escolhida
        if (gerenciador != null)
        {
            gerenciador.EscolherPorta(numeroPorta);
        }
        else
        {
            Debug.Log("Gerenciador não encontrado!");
        }
    }
    
    // Quando o jogador encostar na porta
    void OnTriggerEnter2D(Collider2D other)
    {
        // Se o jogador encostar na porta E ainda não foi usada
        if (other.CompareTag("Player") && !jaUsada)
        {
            jaUsada = true; // Marcar como usada
            Debug.Log("Jogador entrou na porta: " + numeroPorta);
            if (gerenciador != null)
            {
                gerenciador.EscolherPorta(numeroPorta);
            }
            else
            {
                Debug.Log("Gerenciador não encontrado!");
            }
        }
    }
    
    // Função para resetar a porta quando trocar de sala
    public void ResetarPorta()
    {
        jaUsada = false;
    }
}
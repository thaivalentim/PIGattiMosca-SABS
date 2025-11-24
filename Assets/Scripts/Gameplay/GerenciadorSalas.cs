using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GerenciadorSalas : MonoBehaviour
{
    [Header("Configuração das Salas")]
    public GameObject[] prefabsSalas = new GameObject[7]; // Array para os 7 prefabs de sala
    public Transform pontoSpawnSala; // Onde a sala vai aparecer na tela
    
    [Header("Estado do Jogo")]
    public int salaAtual = 0; // Qual sala estamos agora (começa na 0)
    public int itensColetados = 0; // Quantos itens já pegamos
    public int itensNecessarios = 4; // Quantos itens precisamos para ganhar
    public string proximaFase = "Fase2_Minotauro"; // Cena para onde ir após coletar itens
    
    [Header("Conexões das Salas - Configure no Inspector")]
    public int[] sala0_destinos = new int[2]; // Para onde as 2 portas da sala 0 levam
    public int[] sala1_destinos = new int[2]; // Para onde as 2 portas da sala 1 levam
    public int[] sala2_destinos = new int[2]; // E assim por diante...
    public int[] sala3_destinos = new int[2];
    public int[] sala4_destinos = new int[2];
    public int[] sala5_destinos = new int[2];
    public int[] sala6_destinos = new int[2];
    
    private GameObject salaAtiva; // Referência para a sala que está na tela agora
    
    void Start()
    {
        // Quando o jogo começar, carregar a primeira sala
        CarregarSala(0);
    }
    
    // Função para trocar de sala
    public void CarregarSala(int numeroSala)
    {
        // Se já tem uma sala na tela, destruir ela
        if (salaAtiva != null)
        {
            Destroy(salaAtiva);
        }
        
        // Criar a nova sala
        salaAtiva = Instantiate(prefabsSalas[numeroSala], pontoSpawnSala.position, Quaternion.identity);
        salaAtual = numeroSala;
        
        // Mover jogador para o centro da nova sala
        GameObject jogador = GameObject.FindWithTag("Player");
        if (jogador != null)
        {
            jogador.transform.position = pontoSpawnSala.position;
        }
        
        // Resetar todas as portas da nova sala
        Porta[] portas = salaAtiva.GetComponentsInChildren<Porta>();
        foreach (Porta porta in portas)
        {
            porta.ResetarPorta();
        }
        
        Debug.Log("Carregou sala: " + numeroSala);
    }
    
    // Função que será chamada quando o jogador clicar numa porta
    public void EscolherPorta(int numeroPorta)
    {
        // numeroPorta será 0 ou 1 (primeira ou segunda porta)
        int proximaSala = 0;
        
        // Descobrir para onde essa porta leva baseado na sala atual
        switch (salaAtual)
        {
            case 0: proximaSala = sala0_destinos[numeroPorta]; break;
            case 1: proximaSala = sala1_destinos[numeroPorta]; break;
            case 2: proximaSala = sala2_destinos[numeroPorta]; break;
            case 3: proximaSala = sala3_destinos[numeroPorta]; break;
            case 4: proximaSala = sala4_destinos[numeroPorta]; break;
            case 5: proximaSala = sala5_destinos[numeroPorta]; break;
            case 6: proximaSala = sala6_destinos[numeroPorta]; break;
        }
        
        // Ir para a próxima sala
        CarregarSala(proximaSala);
    }
    
    // Função que será chamada quando pegar um item
    public void ColetarItem()
    {
        itensColetados++;
        Debug.Log("Itens coletados: " + itensColetados + "/" + itensNecessarios);
        
        // Verificar se coletou todos os itens
        if (itensColetados >= itensNecessarios)
        {
            Debug.Log("PARABÉNS! Todos os itens coletados! Indo para o combate...");
            StartCoroutine(IrParaProximaFase());
        }
    }
    
    IEnumerator IrParaProximaFase()
    {
        // Aguardar 2 segundos para o jogador ver a mensagem
        yield return new WaitForSeconds(2f);
        
        // Carregar próxima fase
        SceneManager.LoadScene(proximaFase);
    }
}
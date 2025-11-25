using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Medusa : MonoBehaviour
{
    // === CONFIGURAÇÕES PÚBLICAS ===
    [Header("Alvo e Movimento")]
    public Transform alvo;
    public float VelocidadeMovimento = 4f;
    [Tooltip("Distância MÁXIMA para parar e atacar.")]
    public float DistanciaParada = 0.8f;
    public float AlcancePerseguicao = 12f;

    [Header("Parâmetros de Combate")]
    public float SaudeMaxima = 1500f;
    public float SaudeAtual;
    public float DanoAtaque = 10f;
    public float DanoContatoRecebido = 7f;
    public float TempoEsperaDanoContato = 0.5f;
    [Tooltip("Limiar de vida abaixo do qual a Medusa entra em Fuga (Ex: 50f).")]
    public float LimiteSaudeFuga = 50f;
    public float TaxaRegeneracao = 15f;

    [Header("Animação de Morte")]
    public string NomeAnimacaoMorteTrigger = "Die";
    public float DuracaoAnimacaoMorte = 1.5f;

    [Header("Interface")]
    public Slider BarraSaude;

    [Header("Parâmetros de Fuga")]
    [Tooltip("Distância MÍNIMA para fugir (Ex: 5f).")]
    public float DistanciaFugaSegura = 5f;

    // Variáveis que não são usadas pela Medusa, mas mantidas por compatibilidade.
    [Header("Próximo Spawn (Não Usado)")]
    public Animator EstatuaAnimator;
    public string NomeAnimacaoEstatuaInicialBool = "IsEstatuaa";
    public string NomeTriggerEstatuaFinal = "AtivarCor";
    public float DuracaoAnimacaoEstatua = 0f;
    public GameObject PrefabMedusa;
    public Transform PontoSpawnMedusa;

    // === VARIÁVEIS PRIVADAS ===
    private Rigidbody2D rb;
    private Animator animator;
    private bool estaAtacando = false;
    private float ultimoTempoDanoContato;
    private bool estaMorto = false;
    private bool estaEmContatoComPrincesa = false;
    private float tempoFimAtaque;
    private const float DURACAO_ANIMACAO_ATAQUE = 0.5f;

    private enum Estado { Ocioso, Perseguindo, Fugindo }
    private Estado estadoAtual = Estado.Ocioso;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null || animator == null)
        {
            Debug.LogError("Medusa precisa de Rigidbody2D e Animator!");
            enabled = false;
            return;
        }

        rb.freezeRotation = true;
        SaudeAtual = SaudeMaxima;
        if (BarraSaude != null)
        {
            BarraSaude.maxValue = SaudeMaxima;
            BarraSaude.minValue = 0;
            BarraSaude.value = SaudeAtual;
        }

        // A Medusa começa sem estátua visível ou ativação de estátua.
        if (EstatuaAnimator != null) EstatuaAnimator.gameObject.SetActive(false);
    }


    void Update()
    {
        if (estaMorto) return;

        float distanciaAteAlvo = alvo != null ? Vector2.Distance(transform.position, alvo.position) : float.MaxValue;

        VerificarTransicoesEstado();

        if (estaAtacando)
        {
            rb.velocity = Vector2.zero;
            if (Time.time >= tempoFimAtaque) estaAtacando = false;
            AtualizarAnimacao(distanciaAteAlvo);
            return;
        }

        switch (estadoAtual)
        {
            case Estado.Ocioso:
                rb.velocity = Vector2.zero;
                if (distanciaAteAlvo <= AlcancePerseguicao) estadoAtual = Estado.Perseguindo;
                break;
            case Estado.Perseguindo:
                GerenciarPerseguicao(distanciaAteAlvo);
                break;
            case Estado.Fugindo:
                GerenciarFuga(distanciaAteAlvo);
                RegenerarSaude();
                break;
        }

        AtualizarAnimacao(distanciaAteAlvo);
    }

    // 🎯 MÉTODOS DE LÓGICA DE IA E MOVIMENTO (ESTAVAM FALTANDO)

    private void VerificarTransicoesEstado() // 👈 MÉTODO FALTANDO
    {
        if (SaudeAtual <= LimiteSaudeFuga && estadoAtual != Estado.Fugindo)
        {
            estadoAtual = Estado.Fugindo;
            estaAtacando = false;
        }
        else if (SaudeAtual >= SaudeMaxima && estadoAtual == Estado.Fugindo)
        {
            estadoAtual = Estado.Perseguindo;
        }
    }

    private void GerenciarPerseguicao(float distanciaAteAlvo) // 👈 MÉTODO FALTANDO
    {
        if (distanciaAteAlvo <= DistanciaParada)
        {
            rb.velocity = Vector2.zero;
            AtacarAlvo();
        }
        else if (distanciaAteAlvo <= AlcancePerseguicao)
        {
            Vector2 direcao = (alvo.position - transform.position).normalized;
            rb.velocity = direcao * VelocidadeMovimento;
        }
        else
        {
            estadoAtual = Estado.Ocioso;
            rb.velocity = Vector2.zero;
        }
    }

    private void GerenciarFuga(float distanciaAteAlvo) // 👈 MÉTODO FALTANDO
    {
        if (distanciaAteAlvo > DistanciaFugaSegura || alvo == null)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            Vector2 direcao = (transform.position - alvo.position).normalized;
            rb.velocity = direcao * VelocidadeMovimento * 0.8f;
            VirarPara(rb.velocity.x);
        }
    }

    private void AtacarAlvo() // 👈 MÉTODO FALTANDO
    {
        if (estaAtacando) return;
        estaAtacando = true;
        tempoFimAtaque = Time.time + DURACAO_ANIMACAO_ATAQUE;
    }


    public void ReceberDano(float dano)
    {
        if (SaudeAtual > 0)
        {
            SaudeAtual -= dano;
            if (BarraSaude != null) BarraSaude.value = SaudeAtual;
        }

        if (SaudeAtual <= 0 && !estaMorto)
        {
            Morrer();
        }
    }

    // --- MÉTODOS AUXILIARES ---

    private void AtualizarAnimacao(float distanciaAteAlvo) // 👈 MÉTODO FALTANDO
    {
        bool estaMovendo = rb.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", estaMovendo);
        animator.SetBool("IsAttacking", estaAtacando);

        if (estaMovendo) VirarPara(rb.velocity.x);
        else if (estadoAtual == Estado.Perseguindo || estadoAtual == Estado.Ocioso) VirarPara(alvo.position.x - transform.position.x);
    }

    private void VirarPara(float direcaoX) // 👈 MÉTODO FALTANDO
    {
        if (Mathf.Abs(direcaoX) > 0.01f)
        {
            float direcaoAlvoX = direcaoX;
            if ((direcaoAlvoX > 0 && transform.localScale.x < 0) || (direcaoAlvoX < 0 && transform.localScale.x > 0))
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        if (BarraSaude != null)
        {
            float escalaOriginalBarraX = Mathf.Abs(BarraSaude.transform.localScale.x);
            if (transform.localScale.x < 0) BarraSaude.transform.localScale = new Vector3(-escalaOriginalBarraX, BarraSaude.transform.localScale.y, BarraSaude.transform.localScale.z);
            else BarraSaude.transform.localScale = new Vector3(escalaOriginalBarraX, BarraSaude.transform.localScale.y, BarraSaude.transform.localScale.z);
        }
    }

    private void RegenerarSaude() // 👈 MÉTODO FALTANDO
    {
        if (!estaEmContatoComPrincesa)
        {
            SaudeAtual = Mathf.Min(SaudeAtual + TaxaRegeneracao * Time.deltaTime, SaudeMaxima);
            if (BarraSaude != null) BarraSaude.value = SaudeAtual;
        }
    }

    // --- MÉTODOS DE COLISÃO (MANTIDOS) ---

    private void OnTriggerStay2D(Collider2D other) // 👈 MÉTODO FALTANDO
    {
        if (other.gameObject.CompareTag("Player"))
        {
            estaEmContatoComPrincesa = true;
            if (Time.time > ultimoTempoDanoContato + TempoEsperaDanoContato)
            {
                ReceberDano(DanoContatoRecebido);
                ultimoTempoDanoContato = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // 👈 MÉTODO FALTANDO
    {
        if (other.gameObject.CompareTag("Player"))
        {
            estaEmContatoComPrincesa = false;
        }
    }

    // 🛑 MÉTODO DE MORTE
    void Morrer()
    {
        if (estaMorto) return;

        estaMorto = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.SetTrigger(NomeAnimacaoMorteTrigger);

        // A Medusa não tem lógica de estátua
        // ...

        if (BarraSaude != null) BarraSaude.gameObject.SetActive(false);
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        StartCoroutine(AtrasarDestruicao(DuracaoAnimacaoMorte));
    }

    IEnumerator AtrasarDestruicao(float tempoEspera)
    {
        yield return new WaitForSeconds(tempoEspera);
        Destroy(gameObject); // Destroi a Medusa
    }

    // Este método não é usado na Medusa, mas é mantido por segurança.
    IEnumerator AtrasarDestruicaoEstatua(GameObject objetoParaDestruir, float tempoEspera)
    {
        yield return new WaitForSeconds(tempoEspera);
        Destroy(objetoParaDestruir);
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Minotauro : MonoBehaviour
{
    // === CONFIGURAÇÕES PÚBLICAS ===
    [Header("Alvo e Movimento")]
    public Transform alvo;
    public float VelocidadeMovimento = 3f;
    [Tooltip("Distância MÁXIMA para parar e atacar.")]
    public float DistanciaParada = 0.6f;
    public float AlcancePerseguicao = 10f;

    [Header("Parâmetros de Combate")]
    public float SaudeMaxima = 1000f;
    public float SaudeAtual;
    public float DanoAtaque = 5f;
    public float DanoContatoRecebido = 5f;
    public float TempoEsperaDanoContato = 0.5f;
    [Tooltip("Limiar de vida abaixo do qual o Minotauro entra em Fuga (Ex: 30f).")]
    public float LimiteSaudeFuga = 30f;
    public float TaxaRegeneracao = 10f;

    [Header("Animação de Morte")]
    public string NomeAnimacaoMorteTrigger = "Die";
    public float DuracaoAnimacaoMorte = 1.5f;

    [Header("Interface")]
    public Slider BarraSaude;

    [Header("Parâmetros de Fuga")]
    [Tooltip("Distância MÍNIMA para fugir (Ex: 5f).")]
    public float DistanciaFugaSegura = 5f;

    // 🎯 CONFIGURAÇÕES DA ESTÁTUA
    [Header("Estátua")]
    public Animator EstatuaAnimator;
    public string NomeAnimacaoEstatuaInicialBool = "IsEstatuaa";
    public string NomeTriggerEstatuaFinal = "AtivarCor";
    [Tooltip("Duração da animação EstatuaParada em segundos ANTES de ser destruída.")]
    public float DuracaoAnimacaoEstatua = 2.0f;

    // 🎯 NOVO: CONFIGURAÇÕES DA MEDUSA
    [Header("Próximo Chefe (Medusa)")]
    [Tooltip("Arraste o Prefab da Medusa aqui.")]
    public GameObject PrefabMedusa;
    [Tooltip("Posição onde a Medusa deve aparecer (Geralmente a posição da Estátua).")]
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
            Debug.LogError("Minotauro precisa de Rigidbody2D e Animator!");
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

        // LÓGICA DA ESTÁTUA NO START:
        if (EstatuaAnimator != null)
        {
            EstatuaAnimator.gameObject.SetActive(true);
            EstatuaAnimator.SetBool(NomeAnimacaoEstatuaInicialBool, true);
        }

        // Se você não definiu um ponto de spawn específico, usa a posição da Estátua.
        if (PontoSpawnMedusa == null && EstatuaAnimator != null)
        {
            PontoSpawnMedusa = EstatuaAnimator.transform;
        }
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


    void VerificarTransicoesEstado()
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


    void GerenciarPerseguicao(float distanciaAteAlvo)
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


    void GerenciarFuga(float distanciaAteAlvo)
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


    void AtacarAlvo()
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

    // --- Métodos Auxiliares e Colisões (MANTIDOS) ---

    void AtualizarAnimacao(float distanciaAteAlvo)
    {
        bool estaMovendo = rb.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", estaMovendo);
        animator.SetBool("IsAttacking", estaAtacando);

        if (estaMovendo) VirarPara(rb.velocity.x);
        else if (estadoAtual == Estado.Perseguindo || estadoAtual == Estado.Ocioso) VirarPara(alvo.position.x - transform.position.x);
    }

    void VirarPara(float direcaoX)
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

    void RegenerarSaude()
    {
        if (!estaEmContatoComPrincesa)
        {
            SaudeAtual = Mathf.Min(SaudeAtual + TaxaRegeneracao * Time.deltaTime, SaudeMaxima);
            if (BarraSaude != null) BarraSaude.value = SaudeAtual;
        }
    }

    void OnTriggerStay2D(Collider2D other)
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            estaEmContatoComPrincesa = false;
        }
    }

    // MÉTODO DE MORTE
    void Morrer()
    {
        if (estaMorto) return;

        estaMorto = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.SetTrigger(NomeAnimacaoMorteTrigger);

        // LÓGICA DA ESTÁTUA
        if (EstatuaAnimator != null)
        {
            EstatuaAnimator.gameObject.SetActive(true);
            EstatuaAnimator.SetBool(NomeAnimacaoEstatuaInicialBool, false);
            EstatuaAnimator.SetTrigger(NomeTriggerEstatuaFinal);

            // Inicia a contagem para destruir a estátua e spawnar a Medusa
            StartCoroutine(AtrasarDestruicaoEstatua(EstatuaAnimator.gameObject, DuracaoAnimacaoEstatua));
        }

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
        Destroy(gameObject); // Destroi o Minotauro
    }

    // 🎯 NOVO: Coroutine que destrói a estátua E SPAWNA A MEDUSA
    IEnumerator AtrasarDestruicaoEstatua(GameObject objetoParaDestruir, float tempoEspera)
    {
        yield return new WaitForSeconds(tempoEspera);

        // SPAWN DA MEDUSA:
        if (PrefabMedusa != null && PontoSpawnMedusa != null)
        {
            GameObject novaMedusa = Instantiate(PrefabMedusa, PontoSpawnMedusa.position, Quaternion.identity);

            // Tenta configurar o alvo da Medusa automaticamente (se ela usar o mesmo script)
            Medusa medusaScript = novaMedusa.GetComponent<Medusa>();
            if (medusaScript != null)
            {
                medusaScript.alvo = this.alvo;
            }
        }

        Destroy(objetoParaDestruir); // Destroi a Estátua
    }
}
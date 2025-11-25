using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class FimDeJogoController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fundoImagem;
    public TextMeshProUGUI textoNarracao;
    public Image fadePanel;
    
    [Header("Configurações")]
    public float velocidadeTexto = 0.05f;
    public float tempoEntreLinhas = 2f;
    public string proximaCena = "TelaInicio";
    
    private string[] linhasNarracao = {
        "Vitória! O labirinto de Dédalo foi conquistado!",
        "As criaturas das trevas foram derrotadas por sua coragem.",
        "Os artefatos sagrados brilham, reconhecendo seu valor.",
        "Você emergiu das profundezas como um verdadeiro herói.",
        "Os deuses do Olimpo sorriem para sua conquista.",
        "Sua saga ecoará pelos séculos na memória dos mortais..."
    };
    
    void Start()
    {
        if (textoNarracao != null)
            textoNarracao.color = Color.black;
            
        StartCoroutine(IniciarFimDeJogo());
    }
    
    IEnumerator IniciarFimDeJogo()
    {
        textoNarracao.text = "";
        yield return StartCoroutine(FadeIn());
        
        foreach (string linha in linhasNarracao)
        {
            textoNarracao.text = "";
            yield return StartCoroutine(MostrarTexto(linha));
            yield return new WaitForSeconds(tempoEntreLinhas);
        }
        
        yield return StartCoroutine(AguardarInput());
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(proximaCena);
    }
    
    IEnumerator MostrarTexto(string texto)
    {
        textoNarracao.text = "";
        yield return new WaitForSeconds(0.1f);
        
        foreach (char letra in texto)
        {
            textoNarracao.text += letra;
            yield return new WaitForSeconds(velocidadeTexto);
        }
    }
    
    IEnumerator FadeIn()
    {
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
    }
    
    IEnumerator FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
    
    IEnumerator AguardarInput()
    {
        textoNarracao.text = "\n\n<i>Pressione qualquer tecla para voltar ao menu...</i>";
        
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
    }
}
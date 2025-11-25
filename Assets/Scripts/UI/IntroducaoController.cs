using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class IntroducaoController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fundoImagem;
    public TextMeshProUGUI textoNarracao;
    public Image fadePanel;
    
    [Header("Configurações")]
    public float velocidadeTexto = 0.05f;
    public float tempoEntreLinhas = 2f;
    public string proximaCena = "SampleScene";
    
    private string[] linhasNarracao = {
        "Nos tempos antigos da Grécia...",
        "Uma princesa foi condenada ao terrível Labirinto de Dédalo.",
        "Nas profundezas sombrias, o Minotauro aguarda...",
        "Sua única esperança é encontrar os artefatos sagrados",
        "e escapar antes que seja tarde demais.",
        "Que os deuses a protejam nesta jornada..."
    };
    
    void Start()
    {
        // Garantir que o texto seja preto
        if (textoNarracao != null)
            textoNarracao.color = Color.black;
            
        StartCoroutine(IniciarIntroducao());
    }

    
    IEnumerator IniciarIntroducao()
    {
        // Fade in inicial
        yield return StartCoroutine(FadeIn());
        
        // Mostrar cada linha da narração
        foreach (string linha in linhasNarracao)
        {
            yield return StartCoroutine(MostrarTexto(linha));
            yield return new WaitForSeconds(tempoEntreLinhas);
        }
        
        // Aguardar input do jogador
        yield return StartCoroutine(AguardarInput());
        
        // Fade out e carregar próxima cena
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(proximaCena);
    }
    
    IEnumerator MostrarTexto(string texto)
    {
        textoNarracao.text = "";
        
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
        textoNarracao.text = "\n\n<i>Pressione qualquer tecla para continuar...</i>";
        
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
    }
}
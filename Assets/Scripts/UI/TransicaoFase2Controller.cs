using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class TransicaoFase2Controller : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fundoImagem;
    public TextMeshProUGUI textoNarracao;
    public Image fadePanel;
    
    [Header("Configurações")]
    public float velocidadeTexto = 0.05f;
    public float tempoEntreLinhas = 2f;
    public string proximaCena = "Combate";
    
    private string[] linhasNarracao = {
        "Os artefatos sagrados pulsam com energia divina...",
        "O poder dos deuses antigos flui através de você.",
        "Nas profundezas do labirinto, uma presença sombria desperta...",
        "O rugido do Minotauro ecoa pelas pedras milenares.",
        "Chegou a hora do confronto que decidirá seu destino!"
    };
    
    void Start()
    {
        if (textoNarracao != null)
            textoNarracao.color = Color.black;
            
        StartCoroutine(IniciarTransicao());
    }
    
    IEnumerator IniciarTransicao()
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
        textoNarracao.text = "\n\n<i>Pressione qualquer tecla para continuar...</i>";
        
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
    }
}
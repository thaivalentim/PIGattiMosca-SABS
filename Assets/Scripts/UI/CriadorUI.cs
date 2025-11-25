using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CriadorUI : MonoBehaviour
{
    [Header("Assets")]
    public Sprite imagemFundo;
    public TMP_FontAsset fonteCinzel;
    
    [ContextMenu("Criar UI")]
    public void CriarElementosUI()
    {
        // Criar Fundo
        GameObject fundoObj = new GameObject("FundoImagem");
        fundoObj.transform.SetParent(transform);
        Image fundoImage = fundoObj.AddComponent<Image>();
        if (imagemFundo != null) fundoImage.sprite = imagemFundo;
        
        RectTransform rectFundo = fundoObj.GetComponent<RectTransform>();
        rectFundo.anchorMin = Vector2.zero;
        rectFundo.anchorMax = Vector2.one;
        rectFundo.offsetMin = Vector2.zero;
        rectFundo.offsetMax = Vector2.zero;
        
        // Criar Texto
        GameObject textoObj = new GameObject("TextoNarracao");
        textoObj.transform.SetParent(transform);
        TextMeshProUGUI texto = textoObj.AddComponent<TextMeshProUGUI>();
        texto.text = "Texto de teste";
        texto.fontSize = 28;
        texto.color = Color.black;
        texto.alignment = TextAlignmentOptions.Center;
        if (fonteCinzel != null) texto.font = fonteCinzel;
        
        RectTransform rectTexto = textoObj.GetComponent<RectTransform>();
        rectTexto.anchorMin = new Vector2(0.1f, 0.2f);
        rectTexto.anchorMax = new Vector2(0.9f, 0.8f);
        rectTexto.offsetMin = Vector2.zero;
        rectTexto.offsetMax = Vector2.zero;
        
        // Criar Fade Panel
        GameObject fadeObj = new GameObject("FadePanel");
        fadeObj.transform.SetParent(transform);
        Image fadeImage = fadeObj.AddComponent<Image>();
        fadeImage.color = Color.black;
        
        RectTransform rectFade = fadeObj.GetComponent<RectTransform>();
        rectFade.anchorMin = Vector2.zero;
        rectFade.anchorMax = Vector2.one;
        rectFade.offsetMin = Vector2.zero;
        rectFade.offsetMax = Vector2.zero;
        
        fadeObj.transform.SetAsLastSibling();
        
        Debug.Log("UI criada com sucesso!");
    }
}
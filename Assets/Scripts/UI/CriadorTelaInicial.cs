using UnityEngine;
using UnityEngine.UI;

public class CriadorTelaInicial : MonoBehaviour
{
    [Header("Assets")]
    public Sprite telaInicialSprite;
    
    [ContextMenu("Criar Tela Inicial")]
    public void CriarTelaInicial()
    {
        // Criar Imagem da Tela Inicial
        GameObject telaObj = new GameObject("TelaInicial");
        telaObj.transform.SetParent(transform);
        Image telaImage = telaObj.AddComponent<Image>();
        if (telaInicialSprite != null) telaImage.sprite = telaInicialSprite;
        
        RectTransform rectTela = telaObj.GetComponent<RectTransform>();
        rectTela.anchorMin = Vector2.zero;
        rectTela.anchorMax = Vector2.one;
        rectTela.offsetMin = Vector2.zero;
        rectTela.offsetMax = Vector2.zero;
        
        // Criar Fade Panel
        GameObject fadeObj = new GameObject("FadePanel");
        fadeObj.transform.SetParent(transform);
        Image fadeImage = fadeObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
        
        RectTransform rectFade = fadeObj.GetComponent<RectTransform>();
        rectFade.anchorMin = Vector2.zero;
        rectFade.anchorMax = Vector2.one;
        rectFade.offsetMin = Vector2.zero;
        rectFade.offsetMax = Vector2.zero;
        
        fadeObj.transform.SetAsLastSibling();
        fadeObj.SetActive(false);
        
        Debug.Log("Tela Inicial criada!");
    }
}
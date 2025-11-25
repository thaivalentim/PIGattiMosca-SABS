using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TelaInicialController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image imagemTelaInicial;
    public Image fadePanel;
    
    [Header("Configurações")]
    public string cenaIntroducao = "Introducao";
    
    void Start()
    {
        StartCoroutine(AguardarInput());
    }
    
    IEnumerator AguardarInput()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(cenaIntroducao);
    }
    
    IEnumerator FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        float alpha = 0f;
        
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 2f;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EfeitosCinematograficos : MonoBehaviour
{
    [Header("Efeitos Visuais")]
    public Image fundoImagem;
    public float intensidadePulsacao = 0.1f;
    public float velocidadePulsacao = 1f;
    
    private Color corOriginal;
    
    void Start()
    {
        if (fundoImagem != null)
        {
            corOriginal = fundoImagem.color;
            StartCoroutine(EfeitoPulsacao());
        }
    }
    
    IEnumerator EfeitoPulsacao()
    {
        while (true)
        {
            // Escurecer levemente
            float alpha = Mathf.Lerp(1f, 1f - intensidadePulsacao, 
                (Mathf.Sin(Time.time * velocidadePulsacao) + 1f) / 2f);
            
            fundoImagem.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, alpha);
            
            yield return null;
        }
    }
    
    public void PararEfeitos()
    {
        StopAllCoroutines();
        if (fundoImagem != null)
            fundoImagem.color = corOriginal;
    }
}
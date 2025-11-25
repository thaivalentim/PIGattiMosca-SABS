using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class armazenamentoDeItens : MonoBehaviour
{
    public List<string> itensPegos = new List<string>();

    void Start()
    {

    }
    void Update()
    {
        
    }
    public void DetectarLimite()
    {
        if(itensPegos.Count >= 4)
        {
            StartCoroutine(IrParaProximaFase());
        }
    }

    IEnumerator IrParaProximaFase()
    {
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene("Combate");
    }
    public void PegarItem(string item)
    {
        {
            if (!itensPegos.Contains(item))
            {
                itensPegos.Add(item);
                Debug.Log("Item adicionado: " + item);
            }
            else
            {
                Debug.Log("Item já existe na lista!");
            }
        }
    }
}

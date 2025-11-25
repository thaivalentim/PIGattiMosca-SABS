using System;
using System.Collections;
using System.Collections.Generic;
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
        if(itensPegos.Count >= 7)
        {
            Debug.Log("Pegou todos os itens, aplicar a lógica de chutar pra sala de boss e minotauro!");
        }
    }
    public void PegarItem(string item)
    {
        foreach(var itemPego in itensPegos)
        {
            if (itemPego == item)
            {
                return;
            }
            itensPegos.Add(item);
        }
    }
}

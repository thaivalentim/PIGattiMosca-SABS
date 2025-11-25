using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicaPegarItem : MonoBehaviour
{
    public armazenamentoDeItens itemManager;
    public GameObject item;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado com: " + other.gameObject.name);
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("ITEM COLETADO!");
            
            // Sistema antigo
            if (itemManager != null)
            {
                other.GetComponent<armazenamentoDeItens>();
                itemManager.PegarItem(item.name);
                itemManager.DetectarLimite();
            }
            
            // Sistema novo - GerenciadorSalas
            GerenciadorSalas gerenciador = FindObjectOfType<GerenciadorSalas>();
            if (gerenciador != null)
            {
                Debug.Log("Chamando ColetarItem no GerenciadorSalas");
                gerenciador.ColetarItem();
            }
            else
            {
                Debug.Log("GerenciadorSalas não encontrado!");
            }
            
            Destroy(gameObject);
        }
    }
}

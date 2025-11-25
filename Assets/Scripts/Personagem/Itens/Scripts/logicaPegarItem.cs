using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicaPegarItem : MonoBehaviour
{
    public GameObject gameManager;
    public armazenamentoDeItens itemManager;
    public GameObject item;
    public Rigidbody2D rb;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController");
        itemManager = gameManager.GetComponent<armazenamentoDeItens>();
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado com: " + other.gameObject.name);
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("ITEM COLETADO!");
            
            if (itemManager != null)
            {
                other.GetComponent<armazenamentoDeItens>();
                itemManager.PegarItem(item.name);
                itemManager.DetectarLimite();
            }
            Destroy(gameObject);
        }
    }
}

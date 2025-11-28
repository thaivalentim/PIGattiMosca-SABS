using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.Tilemaps;

public class armazenamentoDeItens : MonoBehaviour
{
    public List<string> itensPegos = new List<string>();
    public GameObject player;

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
        yield return new WaitForSeconds(0.5f);
        
        player = GameObject.FindWithTag("Player");
        player.transform.position = new Vector3(45, -22, 0);
        player.transform.localScale = new Vector3(1, 1, 1);   
        AudioManager.instance.PlayMusicaCombate1();
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

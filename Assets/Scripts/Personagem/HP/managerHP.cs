using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class managerHP : MonoBehaviour
{
    public int hpMaximo = 20;
    public int hpAtual;
    public GameObject manager;
    void Start()
    {
        hpAtual = hpMaximo;
    }
    public void TomarDano(int dano) // sempre que tomar dano, puxa essa função e coloca o valor do dano junto
    {
        hpAtual -= dano;
        if (hpAtual <= 0)
        {
            Morrer();
        }
    }
    void Morrer()
    {
        Debug.Log("Jogador morreu.");
        SceneManager.LoadScene("GameOver"); // tem que criar uma cena pro gameover, e de lá puxar pro menu de volta
    }

    public void RecuperarVida(int cura) // sempre que for curar (tipo, pegar algo), puxa essa função e coloca o valor da cura junto
    {
        hpAtual += cura;
        if (hpAtual > hpMaximo)
        {
            hpAtual = hpMaximo;
        }
    }
}

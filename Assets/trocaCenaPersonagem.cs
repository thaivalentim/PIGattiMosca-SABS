using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trocaCenaPersonagem : MonoBehaviour
{
    public GameObject princesa;
    public GameObject teseu;
    public void CarregarCena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase1-Labirinto");
    }
    public void CarregarTeseu()
    {
        GameObject personagem = Instantiate(teseu, new Vector3(0, 0, 0), Quaternion.identity);
        DontDestroyOnLoad(personagem);
    }
    public void CarregarPrincesa()
    {
        GameObject personagem = Instantiate(princesa, new Vector3(0, 0, 0), Quaternion.identity);
        DontDestroyOnLoad(personagem);
    }
}

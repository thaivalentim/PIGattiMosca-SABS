using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configurações")]
    public Transform jogador;
    public float suavidade = 2f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    void LateUpdate()
    {
        if (jogador != null)
        {
            Vector3 posicaoDesejada = jogador.position + offset;
            transform.position = Vector3.Lerp(transform.position, posicaoDesejada, suavidade * Time.deltaTime);
        }
    }
    
    public void DefinirJogador(Transform novoJogador)
    {
        jogador = novoJogador;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicaFundo;
    public AudioSource musicaCombate1;
    public AudioSource musicaCombate2;
    public AudioSource somMorteMinotauro;
    public AudioSource somMedusaAwake;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        PlayMusicaFundo();
    }

    public void PararMusicas()
    {
        musicaFundo.Stop();
        musicaCombate1.Stop();
        musicaCombate2.Stop();
    }
    public void PlayMusicaFundo()
    {
        musicaFundo.Play();
        musicaCombate1.Stop();
        musicaCombate2.Stop();
    }

    public void PlayMusicaCombate1()
    {
        musicaCombate1.Play();
        musicaFundo.Stop();
        musicaCombate2.Stop();
    }

    public void PlayMusicaCombate2()
    {
        musicaCombate2.Play();
        musicaFundo.Stop();
        musicaCombate1.Stop();
    }

    public void TocarMorteMinotauro()
    {
        somMorteMinotauro.Play();
    }

    public void TocarMedusaAwake()
    {
        somMedusaAwake.Play();
    }
}

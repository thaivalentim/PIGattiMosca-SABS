using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Configurações do Jogo")]
    public bool mostrarIntroducao = true;
    public string cenaIntroducao = "Introducao";
    public string cenaJogo = "SampleScene";
    
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Se for a primeira vez, mostrar introdução
        if (mostrarIntroducao && !PlayerPrefs.HasKey("IntroducaoVista"))
        {
            IniciarIntroducao();
        }
    }
    
    public void IniciarIntroducao()
    {
        SceneManager.LoadScene(cenaIntroducao);
    }
    
    public void IniciarJogo()
    {
        PlayerPrefs.SetInt("IntroducaoVista", 1);
        SceneManager.LoadScene(cenaJogo);
    }
    
    public void ReiniciarJogo()
    {
        PlayerPrefs.DeleteKey("IntroducaoVista");
        SceneManager.LoadScene(cenaIntroducao);
    }
}
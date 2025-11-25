using UnityEngine;

public class TochaAnimator : MonoBehaviour
{
    [Header("Sprites da Animação")]
    public Sprite[] frames;
    
    [Header("Configuração")]
    public float frameRate = 0.1f;
    
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
        }
    }
}
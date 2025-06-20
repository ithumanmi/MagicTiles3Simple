using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    public Sprite backgroundSprite;
    public Sprite decorSprite;
    public Camera mainCamera;
    
    void Start()
    {
        SetupBackground();
        // Nếu decorSprite có, bắt đầu hiệu ứng chớp chớp
        if (decorSprite != null)
        {
            StartCoroutine(BlinkDecor());
        }
    }
    
    void SetupBackground()
    {
        // Tạo GameObject cho background
        GameObject background = new GameObject("Background");
        GameObject decor = new GameObject("decor");
        SpriteRenderer sr = background.AddComponent<SpriteRenderer>();
        SpriteRenderer decorsr = decor.AddComponent<SpriteRenderer>();
        
        // Gán sprite
        if (backgroundSprite != null)
        {
            sr.sprite = backgroundSprite;
        }
        if (decorSprite != null)
        {
            decorsr.sprite = decorSprite;
        }
        // Đặt background ở dưới cùng
        sr.sortingOrder = -1;
        decorsr.sortingOrder = 0;
        
        // Scale background để vừa màn hình
        if (mainCamera != null)
        {
            float screenHeight = 2f * mainCamera.orthographicSize;
            float screenWidth = screenHeight * mainCamera.aspect;
            
            float spriteHeight = sr.sprite.bounds.size.y;
            float spriteWidth = sr.sprite.bounds.size.x;
            
            float scaleX = screenWidth / spriteWidth;
            float scaleY = screenHeight / spriteHeight;
            float scale = Mathf.Max(scaleX, scaleY);
            
            background.transform.localScale = new Vector3(scale, scale, 1f);
        }
        
        // Đặt vị trí
        background.transform.position = new Vector3(0f, 0f, 1f);
        decor.transform.position = new Vector3(0f, 0f, 0.5f);
        decor.transform.localScale = Vector3.one * 3f;
        // Lưu lại decor renderer để dùng cho hiệu ứng
        _decorRenderer = decorsr;
    }
    
    private SpriteRenderer _decorRenderer;
    private IEnumerator BlinkDecor()
    {
        float duration = 0.5f; // thời gian 1 chu kỳ chớp
        float minAlpha = 0.2f;
        float maxAlpha = 1f;
        while (true)
        {
            // Fade in
            float t = 0f;
            while (t < duration/2f)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(minAlpha, maxAlpha, t/(duration/2f));
                SetDecorAlpha(alpha);
                yield return null;
            }
            // Fade out
            t = 0f;
            while (t < duration/2f)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(maxAlpha, minAlpha, t/(duration/2f));
                SetDecorAlpha(alpha);
                yield return null;
            }
        }
    }
    private void SetDecorAlpha(float alpha)
    {
        if (_decorRenderer != null)
        {
            Color c = _decorRenderer.color;
            c.a = alpha;
            _decorRenderer.color = c;
        }
    }
    
    // Load background từ Resources
    public void LoadBackgroundFromResources(string path)
    {
        Sprite bgSprite = Resources.Load<Sprite>(path);
        if (bgSprite != null)
        {
            backgroundSprite = bgSprite;
            SetupBackground();
        }
    }
} 
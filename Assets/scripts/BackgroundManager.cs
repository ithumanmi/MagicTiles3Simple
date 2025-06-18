using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Sprite backgroundSprite;
    public Camera mainCamera;
    
    void Start()
    {
        SetupBackground();
    }
    
    void SetupBackground()
    {
        // Tạo GameObject cho background
        GameObject background = new GameObject("Background");
        SpriteRenderer sr = background.AddComponent<SpriteRenderer>();
        
        // Gán sprite
        if (backgroundSprite != null)
        {
            sr.sprite = backgroundSprite;
        }
        
        // Đặt background ở dưới cùng
        sr.sortingOrder = -1;
        
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
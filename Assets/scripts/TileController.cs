using UnityEngine;
using DG.Tweening;

public class TileController : MonoBehaviour
{
    [Header("Movement")]
    public float fallSpeed = 5f;
    
    [Header("Hit Zones")]
    public float perfectY = -3.5f; // Vị trí Y cho "Perfect"
    public float goodWindow = 0.5f; // Khoảng cho "Good"
    public float perfectWindow = 0.2f; // Khoảng cho "Perfect"
    
    [Header("Visual Feedback")]
    public GameObject perfectEffect;
    public GameObject goodEffect;
    public GameObject missEffect;
    public GameObject tapEffectPrefab; // Gán là child object trong prefab
    public GameObject tapEffectBorderPrefab; // Gán là child object trong prefab
    public ParticleSystem hitParticle; // Hiệu ứng particle khi chạm tile

    private bool isActive = true;
    private SpriteRenderer spriteRenderer;
    private Vector3 _tapEffectOriginalScale;
    private int laneIndex = -1;
   
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FitWidthToColumn();
        // Đảm bảo có Collider2D
        if (GetComponent<Collider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();
        // Lưu lại scale gốc của tapEffectPrefab
        if (tapEffectPrefab != null)
        {
            _tapEffectOriginalScale = tapEffectPrefab.transform.localScale;
        }
    }
    
    // Reset state when getting from pool
    public void ResetState()
    {
        isActive = true;
        if (tapEffectPrefab != null)
            tapEffectPrefab.SetActive(false);
        if (tapEffectBorderPrefab != null)
            tapEffectBorderPrefab.SetActive(false);
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = Color.white;
        }
        if (hitParticle != null)
        {
            hitParticle.Stop();
            hitParticle.gameObject.SetActive(false);
        }
        FitWidthToColumn();
        transform.localScale = Vector3.one;
    }
    
    void FitWidthToColumn()
    {
        int gridWidth = 4;
        var gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
            gridWidth = gridManager.gridWidth;
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;
        float columnWidth = screenWidth / gridWidth;
        if (spriteRenderer != null)
        {
            float spriteWidth = spriteRenderer.bounds.size.x / transform.localScale.x;
            float scaleX = columnWidth / spriteWidth;
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
    }
    
    void Update()
    {
        
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        
        // Lấy đáy camera
        float cameraBottom = -Camera.main.orthographicSize;
        float tileHeight = 1f;
        if (spriteRenderer != null)
            tileHeight = spriteRenderer.bounds.size.y;
        float tileBottom = transform.position.y + tileHeight;
        if (tileBottom < cameraBottom)
        {
            Missed();
        }
    }
    
    void OnMouseDown()
    {
        if (!isActive) return;
        isActive = false; // Dừng fall ngay lập tức

        // Thêm điểm và tăng combo
        GameManager.Instance.AddCombo(1);
        
        // Hide original tile
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // Kích hoạt particle effect
        if (hitParticle != null)
        {
            hitParticle.gameObject.SetActive(true);
            hitParticle.Play();
        }
        if (tapEffectBorderPrefab != null)
            tapEffectBorderPrefab.SetActive(true);
        // Show tap effect and start scale down animation bằng DOTween
        if (tapEffectPrefab != null)
        {
            tapEffectPrefab.SetActive(true);
            tapEffectPrefab.transform.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack);
                
        }
        
    }
    
    void Missed()
    {
        if(isActive)
        {
            GameManager.Instance.GameOver();
        }
        if (hitParticle != null)
        {
            hitParticle.Stop();
            hitParticle.gameObject.SetActive(false);
        }
        ResetState();
        DeactivateAndReturnToPool();
    }
    
    void ShowEffect(GameObject effectPrefab)
    {
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f); // Destroy effect after 2 seconds
        }
    }
    
    // Visual feedback for tile state
    public void SetTileColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
    
    // Optional: Add glow effect when approaching hit zone
    public void AddGlowEffect()
    {
        if (spriteRenderer != null)
        {
            // Add a simple glow by changing material or adding a light
            spriteRenderer.material.SetFloat("_EmissionIntensity", 0.5f);
        }
    }
    
    void DeactivateAndReturnToPool()
    {
        isActive = false;
        if (tapEffectPrefab != null)
        {
            tapEffectPrefab.SetActive(false);
            tapEffectPrefab.transform.localScale = _tapEffectOriginalScale;
        }
        if (tapEffectBorderPrefab != null)
            tapEffectBorderPrefab.SetActive(false);
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        if (laneIndex >= 0 && TileSpawner.Instance != null) TileSpawner.Instance.SetLaneFree(laneIndex);
        TilePooler.ReturnTile(gameObject, false); // false = tile thường
    }
}

using UnityEngine;
using DG.Tweening;

public class TileControllerHoldToTop : BaseTileController
{
    [Header("Guide Line")]
    public Transform startPoint; // Gán là vị trí ngôi sao
    public Transform endPoint;   // Gán là vị trí kết thúc (có thể là empty object ở đỉnh tile)

    [Header("Star Object")]
    public GameObject starObject; // Ngôi sao (con của tile hold)
    public GameObject slideEffectPrefab; // Prefab hiệu ứng trượt
    private GameObject slideEffectInstance;

    [Header("Blinking Effect")]
    public float blinkSpeed = 0.2f; // Tốc độ nhấp nháy
    private bool isBlinking = false;
    private float blinkTimer = 0f;
    private Color originalColor; // Lưu màu gốc
    private Color blinkColor = Color.yellow; // Màu nhấp nháy

    public ArrowTouchHandler arrow;
    private bool isHolding = false;
    private float originalY;
    public Transform star; // Ngôi sao (con của tile hold)
    private bool isHoldingArrow = false;
    private bool isPointerInside = false; // Để kiểm tra khi chạm vào star

    protected override void Start()
    {
        originalY = transform.localScale.y;
        base.Start();
        originalColor = spriteRenderer.color; // Lưu màu gốc
        arrow.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        // Nếu đang giữ, cập nhật hiệu ứng trượt
        if (isHolding && slideEffectInstance != null)
        {
            slideEffectInstance.transform.position = starObject.transform.position;
        }
        // Nếu đang giữ star, kiểm tra tile hold đã đến endPoint chưa
        if (isHoldingArrow)
        {
            if (arrow.transform.position.y >= endPoint.position.y)
            {
                isActive = false; 
                GameManager.Instance.AddCombo(1);
                ReleaseArrow();
                StartBlinking(); // Bắt đầu hiệu ứng nhấp nháy
            }
        }
        // Xử lý hiệu ứng nhấp nháy
        if (isBlinking)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkSpeed)
            {
                blinkTimer = 0f;
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = (spriteRenderer.color == originalColor) ? blinkColor : originalColor;
                }
            }
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        isHolding = false;
        isBlinking = false;
        blinkTimer = 0f;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }
        transform.localScale = new Vector3(transform.localScale.x, _originalScaleY, transform.localScale.z);
    }

    protected override void DeactivateAndReturnToPool()
    {
        isActive = false;
        isPointerInside = false;
        isBlinking = false;
        arrow.isArrowHeld = false;
        arrow.gameObject.SetActive(false);  
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        if (laneIndex >= 0 && TileSpawner.Instance != null) TileSpawner.Instance.SetLaneFree(laneIndex);
        TilePooler.ReturnTile(gameObject, true); // true = tile hold
    }

    // Các hàm riêng cho tile hold giữ nguyên
    public void OnStarPointerDown()
    {
        isHoldingArrow = true;
        if(isPointerInside) return;
        if (arrow != null)
        {
            arrow.gameObject.SetActive(true);
            arrow.transform.position = star.transform.position;
            arrow.transform.SetParent(null, true);
        }
        isPointerInside = true;
        MusicManager.Instance.PlaySFX(MusicManager.Instance.slideTileHoldClip);
    }
    public void OnArrowPointerDown()
    {
        isHoldingArrow = true;
        arrow.transform.SetParent(null, true);
    }
    public void OnArrowPointerUp()
    {
        isHoldingArrow = false;
        ReleaseArrow();
    }
    void ReleaseArrow()
    {
        isHoldingArrow = false;
        arrow.gameObject.transform.SetParent(transform, true);
    }
    void StartBlinking()
    {
        isBlinking = true;
        blinkTimer = 0f;
    }
} 
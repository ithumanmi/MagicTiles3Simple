using UnityEngine;
using DG.Tweening;

public class TileControllerHoldToTop : MonoBehaviour
{
    [Header("Movement")]
    public float fallSpeed = 5f;

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
    
    private bool isActive = true;
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;
    private bool isHolding = false;
    private int laneIndex = -1;
    private float originalY; // hoặc lưu lại giá trị gốc khi khởi tạo
    private float _originalScaleY;
    public Transform star; // Ngôi sao (con của tile hold)
    private bool isHoldingArrow = false;
    private bool isPointerInside = false; // Để kiểm tra khi chạm vào star

    private void Awake()
    {
        originalY = transform.localScale.y; // Lưu lại giá trị gốc của Y scale
   
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Lưu màu gốc
        FitWidthToColumn();
        if (GetComponent<Collider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();

        arrow.gameObject.SetActive(false);
        mainCam = Camera.main;
        _originalScaleY = transform.localScale.y;
    }


    void Update()
    {
       
        // Tile rơi bình thường
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Nếu đang giữ, cập nhật hiệu ứng trượt
        if (isHolding && slideEffectInstance != null)
        {
            slideEffectInstance.transform.position = starObject.transform.position;
            // Có thể update hướng, scale, v.v. nếu muốn
        }

        // Nếu đang giữ và đã ra khỏi collider, trigger điểm

        // Nếu tile rơi khỏi màn hình thì tự hủy hoặc trả về pool
        float cameraBottom = -mainCam.orthographicSize;
        float tileHeight = 1f;
        if (spriteRenderer != null)
            tileHeight = spriteRenderer.bounds.size.y;
        float tileBottom = transform.position.y + tileHeight;
        if (tileBottom < cameraBottom)
        {
            Missed();
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
                    // Thay đổi màu giữa màu gốc và màu nhấp nháy
                    spriteRenderer.color = (spriteRenderer.color == originalColor) ? blinkColor : originalColor;
                }
            }
        }

    }

    void Missed()
    {
        if(isActive)
        {
            GameManager.Instance.GameOver();
        }
        DeactivateAndReturnToPool();
    }

    void StartBlinking()
    {
        isBlinking = true;
        blinkTimer = 0f;
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
            transform.localScale = new Vector3(scaleX, originalY, transform.localScale.z);
        }
    }

    public void ResetState()
    {
        isActive = true;
        isHolding = false;
        isBlinking = false; // Reset trạng thái nhấp nháy
        blinkTimer = 0f;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor; // Khôi phục màu gốc
        }
      
        FitWidthToColumn();
        transform.localScale = new Vector3(transform.localScale.x, _originalScaleY, transform.localScale.z);

    }

    void DeactivateAndReturnToPool()
    {
        isActive = false;
        isPointerInside = false; // Reset trạng thái pointer
        isBlinking = false; // Dừng hiệu ứng nhấp nháy
        arrow.isArrowHeld = false; // Reset trạng thái của arrow
        arrow.gameObject.SetActive(false);  
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true; // Đảm bảo sprite được hiển thị khi trả về pool
        }
        if (laneIndex >= 0 && TileSpawner.Instance != null) TileSpawner.Instance.SetLaneFree(laneIndex);
        TilePooler.ReturnTile(gameObject, true); // true = tile hold
    }

    public interface ITileLaneSetter { void SetLaneIndex(int lane); }
    public void SetLaneIndex(int lane) { laneIndex = lane; }

//#if UNITY_ANDROID || UNITY_IOS
//    void OnTouch()
//    {
//        if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            Vector3 worldPos = mainCam.ScreenToWorldPoint(touch.position);
//            Vector2 point = new Vector2(worldPos.x, worldPos.y);
//            if (touch.phase == TouchPhase.Began && starArea != null && starArea.OverlapPoint(point))
//            {
//                StartHold();
//                isPointerInside = true;
//            }
//            else if (touch.phase == TouchPhase.Moved && isHolding && starArea != null)
//            {
//                bool nowInside = starArea.OverlapPoint(point);
//                if (isPointerInside && !nowInside)
//                {
//                    isPointerInside = false;
//                }
//            }
//            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isHolding && isPointerInside && !hasTriggered)
//            {
//                StopHold();
//            }
//        }
//    }
//    void LateUpdate() { OnTouch(); }
//#endif


    // Khi user chạm vào star
    public void OnStarPointerDown()
    {
        isHoldingArrow = true;
        if(isPointerInside) return; // Nếu đã giữ star thì không làm gì nữa
        if (arrow != null)
        {
            arrow.gameObject.SetActive(true);
            arrow.transform.position = star.transform.position;
            arrow.transform.SetParent(null, true);
        }
        isPointerInside = true; // Đánh dấu là đang giữ star
        MusicManager.Instance.PlaySFX(MusicManager.Instance.slideTileHoldClip); // Phát âm thanh click tile
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
    // Khi cần nhả star (đến endPoint hoặc user thả tay)
    void ReleaseArrow()
    {
        isHoldingArrow = false;
        // Gắn lại star vào tile hold
        arrow.gameObject.transform.SetParent(transform, true);
        // Đặt lại vị trí local nếu cần
        // star.localPosition = ...;
    }
} 
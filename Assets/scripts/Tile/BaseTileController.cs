using UnityEngine;

public abstract class BaseTileController : MonoBehaviour
{
    [Header("Movement")]
    public float fallSpeed = 5f;

    protected bool isActive = true;
    protected SpriteRenderer spriteRenderer;
    protected int laneIndex = -1;
    protected float _originalScaleY;


    protected virtual void Start()
    {
    
        spriteRenderer = GetComponent<SpriteRenderer>();
  
        FitWidthToColumn();
        if (GetComponent<Collider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();
    }

    private void Awake()
    {
        _originalScaleY = transform.localScale.y;
    }

    public virtual void ResetState()
    {
        isActive = true;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = Color.white;
        }
        FitWidthToColumn();
        transform.localScale = new Vector3(transform.localScale.x, _originalScaleY, transform.localScale.z);
    }

    protected virtual void FitWidthToColumn()
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
            transform.localScale = new Vector3(scaleX, _originalScaleY, transform.localScale.z);
        }
    }

    public void SetLaneIndex(int lane) { laneIndex = lane; }

    protected virtual void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
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

    protected virtual void Missed()
    {
        if (isActive)
        {
            GameManager.Instance.GameOver();
        }
        ResetState();
        DeactivateAndReturnToPool();
    }

    protected abstract void DeactivateAndReturnToPool();
} 
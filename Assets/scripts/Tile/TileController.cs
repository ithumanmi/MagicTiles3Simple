using UnityEngine;
using DG.Tweening;

public class TileController : BaseTileController
{
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

    private Vector3 _tapEffectOriginalScale;
   
    protected override void Start()
    {
        base.Start();
        // Lưu lại scale gốc của tapEffectPrefab
        if (tapEffectPrefab != null)
        {
            _tapEffectOriginalScale = tapEffectPrefab.transform.localScale;
        }
    }
    
    public override void ResetState()
    {
        base.ResetState();
        if (tapEffectPrefab != null)
            tapEffectPrefab.SetActive(false);
        if (tapEffectBorderPrefab != null)
            tapEffectBorderPrefab.SetActive(false);
        if (hitParticle != null)
        {
            hitParticle.Stop();
            hitParticle.gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (!isActive) return;
        isActive = false; // Dừng fall ngay lập tức
        // Thêm điểm và tăng combo
        GameManager.Instance.AddCombo(1);
        MusicManager.Instance.PlaySFX(MusicManager.Instance.clickTileClip); // Phát âm thanh click tile
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

    protected override void DeactivateAndReturnToPool()
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

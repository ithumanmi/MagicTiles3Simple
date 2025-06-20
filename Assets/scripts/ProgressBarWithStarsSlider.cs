using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBarWithStarsSlider : MonoBehaviour
{
    [Header("Progress Bar")]
    public Slider progressSlider; // Slider UI

    [Header("Stars")]
    public Image[] starImages; // Các ngôi sao
    public Sprite starOn, starOff; // Sprite vàng và trắng

    public Image[] starShow;

    public void UpdateProgress(int score)
    {
        int[] starThresholds = GameManager.Instance.starThresholds;
        if (starThresholds == null || starThresholds.Length == 0)
        {
            Debug.LogError("Star thresholds are not set in GameManager!");
            return;
        }

        // Tween fill bar
        float targetFill = Mathf.Clamp01((float)score / starThresholds[starThresholds.Length - 1]);

        // Tween giá trị slider
        progressSlider.DOValue(targetFill, 0.5f).SetEase(Ease.OutCubic);

        // Ẩn tất cả starShow trước khi show mới
        HideStarShow();

        // Cập nhật trạng thái sao và hiệu ứng
        for (int i = 0; i < starImages.Length; i++)
        {
            if (score >= starThresholds[i])
            {
                if (starImages[i].sprite != starOn)
                {
                    starImages[i].sprite = starOn;
                    // Hiệu ứng scale khi đạt sao mới
                    starImages[i].transform.DOKill();
                    starImages[i].transform.localScale = Vector3.one * 1.5f;
                    starImages[i].transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);

                
                    ShowStartShow(i);

                    if (!GameManager.Instance.IsEasy)
                    {
                        GameManager.Instance.beatInterval -= 0.1f; // Giảm thời gian giữa các nhịp khi đạt sao mới
                    }
                }
             
            }
            else
            {
                starImages[i].sprite = starOff;
                starImages[i].transform.localScale = Vector3.one;
            }
        }
    }

    void ShowStartShow(int index)
    {
        for (int i = 0; i < starShow.Length; i++)
        {
            if (i <= index)
            {
                starShow[i].transform.DOKill();
                starShow[i].transform.DOScale(2f, 0.3f)
                .SetDelay(0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    HideStarShow();
                });
            }
        }
    }

    // Thêm hàm ẩn tất cả starShow
    void HideStarShow()
    {
        for (int i = 0; i < starShow.Length; i++)
        {
            starShow[i].transform.DOKill();
            starShow[i].transform.DOScale(0f, 0.2f).SetDelay(0.5f).SetEase(Ease.InBack);
        }
    }
}
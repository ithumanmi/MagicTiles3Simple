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

    [Header("Star Thresholds")]
    public int[] starThresholds; // Mốc điểm cho từng sao

    public void UpdateProgress(int score)
    {
         
        // Tween fill bar
        float targetFill = Mathf.Clamp01((float)score / starThresholds[starThresholds.Length - 1]);

        // Tween giá trị slider
        progressSlider.DOValue(targetFill, 0.5f).SetEase(Ease.OutCubic);

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
                }
            }
            else
            {
                starImages[i].sprite = starOff;
                starImages[i].transform.localScale = Vector3.one;
            }
        }
    }
}
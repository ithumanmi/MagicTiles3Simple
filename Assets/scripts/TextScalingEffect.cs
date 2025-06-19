using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextScalingEffect : MonoBehaviour
{
    [SerializeField] List<Transform> _texts; // Tham chiếu đến TMP Text
    [SerializeField] CanvasGroup _canvasGroup;

    private List<Sequence> _sequences = new List<Sequence>();

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Play()
    {
        foreach (var text in _texts)
        {
            text.localScale = Vector3.one;
        }

        foreach (var sequence in _sequences)
        {
            sequence.Kill();
        }

        this.transform.localScale = Vector3.one * 2f;

        _sequences.Clear();

        AnimateText();
    }

    private void AnimateText()
    {
        gameObject.SetActive(true);

        var sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(1, 0.3f).From(0));
        sequence.Join(this.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).From(2f));
        _sequences.Add(sequence);

        for (int i = 0; i < _texts.Count; i++)
        {
            var delay = i * 0.08f;
            var textTransform = _texts[i];

            sequence = DOTween.Sequence();
            sequence.SetDelay(delay);

            sequence.Append(textTransform.DOScale(2.2f, 0.3f).SetEase(Ease.OutSine)); // Lớn dần một cách mượt mà
            sequence.Append(textTransform.DOScale(1f, 0.25f).SetEase(Ease.InOutCubic)); // Quay về từ từ, không quá gấp gáp
            sequence.AppendInterval(1f);
            sequence.Append(textTransform.DOScale(0, 0.15f).SetEase(Ease.OutSine));
            _sequences.Add(sequence);

            if (i == _texts.Count - 1)
            {
                sequence.AppendCallback(() => gameObject.SetActive(false));
            }
        }
    }

    public void Stop()
    {
        foreach (var sequence in _sequences)
        {
            sequence.Kill();
        }

        _sequences.Clear();

        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void FindAllText()
    {
        _texts = new List<Transform>(GetComponentsInChildren<TMP_Text>().Where(x => x.text != "").Select(x => x.transform));

        foreach (var text in _texts)
        {
            text.localScale = Vector3.one;
        }

        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();

        _canvasGroup.alpha = 1;

        EditorUtility.SetDirty(this.gameObject);
    }
#endif
}
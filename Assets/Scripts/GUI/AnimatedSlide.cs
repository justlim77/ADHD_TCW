using UnityEngine;
using System.Collections;

public class AnimatedSlide : MonoBehaviour {

    public Vector2 slideOffset;
    public float slideDuration = 1.0f;
    public iTween.EaseType easeType;

    RectTransform _rectTransform;
    RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    Vector2 m_ToAnchoredPos;
    Vector2 m_FromAnchoredPos;

    void Awake()
    {
        Initialize();
        Reset();
    }

    public bool Initialize()
    {
        m_FromAnchoredPos = rectTransform.anchoredPosition;
        m_ToAnchoredPos = new Vector2(rectTransform.anchoredPosition.x + slideOffset.x, rectTransform.anchoredPosition.y + slideOffset.y);
        return true;
    }

    public void Reset()
    {
        rectTransform.anchoredPosition = m_ToAnchoredPos;
    }

    public void SlideIn()
    {
        StartCoroutine(RunSlide(m_FromAnchoredPos));
    }

    public void SlideOut()
    {
        StartCoroutine(RunSlide(m_ToAnchoredPos));
    }

    IEnumerator RunSlide(Vector2 targetPosition)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", rectTransform.anchoredPosition,
            "to", targetPosition,
            "time", slideDuration,
            "onupdatetarget", gameObject,
            "onupdate", "MoveOnUpdateCallback",
            "easetype", easeType
            )
        );

        yield return null;
    }

    void MoveOnUpdateCallback(Vector2 value)
    {
        rectTransform.anchoredPosition = value;
    }
}

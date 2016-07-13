using UnityEngine;
using System.Collections;

public class BarSlide : MonoBehaviour
{
    public float slideDuration = 1.0f;

    RectTransform m_Rect;
    Vector2 m_StartAnchoredPos;

    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
        m_StartAnchoredPos = m_Rect.anchoredPosition;
    }

    public void SlideInBar()
    {
        StartCoroutine(RunSlideInBar());
    }

    IEnumerator RunSlideInBar()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", m_Rect.anchoredPosition.x,
            "to", m_StartAnchoredPos.x,
            "time", slideDuration,
            "onupdatetarget", gameObject,
            "onupdate", "MoveOnUpdateCallback",
            "easetype", iTween.EaseType.spring
            )
        );
        yield return null;
    }

    void MoveOnUpdateCallback(float value)
    {
        m_Rect.anchoredPosition = new Vector2(value, m_Rect.anchoredPosition.y);
    }

    public void Initialize()
    {
		float barWidth = m_Rect.rect.width;
		float offset = 240 + (barWidth * 0.5f);
		m_Rect.anchoredPosition = new Vector2(offset, m_Rect.anchoredPosition.y);
    }
}

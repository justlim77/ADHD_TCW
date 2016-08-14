using UnityEngine;
using System.Collections;

public class AnimatedSlide : MonoBehaviour {

    public Vector2 slideOffset;
    public float slideDuration = 1.0f;

    RectTransform m_Rect;
    Vector2 m_StartAnchoredPos;

    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
        m_StartAnchoredPos = m_Rect.anchoredPosition;
    }

    public void SlideIn()
    {
        StartCoroutine(RunSlideIn());
    }

    IEnumerator RunSlideIn()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", m_Rect.anchoredPosition,
            "to", m_StartAnchoredPos,
            "time", slideDuration,
            "onupdatetarget", gameObject,
            "onupdate", "MoveOnUpdateCallback",
            "easetype", iTween.EaseType.spring
            )
        );

        yield return null;
    }

    void MoveOnUpdateCallback(Vector2 value)
    {
        m_Rect.anchoredPosition = value;
    }

    public void Initialize()
    {
        Vector2 offset = new Vector2(m_Rect.anchoredPosition.x + slideOffset.x, m_Rect.anchoredPosition.y + slideOffset.y);
        m_Rect.anchoredPosition = offset;
    }
}

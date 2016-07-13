using UnityEngine;
using System.Collections;

public class TextSequence : MonoBehaviour {

    public float fadeDuration = 1.0f;

    CanvasGroup m_CanvasGroup;

    void OnEnable()
    {
        m_CanvasGroup.alpha = 0.0f;
    }

    void Awake()
    {
        m_CanvasGroup = this.GetComponent<CanvasGroup>();
    }

    void Start() { }

    public IEnumerator RunFadeCanvas(float targetAlpha)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", m_CanvasGroup.alpha,
            "to", targetAlpha,
            "time", fadeDuration,
            "onupdatetarget", gameObject,
            "onupdate", "FadeCanvasOnUpdateCallback",
            "easetype", iTween.EaseType.linear
            )
        );
        yield return null;
    }

    void FadeCanvasOnUpdateCallback(float value)
    {
        m_CanvasGroup.alpha = value;
    }
}

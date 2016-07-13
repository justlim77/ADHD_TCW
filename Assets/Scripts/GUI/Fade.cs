using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour {

    public float fadeDuration = 1.0f;

    float m_CurrentAlpha;
    Image m_Image;
    float m_TargetAlpha;

    void Awake() {
        m_Image = this.GetComponent<Image>();
        m_CurrentAlpha = m_Image.color.a;
    }

    void Start() {
    }

    public IEnumerator FadeTo(float direction)
    {
        m_Image.raycastTarget = true;
        m_TargetAlpha = direction;

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", m_CurrentAlpha,
            "to", direction,
            "time", fadeDuration,
            "onupdatetarget", gameObject,
            "onupdate", "AlphaOnUpdateCallback",
            "easetype", iTween.EaseType.linear
            )
        );

        yield return new WaitForSeconds(fadeDuration);
    }

    public void AlphaOnUpdateCallback(float value)
    {
        m_CurrentAlpha = value;
        m_Image.color = new Color(1f, 1f, 1f, m_CurrentAlpha);

        if (m_CurrentAlpha == m_TargetAlpha)
            m_Image.raycastTarget = false;
    }

    public void SetColor(Color color)
    {
        m_Image.color = color;
    }
}

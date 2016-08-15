using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
    float m_CurrentAlpha;
    Image m_Image;
    float m_TargetAlpha;

    void Awake()
    {
        m_Image = this.GetComponent<Image>();
        m_CurrentAlpha = m_Image.color.a;
    }

    public IEnumerator FadeTo(float direction, float fadeDuration)
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
        m_Image.canvasRenderer.SetAlpha(m_CurrentAlpha);

        if (m_CurrentAlpha == m_TargetAlpha)
            m_Image.raycastTarget = false;
    }

    public void SetColor(Color color)
    {
        m_Image.color = color;
    }

    public void CrossFadeAlpha(FadeType fadeType, float fadeDuration)
    {
        switch (fadeType)
        {
            case FadeType.ToWhite:
                //m_Image.CrossFadeAlpha(1.0f, fadeDuration, true);
                iTween.ValueTo(gameObject, iTween.Hash(
                    "from", m_Image.color.a,
                    "to", 1.0f,
                    "time", fadeDuration,
                    "onupdatetarget", gameObject,
                    "onupdate", "AlphaOnUpdateCallback",
                    "easetype", iTween.EaseType.linear
                    )
                );
                break;
        }
    }
}

public enum FadeType
{
    ToWhite,
    ToClear
}

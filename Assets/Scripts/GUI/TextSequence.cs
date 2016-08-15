using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextSequence : MonoBehaviour
{
    public TextTyper dayTextTyper;
    public TextTyper messageTextTyper;

    Image _image;
    Image image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();
            return _image;
        }
    }

    void OnEnable()
    {
        image.canvasRenderer.SetAlpha(0.0f);
    }

    public IEnumerator RunFadeCanvas(float targetAlpha, float fadeDuration)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", image.canvasRenderer.GetAlpha(),
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
        image.canvasRenderer.SetAlpha(value);
    }
}

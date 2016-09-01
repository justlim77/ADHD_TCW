using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class TextSequence : MonoBehaviour
{
    public static event Action<string> OnTapClosed;

    public TextTyper dayTextTyper;
    public TextTyper messageTextTyper;
    public Graphic closeGraphic;

    public bool Initialize()
    {
        dayTextTyper.Clear();
        messageTextTyper.Clear();
        closeGraphic.canvasRenderer.SetAlpha(0.0f);
        return true;
    }

    public void FadeText(float fadeDuration)
    {
        dayTextTyper.FadeText(fadeDuration);
        messageTextTyper.FadeText(fadeDuration);
    }

    public void Skip()
    {
        // Skip headline if not skipped yet
        if (dayTextTyper.skipped == false)
        {
            dayTextTyper.Skip();
            return;
        }

        // Skip info if not skipped yet
        if (messageTextTyper.skipped == false)
        {
            messageTextTyper.Skip();
            return;
        }       
    }

    public void FlashTapToClose(bool value = true)
    {
        if (value)
        {
            closeGraphic.canvasRenderer.SetAlpha(1.0f); // Turn on visibility
            //closeGraphic.canvasRenderer.SetColor(Color.white);  // Set color initially to white
            closeGraphic.DOColor(Color.black, 1f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);  // Pingpong tween between black/white
        }
        else
        {
            TapClosed();
            closeGraphic.DOKill(true);       
        }
    }

    protected virtual void TapClosed()
    {
        if (OnTapClosed != null)
            OnTapClosed("Tapped to skip");
    }
}

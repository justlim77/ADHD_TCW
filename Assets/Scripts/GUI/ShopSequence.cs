using UnityEngine;
using System.Collections;

public class ShopSequence : MonoBehaviour
{
    public Vector2 startPos;

    RectTransform _rectTransform = null;
    RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    void Start()
    {
        rectTransform.anchoredPosition = startPos;
    }

    public void OpenShop()
    {
        if (!GameManager.isGamePause)
        {
            StartCoroutine(RunOpenShop());
            GameManager.isTempPause = true;
            GameManager.isGamePause = true;
        }
    }

    public void CloseShop()
    {
        StartCoroutine(RunCloseShop());
        GameManager.isTempPause = false;
        GameManager.isGamePause = false;
    }

    IEnumerator RunOpenShop()
    {
        gameObject.SetActive(true);
        iTween.MoveTo(gameObject, iTween.Hash("y", Screen.height / 2f, "time", 1));

        yield return null;
    }

    IEnumerator RunCloseShop()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", -Screen.height / 2f, "time", 1));
        yield return null;
    }

    public void LockItems()
    {

    }

    public void UnlockItems()
    {

    }
}

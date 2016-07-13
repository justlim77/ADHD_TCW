using UnityEngine;
using System.Collections;

public class ShopSequence : MonoBehaviour
{
    public Vector2 startPos;

    RectTransform m_Rect;

    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        m_Rect.anchoredPosition = startPos;
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

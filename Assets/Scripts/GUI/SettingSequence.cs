using UnityEngine;
using System.Collections;

public class SettingSequence : MonoBehaviour
{

    public Vector2 startPos;

    RectTransform m_Rect;
    bool lastSetting;

    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        m_Rect.anchoredPosition = startPos;
    }

    public void OpenSettings(bool ignorePause)
    {
        if ((!GameManager.isTempPause) || (ignorePause))
        {
            StartCoroutine(RunOpenSettings());
            lastSetting = GameManager.isTempPause;
            GameManager.isTempPause = true;
            GameManager.isGamePause = true;
        }
    }

    public void PauseGame(bool ignorePause)
    {
        if((GameManager.hasDayStarted) && (!GameManager.isGamePause) && ((!GameManager.isTempPause) || (ignorePause)))
        {
            StartCoroutine(RunOpenSettings());
            lastSetting = GameManager.isTempPause;
            GameManager.isTempPause = true;
            GameManager.isGamePause = true;
        }
    }

    public void CloseSettings(bool bypassDefault)
    {
        StartCoroutine(RunCloseSettings());

        if (bypassDefault)
            GameManager.isTempPause = false;
        else
            GameManager.isTempPause = lastSetting;

        GameManager.isGamePause = false;
    }

    IEnumerator RunOpenSettings()
    {
        gameObject.SetActive(true);
        iTween.MoveTo(gameObject, iTween.Hash("y", Screen.height / 2f, "time", 1));

        yield return null;
    }

    IEnumerator RunCloseSettings()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", -Screen.height / 2f, "time", 1));
        yield return null;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencySequence : MonoBehaviour
{
    public float slideDuration = 1.0f;
    public Text totalGem; 

    RectTransform m_Rect;
    Vector2 m_StartAnchoredPos;
    Vector2 m_OffScreenAnchoredPos;

    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
        m_StartAnchoredPos = m_Rect.anchoredPosition;
    }

    void OnEnable()
    {
        ObtainTotalGem();
        Initialize();
    }

    public void ObtainTotalGem()
    {
        totalGem.text = DataManager.ReadIntData(DataManager.totalGem).ToString();
    }

    #region Private methods
    IEnumerator RunEnterScene()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", m_Rect.anchoredPosition.y,
            "to", m_StartAnchoredPos.y,
            "time", slideDuration,
            "onupdatetarget", gameObject,
            "onupdate", "MoveOnUpdateCallback",
            "easetype", iTween.EaseType.easeOutElastic
            )
        );

        yield return null;
    }

    IEnumerator RunExitScene()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", m_Rect.anchoredPosition.y,
            "to", m_OffScreenAnchoredPos.y,
            "time", slideDuration,
            "onupdatetarget", gameObject,
            "onupdate", "MoveOnUpdateCallback",
            "easetype", iTween.EaseType.easeInOutCirc
            )
        );

        yield return null;
    }

    void MoveOnUpdateCallback(float value)
    {
        m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x, value);
    }
    #endregion

    #region Public methods
    public void Initialize()
    {
        float offset = Screen.height * 0.1f;

        m_OffScreenAnchoredPos = new Vector2(m_Rect.anchoredPosition.x, offset);
        m_Rect.anchoredPosition = m_OffScreenAnchoredPos;
    }

    public void EnterScene()
    {
        StartCoroutine(RunEnterScene());
    }

    public void ExitScene()
    {
        if(!GameManager.isTempPause)
            StartCoroutine(RunExitScene());
    }
    #endregion
}

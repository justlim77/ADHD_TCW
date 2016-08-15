using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanelSequence : MonoBehaviour
{
    public GameObject[] gameplayElements;

    public BoxCollider2D gotoShelf;
    public BoxCollider2D gotoSleep;
    public BoxCollider2D gotoBag;
    public AnimatedSlide timer;
    public AnimatedSlide statsBar;
    public float slideDelay = 0.1f;

    public GameObject currencyPanel;
    public GameObject textPanel;
    public GameObject gameClearPanel;
    public GameObject btnHome;
    public TextTyper dayText;
    public TextTyper messageText;
    public TextTyper[] resultText;
    public Text[] headerText;
    public Text gemGain;
    public SettingSequence gemNotice;
    public SettingSequence startNotice;
    public InputManager inputManager;

    public GameObject btnSettings;
    public GameObject btnPause;

    RectTransform m_TextPanelRect;
    RectTransform m_gameClearRect;
    TextSequence m_TextSequence;
    TextSequence m_gameSequence;
    CurrencySequence m_CurrencySequence;
    WaitForSeconds m_WaitSlideDelay;

    void Awake ()
    {
        m_TextPanelRect = textPanel.GetComponent<RectTransform>();
        m_gameClearRect = gameClearPanel.GetComponent<RectTransform>();
        m_TextSequence = textPanel.GetComponent<TextSequence>();
        m_gameSequence = gameClearPanel.GetComponent<TextSequence>();
        m_CurrencySequence = currencyPanel.GetComponent<CurrencySequence>();
    }

	void Start ()
    {
        ResetForNextScene();
        //gameObject.SetActive(false);
    }

    public void ResetForNextScene()
    {
        gotoSleep.GetComponent<ShakingAnimation>().StopAnimation();
        gotoShelf.GetComponent<ShakingAnimation>().StopAnimation();
        gotoBag.GetComponent<ShakingAnimation>().StopAnimation();

        //foreach (GameObject go in gameplayElements)
        //{
        //    go.SetActive(false);
        //}

        m_WaitSlideDelay = new WaitForSeconds(slideDelay);

        m_TextPanelRect.anchoredPosition = Vector2.zero;
        textPanel.gameObject.SetActive(false);
        gameClearPanel.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        m_CurrencySequence.EnterScene();
    }

    public void IsUserGainGem()
    {
        int gemEarn = 0;

        if (GameManager.CalculateScore(0) != 0)
        {
            gemEarn = GameManager.CalculateScore(0);
            DataManager.StoreIntData(DataManager.totalGem, (DataManager.ReadIntData(DataManager.totalGem) + gemEarn));
            gemGain.text = "You earned " + gemEarn.ToString();
            gemNotice.OpenSettings(false);
        }
        else
            inputManager.Restart();
    }
}

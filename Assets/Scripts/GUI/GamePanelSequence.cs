using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanelSequence : MonoBehaviour
{
    public GameObject[] gameplayElements;

    public BoxCollider2D gotoShelf;
    public BoxCollider2D gotoSleep;
    public BoxCollider2D gotoBag;
    public TimerSlide timerIcon;
    public BarSlide negativityBar;
    public BarSlide tirednessBar;
    public BarSlide messinessBar;
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
        gameObject.SetActive(false);
    }

    public void ResetForNextScene()
    {
        gotoSleep.GetComponent<ShakingAnimation>().StopAnimation();
        gotoShelf.GetComponent<ShakingAnimation>().StopAnimation();
        gotoBag.GetComponent<ShakingAnimation>().StopAnimation();

        foreach (GameObject go in gameplayElements)
        {
            go.SetActive(false);
        }

        m_WaitSlideDelay = new WaitForSeconds(slideDelay);

        m_TextPanelRect.anchoredPosition = Vector2.zero;
        textPanel.gameObject.SetActive(false);
        gameClearPanel.gameObject.SetActive(false);

        timerIcon.Initialize();
        negativityBar.Initialize();
        messinessBar.Initialize();
        tirednessBar.Initialize();
    }

    public void Initialize()
    {
        m_CurrencySequence.EnterScene();
    }

    public void StartSequence()
    {
        if ((DataManager.ReadIntData("LIFE") > 0) || (GameManager.dayScene > 1))
        {
            if ((GameManager.dayScene <= 3) && (!GameManager.isTempPause))
                StartCoroutine(RunStartSequence());
            else
                StartCoroutine(RunEndSequence());
        }
        else
        {
            startNotice.OpenSettings(false);
        }
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

    IEnumerator RunEndSequence()
    {
        //Initialize
        for (int i = 0; i < headerText.Length; i++)
            headerText[i].GetComponent<BarSlide>().Initialize();

        //Fade in White Canvas
        gameClearPanel.gameObject.SetActive(true);

        yield return m_gameSequence.RunFadeCanvas(1.0f);
        yield return new WaitForSeconds(m_gameSequence.fadeDuration);

        //Calculate Results Overall
        headerText[3].text = "Overall Results:";

        headerText[3].GetComponent<BarSlide>().SlideInBar();
        yield return new WaitForSeconds(0.7f);

        if (GameManager.CalculateScore(0) <= 14) //Bad
        {
            //headerText[3].color = new Color32(255, 0, 0, 255);
            resultText[3].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[3].RunTypeText("Uh oh! You are about to enter a boxing match! You are making a mountain out of a molehill. The grass may be greener on the other side but they may be weeds!");
        }
        else if ((GameManager.CalculateScore(0) > 14) && (GameManager.CalculateScore(0) <= 19)) //Mid
        {
            //headerText[3].color = new Color32(255, 100, 0, 255);
            resultText[3].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[3].RunTypeText("Hmmm... You're almost there! Go on, smell the roses! Being mindful is half the battle won.");
        }
        else if (GameManager.CalculateScore(0) > 19) //Very Good
        {
            //headerText[3].color = new Color32(0, 155, 0, 255);
            resultText[3].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[3].RunTypeText("Hooray! You hit jackpot! You are on your way to being a cool cucumber! Be proud of yourself!");
        }

        headerText[0].GetComponent<BarSlide>().SlideInBar();
        yield return new WaitForSeconds(0.7f);

        //Calculate Results E1
        if (GameManager.CalculateScore(1) <= 2) //Bad
        {
            //headerText[0].color = new Color32(255, 0, 0, 255);
            resultText[0].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[0].RunTypeText("You need to work on these:\n\n-> Managing your own emotions\n-> Acknolwedging your child's every effort");
        }
        else if ((GameManager.CalculateScore(1) > 2) && (GameManager.CalculateScore(1) <= 4)) //Mid
        {
            //headerText[0].color = new Color32(255, 100, 0, 255);
            resultText[0].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[0].RunTypeText("You can improve on these:\n\n-> Using appropriate words\n-> Listening to your child's needs\n-> Giving praise and encouragement");
        }
        else if (GameManager.CalculateScore(1) > 4) //Very Good
        {
            //headerText[0].color = new Color32(0, 155, 0, 255);
            resultText[0].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[0].RunTypeText("You nailed these:\n\n-> Being firm and not harsh\n-> Acknowledging your child's needs");
        }

        headerText[1].GetComponent<BarSlide>().SlideInBar();
        yield return new WaitForSeconds(0.7f);

        //Calculate Results E2
        if (GameManager.CalculateScore(2) <= 5) //Bad
        {
            //headerText[1].color = new Color32(255, 0, 0, 255);
            resultText[1].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[1].RunTypeText("You need to work on these:\n\n-> Getting your child's attention first\n-> Stating tasks requirements\n-> Giving instructions one at a time");
        }
        else if ((GameManager.CalculateScore(2) > 5) && (GameManager.CalculateScore(2) <= 7)) //Mid
        {
            //headerText[1].color = new Color32(255, 100, 0, 255);
            resultText[1].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[1].RunTypeText("You can improve on these:\n\n-> Getting your child's attention first\n-> Stating tasks requirements\n-> Keeping instructions short");
        }
        else if (GameManager.CalculateScore(2) > 7) //Very Good
        {
            //headerText[1].color = new Color32(0, 155, 0, 255);
            resultText[1].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[1].RunTypeText("You nailed these:\n\n-> Getting your child's attention first\n-> Stating tasks requirements\n-> Checking for understanding");
        }

        headerText[2].GetComponent<BarSlide>().SlideInBar();
        yield return new WaitForSeconds(0.7f);

        //Calculate Results E3
        if (GameManager.CalculateScore(3) <= 5) //Bad
        {
            //headerText[2].color = new Color32(255, 0, 0, 255);
            resultText[2].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[2].RunTypeText("You need to work on these:\n\n-> Ignoring minor misbehavior\n-> Following through with consequences");
        }
        else if ((GameManager.CalculateScore(3) > 5) && (GameManager.CalculateScore(3) <= 7)) //Mid
        {
            //headerText[2].color = new Color32(255, 100, 0, 255);
            resultText[2].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[2].RunTypeText("You can improve on these:\n\n-> Stating expectations and consequences clearly\n-> Following through with consequences");
        }
        else if (GameManager.CalculateScore(3) > 7) //Very Good
        {
            //headerText[2].color = new Color32(0, 155, 0, 255);
            resultText[2].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[2].RunTypeText("You nailed these:\n\n-> Being flexible with child's request\n-> Following through with consequences");
        }

        yield return new WaitForSeconds(0.1f);
        btnHome.SetActive(true);
    }

    IEnumerator RunStartSequence()
    {
        // Show day number and time arrived at home
        textPanel.gameObject.SetActive(true);

        yield return m_TextSequence.RunFadeCanvas(1.0f);
        yield return new WaitForSeconds(m_TextSequence.fadeDuration);

        // Set gameplay elements active
        foreach (GameObject go in gameplayElements)
        {
            go.SetActive(true);
        }

        switch (GameManager.dayScene)
        {
            case 1:
                GameManager.gameHour = 5;
                yield return dayText.RunTypeText("Day 1");
                yield return messageText.RunTypeText("You arrived home from work at 5:00 P.M.");
                break;
            case 2:
                GameManager.gameHour = 6;
                yield return dayText.RunTypeText("Day 2");
                yield return messageText.RunTypeText("You arrived home from work at 6:00 P.M. due to bad traffic.");
                break;
            case 3:
                GameManager.gameHour = 7;
                yield return dayText.RunTypeText("Day 3");
                yield return messageText.RunTypeText("You arrived home from work at 7:00 P.M. as you had to overtime at work.");
                break;
        }
        
        yield return new WaitForSeconds(2.0f);

        // Fade out text panel canvas group alpha
        yield return m_TextSequence.RunFadeCanvas(0.0f);
        yield return new WaitForSeconds(m_TextSequence.fadeDuration);

        textPanel.gameObject.SetActive(false);
        dayText.Clear();
        messageText.Clear();

        yield return m_WaitSlideDelay;
        timerIcon.SlideIn();
        yield return m_WaitSlideDelay;
        negativityBar.SlideInBar();
        yield return m_WaitSlideDelay;
        messinessBar.SlideInBar();
        yield return m_WaitSlideDelay;
        tirednessBar.SlideInBar();
        yield return m_WaitSlideDelay;

        gotoSleep.enabled = true;
        gotoShelf.enabled = true;
        gotoBag.enabled = true;
        gotoSleep.GetComponent<ShakingAnimation>().StartAnimation();
        gotoShelf.GetComponent<ShakingAnimation>().StartAnimation();
        gotoBag.GetComponent<ShakingAnimation>().StartAnimation();

        GameManager.hasDayStarted = true;
        GameManager.CheckifGameOver();

        btnSettings.SetActive(true);
        btnPause.SetActive(true);
    }
}

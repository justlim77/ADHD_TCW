using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public enum Mode
{
    Default,
    Game,
    Edit,
}

public class UserInterface : MonoBehaviour
{
    public static Action<Mode> OnModeChanged;

    [Header("UI Elements")]
    public AnimatedButton startButton;
    public AnimatedButton editButton;
    public AnimatedButton settingsButton;
    public GameObject topBar;
    public GameObject bottomBar;

    public GameObject notificationPrefab;
    public GameObject arrivalPrefab;

    public GameObject[] gameplayElements;

    public BoxCollider2D gotoShelf;
    public BoxCollider2D gotoSleep;
    public BoxCollider2D gotoBag;

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

    PopupOpener _popupOpener;
    PopupOpener popupOpener
    {
        get
        {
            if (_popupOpener == null)
            {
                _popupOpener = GetComponent<PopupOpener>();
                if (_popupOpener == null)
                {
                    _popupOpener = gameObject.AddComponent<PopupOpener>();
                    _popupOpener.popupPrefab = notificationPrefab;
                }
            }
            return _popupOpener;
        }
    }

    Mode m_mode = Mode.Default;

    RectTransform m_TextPanelRect;
    RectTransform m_gameClearRect;
    TextSequence m_TextSequence;
    TextSequence m_gameSequence;

    void OnEnable()
    {
        editButton.onClick.AddListener(() => ToggleMode());
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        ShelfCleaning.OnCleaningGameOpen += ShelfCleaning_OnCleaningGameOpen;
    }

    private void ShelfCleaning_OnCleaningGameOpen(string obj)
    {
        Debug.Log(obj);
        topBar.GetComponent<AnimatedSlide>().SlideOut();
    }

    void OnDisable()
    {
        editButton.onClick.RemoveAllListeners();
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameState e)
    {
        switch (e)
        {
            case GameState.Default:
                settingsButton.GetComponent<AnimatedSlide>().SlideOut();
                startButton.GetComponent<AnimatedSlide>().SlideOut();
                editButton.GetComponent<AnimatedSlide>().SlideOut();
                topBar.GetComponent<AnimatedSlide>().SlideOut();
                bottomBar.GetComponent<AnimatedSlide>().SlideOut();
                break;
            case GameState.Pregame:
                startButton.GetComponent<AnimatedSlide>().SlideIn();
                editButton.GetComponent<AnimatedSlide>().SlideIn();
                settingsButton.GetComponent<AnimatedSlide>().SlideIn();
                topBar.GetComponent<CurrencyBar>().UpdateGems(DataManager.ReadIntData(DataManager.totalGem));
                topBar.GetComponent<AnimatedSlide>().SlideIn();
                break;
            case GameState.Playing:
                StartSequence();
                break;
        }
    }

    protected void ModeChanged()
    {
        Debug.Log("Mode changed to " + m_mode.ToString());

        if (OnModeChanged != null)
            OnModeChanged(m_mode);

        switch (m_mode)
        {
            case Mode.Edit:
                settingsButton.GetComponent<AnimatedSlide>().SlideIn();
                editButton.GetComponent<AnimatedSlide>().SlideIn();
                topBar.GetComponent<AnimatedSlide>().SlideIn();
                bottomBar.GetComponent<AnimatedSlide>().SlideOut();
                startButton.GetComponent<AnimatedSlide>().SlideOut();
                break;
            case Mode.Game:
                topBar.GetComponent<AnimatedSlide>().SlideIn();
                startButton.GetComponent<AnimatedSlide>().SlideIn();
                settingsButton.GetComponent<AnimatedSlide>().SlideIn();
                editButton.GetComponent<AnimatedSlide>().SlideIn();
                bottomBar.GetComponent<AnimatedSlide>().SlideOut();
                break;
        }
    }

    public void StartGame()
    {
        //GameManager.Instance.PlayGame();
        GameTimer.Instance.DeductLife();
    }

    void ShowNoficationPopup(string headline, string info, params Action[] actions)
    {
        popupOpener.popupPrefab = notificationPrefab;
        popupOpener.initialScale = Vector3.zero;
        GameObject notification = popupOpener.GetOpenPopup();
        Notify notify = notification.GetComponent<Notify>();
        notify.headlineText.text = headline;
        notify.infoText.text = info;
        for (int i = 0; i < actions.Length; i++)
        {
            int n = i;
            notify.confirmButton.onClick.AddListener(() => { actions[n](); });
        }

        notify.confirmButton.onClick.AddListener(() => { notification.GetComponent<Popup>().Close(); });
    }

    IEnumerator ShowArrivalPopup(string headline, string info, params Action[] actions)
    {
        popupOpener.popupPrefab = arrivalPrefab;
        popupOpener.initialScale = Vector3.one;
        GameObject arrival = popupOpener.GetOpenPopup();

        TextSequence sequence = arrival.GetComponent<TextSequence>();
        sequence.Initialize();
        var button = arrival.GetComponent<Popup>().GetBackground().AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        button.onClick.AddListener(() => sequence.Skip());

        yield return StartCoroutine(sequence.dayTextTyper.RunTypeText(headline));
        yield return StartCoroutine(sequence.messageTextTyper.RunTypeText(info));

        sequence.FlashTapToClose();

        button.onClick.AddListener(() => arrival.GetComponent<Popup>().Close());
        button.onClick.AddListener(() => sequence.FadeText(0.25f));
        button.onClick.AddListener(() => sequence.FlashTapToClose(false));
    }

    public void StartSequence()
    {
        startButton.GetComponent<AnimatedSlide>().SlideOut();
        editButton.GetComponent<AnimatedSlide>().SlideOut();
        bottomBar.GetComponent<AnimatedSlide>().SlideIn();

        if ((DataManager.ReadIntData("LIFE") > 0) || (GameManager.dayScene > 1))
        {
            if ((GameManager.dayScene <= 3) && (!GameManager.isTempPause))
                StartCoroutine(ArrivalSequence());
            else
                StartCoroutine(EndSequence());
        }
        else
        {
            ShowNoficationPopup(
                "Uh oh!",
                "Looks like you're out of Energy.",
                startButton.GetComponent<AnimatedSlide>().SlideIn,
                bottomBar.GetComponent<AnimatedSlide>().SlideOut
                );
        }
    }

    IEnumerator ArrivalSequence()
    {
        // Show day number and time arrived at home

        // Set gameplay elements active
        foreach (GameObject go in gameplayElements)
        {
            go.SetActive(true);
        }

        string headline = "";
        string info = "";

        switch (GameManager.dayScene)
        {
            case 1:
                GameManager.gameHour = 5;
                headline = "Day 1";
                info = "You arrived home from work at 5:00 P.M.";
                break;
            case 2:
                GameManager.gameHour = 6;
                headline = "Day 2";
                info = "You arrived home from work at 6:00 P.M. due to bad traffic.";
                break;
            case 3:
                GameManager.gameHour = 7;
                headline = "Day 3";
                info = "You arrived home from work at 7:00 P.M. as you had to overtime at work.";
                break;
        }

        yield return StartCoroutine(ShowArrivalPopup(headline, info));

        gotoSleep.enabled = true;
        gotoShelf.enabled = true;
        gotoBag.enabled = true;

        gotoSleep.GetComponent<AnimatedShake>().StartAnimation();
        gotoShelf.GetComponent<AnimatedShake>().StartAnimation();
        gotoBag.GetComponent<AnimatedShake>().StartAnimation();

        GameManager.hasDayStarted = true;
        GameManager.CheckifGameOver();
    }

    IEnumerator EndSequence()
    {
        //Initialize
        //for (int i = 0; i < headerText.Length; i++)
        //    headerText[i].GetComponent<AnimatedSlide>().Initialize();

        //Fade in White Canvas
        gameClearPanel.gameObject.SetActive(true);

        //yield return m_gameSequence.RunFadeCanvas(1.0f, 1.0f);
        //yield return new WaitForSeconds(m_gameSequence.fadeDuration);

        //Calculate Results Overall
        headerText[3].text = "Overall Results:";

        headerText[3].GetComponent<AnimatedSlide>().SlideIn();
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

        headerText[0].GetComponent<AnimatedSlide>().SlideIn();
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

        headerText[1].GetComponent<AnimatedSlide>().SlideIn();
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

        headerText[2].GetComponent<AnimatedSlide>().SlideIn();
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

    public void SetMode(Mode mode)
    {
        if (mode != m_mode)
        {
            m_mode = mode;
            ModeChanged();
        }
    }

    public Mode GetMode()
    {
        return m_mode;
    }

    public void ToggleMode()
    {
        ImageToggle toggle = editButton.GetComponent<ImageToggle>();

        toggle.Toggle();

        if (toggle.isDefault)
        {
            SetMode(Mode.Game);
        }
        else
        {
            SetMode(Mode.Edit);
        }
    }
}

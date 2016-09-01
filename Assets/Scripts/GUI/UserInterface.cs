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
    public GameObject yesNoPrefab;
    public GameObject arrivalPrefab;
    public GameObject chatPanelPrefab;
    public GameObject resultPrefab;

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

        TopBar.OnShopButtonClicked += TopBar_OnShopButtonClicked;
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        ShelfCleaning.OnCleaningGameOpen += ShelfCleaning_OnCleaningGameOpen;
        ShelfCleaning.OnCleaningGameCompleted += ShelfCleaning_OnCleaningGameCompleted;
        SleepButton.OnBedPressed += SleepButton_OnBedPressed;
    }

    private void TopBar_OnShopButtonClicked(string obj)
    {
        ShopManager.OnItemPurchased += ShopManager_OnItemPurchased;
        ShopManager.OnShopClosed += ShopManager_OnShopClosed;

        EnableColliders(false);
    }

    private void ShopManager_OnShopClosed(string obj)
    {
        ShopManager.OnItemPurchased -= ShopManager_OnItemPurchased;

        if (GameManager.hasDayStarted)
            EnableColliders(true);
    }

    private void ShopManager_OnItemPurchased(string obj)
    {
        ShowNoficationPopup(obj, "", false);
    }

    private void SleepButton_OnBedPressed(string obj)
    {
        ShowYesNoPopup("Go to Sleep?",
                        "",
                        false,
                        StartGame);
    }

    private void ShelfCleaning_OnCleaningGameOpen(string obj)
    {
        //topBar.GetComponent<AnimatedSlide>().SlideOut();
        EnableColliders(false);
        Debug.Log(obj);
    }

    private void ShelfCleaning_OnCleaningGameCompleted(string obj)
    {
        //topBar.GetComponent<AnimatedSlide>().SlideIn();
        ShowNoficationPopup(obj, "");
    }

    void OnDisable()
    {
        editButton.onClick.RemoveAllListeners();

        TopBar.OnShopButtonClicked -= TopBar_OnShopButtonClicked;
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        ShelfCleaning.OnCleaningGameOpen -= ShelfCleaning_OnCleaningGameOpen;
        ShelfCleaning.OnCleaningGameCompleted -= ShelfCleaning_OnCleaningGameCompleted;
        SleepButton.OnBedPressed -= SleepButton_OnBedPressed;
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
        GameManager.Instance.IncreaseDayScene();
        GameTimer.Instance.DeductLife();
        //GameManager.Instance.PlayGame();
        StartSequence();
    }

    void ShowNoficationPopup(string headline, string info, bool enableCollidersOnConfirm = true, params Action[] actions)
    {
        EnableColliders(false); // Disable box collider button raycast

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

        if(enableCollidersOnConfirm)
            notify.confirmButton.onClick.AddListener(() => { EnableColliders(true); });
    }

    void ShowYesNoPopup(string headline, string info, bool enableCollidersOnConfirm = true, params Action[] actions)
    {
        EnableColliders(false); // Disable box collider button raycast

        popupOpener.popupPrefab = yesNoPrefab;
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
        notify.cancelButton.onClick.AddListener(() => { notification.GetComponent<Popup>().Close(); });
        notify.cancelButton.onClick.AddListener(() => { EnableColliders(true); });
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

        TextSequence.OnTapClosed += TextSequence_OnTapClosed;

        button.onClick.AddListener(() => arrival.GetComponent<Popup>().Close());
        button.onClick.AddListener(() => sequence.FadeText(0.25f));
        button.onClick.AddListener(() => sequence.FlashTapToClose(false));
    }

    private void TextSequence_OnTapClosed(string obj)
    {
        EnableColliders(true);
        TextSequence.OnTapClosed -= TextSequence_OnTapClosed;
    }

    IEnumerator ShowResultPopup()
    {
        popupOpener.popupPrefab = resultPrefab;
        popupOpener.initialScale = Vector3.one;
        GameObject result = popupOpener.GetOpenPopup();

        ResultsPopup resultPopup = result.GetComponent<ResultsPopup>();

        yield return StartCoroutine(resultPopup.ShowResults());
    }

    public void StartSequence()
    {
        startButton.GetComponent<AnimatedSlide>().SlideOut();
        editButton.GetComponent<AnimatedSlide>().SlideOut();
        topBar.GetComponent<AnimatedSlide>().SlideIn();
        bottomBar.GetComponent<AnimatedSlide>().SlideIn();

        EnableColliders(false);

        if ((DataManager.ReadIntData("LIFE") > 0) || (GameManager.Instance.dayScene > 1))
        {
            if ((GameManager.Instance.dayScene <= 3) && (!GameManager.isTempPause))
                StartCoroutine(ArrivalSequence());
            else
                StartCoroutine(EndSequence());
        }
        else
        {
            ShowNoficationPopup(
                "Uh oh!",
                "Looks like you're out of Energy.",
                false,
                startButton.GetComponent<AnimatedSlide>().SlideIn,
                bottomBar.GetComponent<AnimatedSlide>().SlideOut,
                topBar.GetComponent<AnimatedSlide>().SlideIn                
                );
        }
    }

    IEnumerator ArrivalSequence()
    {
        EnableColliders(false);

        // Set gameplay elements active
        foreach (GameObject go in gameplayElements)
        {
            go.SetActive(true);
        }

        string headline = "";
        string info = "";

        // Show day number and time arrived at home
        switch (GameManager.Instance.dayScene)
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

        //EnableColliders(true);

        gotoSleep.GetComponent<AnimatedShake>().StartAnimation();
        gotoShelf.GetComponent<AnimatedShake>().StartAnimation();
        gotoBag.GetComponent<AnimatedShake>().StartAnimation();

        GameManager.hasDayStarted = true;
        GameManager.Instance.CheckifGameOver();
    }

    public void EnableColliders(bool value)
    {
        if (!GameManager.isBagDone)
            gotoBag.enabled = value;
        if (!GameManager.isCleanShelfDone)
            gotoShelf.enabled = value;

        gotoSleep.enabled = value;
    }

    public void DisableColliders()
    {
        gotoBag.enabled = false;
        gotoShelf.enabled = false;
        gotoSleep.enabled = false;
    }

    IEnumerator EndSequence()
    {
        //Initialize
        //for (int i = 0; i < headerText.Length; i++)
        //    headerText[i].GetComponent<AnimatedSlide>().Initialize();

        //Fade in White Canvas

        //yield return m_gameSequence.RunFadeCanvas(1.0f, 1.0f);
        //yield return new WaitForSeconds(m_gameSequence.fadeDuration);
        yield return StartCoroutine(ShowResultPopup());
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

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserInterface : MonoBehaviour
{
    [Header("UI Elements")]
    public AnimatedButton startButton;
    public GameObject topBar;
    public GameObject bottomBar;

    void OnEnable()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.PlayGame());
        startButton.onClick.AddListener(() => GameTimer.Instance.DeductLife());

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveAllListeners();

        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameState e)
    {
        switch (e)
        {
            case GameState.Default:
                startButton.GetComponent<AnimatedSlide>().SlideOut();
                topBar.GetComponent<AnimatedSlide>().SlideOut();
                bottomBar.GetComponent<AnimatedSlide>().SlideOut();
                break;
            case GameState.Pregame:
                startButton.GetComponent<AnimatedSlide>().SlideIn();
                topBar.GetComponent<CurrencyBar>().UpdateGems(DataManager.ReadIntData(DataManager.totalGem));
                topBar.GetComponent<AnimatedSlide>().SlideIn();
                break;
            case GameState.Playing:
                bottomBar.GetComponent<AnimatedSlide>().SlideIn();
                break;
        }
    }
}

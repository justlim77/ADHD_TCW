using UnityEngine;
using System.Collections;
using System;

public class ShelfCleaning : MonoBehaviour
{
    public static event Action<string> OnCleaningGameOpen;
    public static event Action<string> OnCleaningGameCompleted;

    public SpriteRenderer bgShelf, goShelf;
    public GameObject cleaningGamePrefab;

    GameObject m_cleaningGame;

    BoxCollider2D m_boxCollider;
    BoxCollider2D boxCollider
    {
        get
        {
            if (m_boxCollider == null)
            {
                m_boxCollider = GetComponent<BoxCollider2D>();
                
            }
            return m_boxCollider;
        }
    }
        
    public float slideDelay = 0.1f;

    WaitForSeconds m_WaitSlideDelay;

    void Start()
    {
        m_WaitSlideDelay = new WaitForSeconds(slideDelay);
    }

    protected void CleaningGameOpened()
    {
        if (OnCleaningGameOpen != null)
        {
            OnCleaningGameOpen("Shelf Cleaning game opened");
        }
    }
    protected void CleaningGameCompleted()
    {
        Destroy(m_cleaningGame);

        if (OnCleaningGameCompleted != null)
        {
            OnCleaningGameCompleted("Dust Cleared");
        }
    }


    void OnEnable()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameState e)
    {
        switch (e)
        {
            case GameState.Playing:
                Initialize();
                break;
        }
    }

    public void Initialize()
    {
        //if (!GameManager.isTempPause)
        //{
        //    StartCoroutine(ShowShelf());
        //    GameManager.isTempPause = true;
        //}

        boxCollider.enabled = true;

    }

    public void DeInitialize()
    {
        StartCoroutine(HideSelf());
    }

    IEnumerator HideSelf()
    {
        yield return m_WaitSlideDelay;

        bgShelf.gameObject.SetActive(false);
        goShelf.gameObject.SetActive(false);
    }

    public void SetBoxCollider(bool isActive)
    {
        boxCollider.enabled = isActive;
    }

    public void RestoreBoxCollider()
    {
        if(!GameManager.isCleanShelfDone)
            boxCollider.enabled = true;
    }

    public void OpenGame()
    {
        boxCollider.enabled = false;
        m_cleaningGame = Instantiate(cleaningGamePrefab);
        CleaningGame.OnDustCleared += CleaningGame_OnDustCleared;
        CleaningGameOpened();
    }

    private void CleaningGame_OnDustCleared(string obj)
    {
        CleaningGameCompleted();
        CleaningGame.OnDustCleared -= CleaningGame_OnDustCleared;
    }
}

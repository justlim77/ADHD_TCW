using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
    public RectTransform sh;
    public RectTransform lh;
    float min, hour, tempHour;
    int count = 0;

    //Force Go Sleep
    public BoxCollider2D bcShelf;
    public SettingSequence sleepPanel;
    public GamePanelSequence gamePanel;
    public GameManager gameManager;
    public ShelfCleaning shelfClean;

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
        //switch (e)
        //{
        //    case GameState.Playing:
                
        //}
    }

    public void ResetClockCount()
    {
        count = 0;
        tempHour = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameManager.hasDayStarted) && (!GameManager.isGamePause))
        {
            GameManager.gameTime += Time.deltaTime;

            min = GameManager.gameTime;
            hour = GameManager.gameTime / 60;

            if (tempHour > 0)
            {
                tempHour = 0;
                count++;
                GameManager.lvlStamina--;
                GameManager.UpdateStaminaBar();
            }
            else
                tempHour = (GameManager.gameHour + hour - 10) - count;

            if (GameManager.gameHour + hour >= 16)
            {
                GameManager.hasDayStarted = false;
                sleepPanel.CloseSettings(false);
                shelfClean.DeInitialize();
                gamePanel.ResetForNextScene();
                gameManager.IncreaseDayScene();
                //gamePanel.StartSequence();
                gameManager.SetInteractableWithoutScript(true);
                ResetClockCount();
            }

            lh.localEulerAngles = new Vector3(0, 0, -(min * 6));
            sh.localEulerAngles = new Vector3(0, 0, -((GameManager.gameHour + hour) * 30));
        }
    }
}

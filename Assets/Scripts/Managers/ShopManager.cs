using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//[SHOP ITEM ID]
//0 - REFIL GEM
//1 - REFIL POSITIVITY
//2 - REFIL CLEANLINESS
//3 - REFIL STAMINA


public class ShopManager : MonoBehaviour
{
    //Shop Price (Set from Unity)
    public int refilGem = 100;
    public int refilPositive = 100;
    public int refilCleanliness = 100;
    public int refilStamina = 100;

    public Text[] shopPrice;
    public Text resultHeader;
    public Text resultMsg;
    public SettingSequence purchaseResult;
    public CurrencySequence currencyPanel;
    public GameTimer lifeTimer;

    public ScrollRect shopScroll;
    public Button shopClose;
    public Button[] buyButton;

    void Start()
    {
        shopPrice[0].text = refilGem.ToString();
        shopPrice[1].text = refilPositive.ToString();
        shopPrice[2].text = refilCleanliness.ToString();
        shopPrice[3].text = refilStamina.ToString();
    }

    void DisplayResultMessage(bool Success, string Message)
    {
        ButtonAndScroll(false);

        if (!Success)
            resultHeader.text = "Purchase Failed!";
        else
            resultHeader.text = "Purchase Success!";

        resultMsg.text = Message;
        purchaseResult.OpenSettings(true);
    }

    public void ButtonAndScroll(bool isEnable)
    {
        for (int i = 0; i < buyButton.Length; i++)
            buyButton[i].enabled = isEnable;

        shopClose.enabled = isEnable;
        shopScroll.enabled = isEnable;
    }

    void DeductGem(int Amount)
    {
        DataManager.StoreIntData(DataManager.totalGem, (DataManager.ReadIntData(DataManager.totalGem) - Amount));
        currencyPanel.ObtainTotalGem();

        if (DataManager.ReadIntData(DataManager.acThree) == 0)
            DataManager.StoreIntData(DataManager.acThree, 1);
    }

    public void AttemptPurchase(int itemID)
    {
        switch(itemID)
        {
            case 0: //Refil GEM
                if(DataManager.ReadIntData("LIFE") < 3)
                {
                    if(DataManager.ReadIntData(DataManager.totalGem) >= refilGem) //Check if GEM is sufficient
                    {
                        DeductGem(refilGem);
                        lifeTimer.ResetLife();
                        DisplayResultMessage(true, "Life has been refilled!");
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient GEM!");
                    }
                }
                else
                {
                    //Display Error (Game hasn't started)
                    DisplayResultMessage(false, "You have full lifes!");
                }
                break;
            case 1: //Refil Posivity
                if (GameManager.hasDayStarted) //Check if Game have started
                {
                    if(DataManager.ReadIntData(DataManager.totalGem) >= refilPositive) //Check if GEM is sufficient
                    {
                        if(GameManager.lvlPositivity < GameManager.maxBar)
                        {
                            DeductGem(refilPositive);
                            GameManager.lvlPositivity = GameManager.maxBar;
                            GameManager.UpdateNegativeBar();
                            DisplayResultMessage(true, "Posivity Bar has been refilled!");
                        }
                        else
                        {
                            //Display Error (Bar is Full)
                            DisplayResultMessage(false, "Posivity Bar is Full!");
                        }
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient GEM!");
                    }
                }
                else
                {
                    //Display Error (Game hasn't started)
                    DisplayResultMessage(false, "Game hasn't started!");
                }
                break;
            case 2: //Refil Cleanliness
                if (GameManager.hasDayStarted) //Check if Game have started
                {
                    if (DataManager.ReadIntData(DataManager.totalGem) >= refilCleanliness) //Check if GEM is sufficient
                    {
                        if (GameManager.lvlCleanliness < GameManager.maxBar)
                        {
                            DeductGem(refilCleanliness);
                            GameManager.lvlCleanliness = GameManager.maxBar;
                            GameManager.UpdateMessinessBar();
                            DisplayResultMessage(true, "Cleanliness Bar has been refilled!");
                        }
                        else
                        {
                            //Display Error (Bar is Full)
                            DisplayResultMessage(false, "Cleanliness Bar is Full!");
                        }
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient GEM!");
                    }
                }
                else
                {
                    //Display Error (Game hasn't started)
                    DisplayResultMessage(false, "Game hasn't started!");
                }
                break;
            case 3: //Refil Stamina
                if (GameManager.hasDayStarted) //Check if Game have started
                {
                    if (DataManager.ReadIntData(DataManager.totalGem) >= refilStamina) //Check if GEM is sufficient
                    {
                        if (GameManager.lvlStamina < GameManager.maxStamina)
                        {
                            DeductGem(refilStamina);
                            GameManager.lvlStamina = GameManager.maxStamina;
                            GameManager.UpdateStaminaBar();
                            DisplayResultMessage(true, "Stamina has been refilled!");
                        }
                        else
                        {
                            //Display Error (Bar is Full)
                            DisplayResultMessage(false, "Stamina is Full!");
                        }
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient GEM!");
                    }
                }
                else
                {
                    //Display Error (Game hasn't started)
                    DisplayResultMessage(false, "Game hasn't started!");
                }
                break;
        }
    }
}

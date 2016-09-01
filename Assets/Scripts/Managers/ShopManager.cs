using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

//[SHOP ITEM ID]
//0 - REFIL ENERGY
//1 - REFIL MOOD
//2 - REFIL HYGIENE
//3 - REFIL VITALITY

public class ShopManager : MonoBehaviour
{
    public static event Action<string> OnItemPurchased;

    //Shop Price (Set from Unity)
    public int refillEnergyCost = 100;
    public int refillMoodCost = 100;
    public int refillHygieneCost = 100;
    public int refillVitalityCost = 100;

    public Text[] priceLabels;
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
        priceLabels[0].text = refillEnergyCost.ToString();
        priceLabels[1].text = refillMoodCost.ToString();
        priceLabels[2].text = refillHygieneCost.ToString();
        priceLabels[3].text = refillVitalityCost.ToString();
    }

    protected virtual void ItemPurchased(string result)
    {
        if (OnItemPurchased != null)
            OnItemPurchased(result);
    }

    void DisplayResultMessage(bool Success, string Message)
    {
        string result = "";

        result = Success ? "Purchase Success!" : "Purchase Failed";

        ItemPurchased(result);
    }

    void DeductGem(int Amount)
    {
        DataManager.StoreIntData(DataManager.totalGem, (DataManager.ReadIntData(DataManager.totalGem) - Amount));

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
                    if(DataManager.ReadIntData(DataManager.totalGem) >= refillEnergyCost) //Check if GEM is sufficient
                    {
                        DeductGem(refillEnergyCost);
                        lifeTimer.ResetLife();
                        DisplayResultMessage(true, "Energy refilled!");
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient Gems!");
                    }
                }
                else
                {
                    //Display Error (Game hasn't started)
                    DisplayResultMessage(false, "Energy full!");
                }
                break;
            case 1: //Refil Posivity
                if (GameManager.hasDayStarted) //Check if Game have started
                {
                    if(DataManager.ReadIntData(DataManager.totalGem) >= refillMoodCost) //Check if GEM is sufficient
                    {
                        if(GameManager.Instance.mood < GameManager.maxBar)
                        {
                            DeductGem(refillMoodCost);
                            GameManager.Instance.mood = GameManager.maxBar;
                            DisplayResultMessage(true, "Mood refilled!");
                        }
                        else
                        {
                            //Display Error (Bar is Full)
                            DisplayResultMessage(false, "Mood full!");
                        }
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient Gems!");
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
                    if (DataManager.ReadIntData(DataManager.totalGem) >= refillHygieneCost) //Check if GEM is sufficient
                    {
                        if (GameManager.Instance.hygiene < GameManager.maxBar)
                        {
                            DeductGem(refillHygieneCost);
                            GameManager.Instance.hygiene = GameManager.maxBar;
                            DisplayResultMessage(true, "Hygiene refilled!");
                        }
                        else
                        {
                            //Display Error (Bar is Full)
                            DisplayResultMessage(false, "Hygiene full!");
                        }
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient Gems!");
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
                    if (DataManager.ReadIntData(DataManager.totalGem) >= refillVitalityCost) //Check if GEM is sufficient
                    {
                        if (GameManager.Instance.vitality < GameManager.maxStamina)
                        {
                            DeductGem(refillVitalityCost);
                            GameManager.Instance.vitality = GameManager.maxStamina;
                            DisplayResultMessage(true, "Vitality refilled!");
                        }
                        else
                        {
                            //Display Error (Bar is Full)
                            DisplayResultMessage(false, "Vitality full!");
                        }
                    }
                    else
                    {
                        //Display Error (Insufficient GEM)
                        DisplayResultMessage(false, "Insufficient Gems!");
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

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
    public static event Action<string> OnShopClosed;

    //Shop Price (Set from Unity)
    public int refillEnergyCost = 100;
    public int refillMoodCost = 100;
    public int refillHygieneCost = 100;
    public int refillVitalityCost = 100;

    public Text[] priceLabels;
    public TintedButton[] buyButtons;

    public AnimatedButton closeButton;

    void OnEnable()
    {
        priceLabels[0].text = refillEnergyCost.ToString();
        priceLabels[1].text = refillMoodCost.ToString();
        priceLabels[2].text = refillHygieneCost.ToString();
        priceLabels[3].text = refillVitalityCost.ToString();

        for (int i = 0; i < buyButtons.Length; i++)
        {
            int n = i;
            buyButtons[n].onClick.AddListener(() => AttemptPurchase(n));
        }

        closeButton.onClick.AddListener(() => ShopClosed());
    }

    void OnDisable()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            buyButtons[i].onClick.RemoveAllListeners();
        }

        closeButton.onClick.RemoveAllListeners();
    }

    protected virtual void ItemPurchased(string result)
    {
        if (OnItemPurchased != null)
            OnItemPurchased(result);
    }

    protected virtual void ShopClosed()
    {
        if (OnShopClosed != null)
            OnShopClosed("Shop closed");
    }

    void DisplayResultMessage(bool success, string message)
    {
        string result = "";

        result = success ? "Purchase Success!" : "Purchase Failed";

        ItemPurchased(message);
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
                        GameTimer.Instance.ResetLife();
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

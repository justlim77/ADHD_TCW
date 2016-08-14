using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencyBar : MonoBehaviour
{
    public Text gemLabel;

    public void UpdateGems(int value)
    {
        gemLabel.text = value.ToString();
    }
}

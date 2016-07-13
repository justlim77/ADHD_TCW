using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public Text[] acText;

    public void Initialize()
    {
        if (DataManager.ReadIntData(DataManager.acOne) == 1)
            acText[0].text = "Status: Completed";
        else
            acText[0].text = "Status: Not Achieved";

        if (DataManager.ReadIntData(DataManager.acTwo) == 1)
            acText[1].text = "Status: Completed";
        else
            acText[1].text = "Status: Not Achieved";

        if (DataManager.ReadIntData(DataManager.acThree) == 1)
            acText[2].text = "Status: Completed";
        else
            acText[2].text = "Status: Not Achieved";
    }
}

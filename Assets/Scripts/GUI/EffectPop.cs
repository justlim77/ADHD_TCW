using UnityEngine;
using System.Collections;

public class EffectPop : MonoBehaviour
{
    public GameObject startButton;
    public GameObject settingButton;
    public float scaleDuration = 1.0f;

    private WaitForSeconds waitForScale;

    public void ScaleDown()
    {
        if(!GameManager.isTempPause)
            StartCoroutine(RunScaleDown());
    }

    public void StartScaleDown()
    {
        if ((!GameManager.isTempPause) && (DataManager.ReadIntData("LIFE") > 0))
            StartCoroutine(RunScaleDown());
    }

    public void StartButtonSetActive(bool isActive)
    {
        if (DataManager.ReadIntData("LIFE") > 0)
        {
            startButton.SetActive(isActive);
            settingButton.SetActive(isActive);
        }
    }

    public IEnumerator RunScaleDown()
    {
        waitForScale = new WaitForSeconds(scaleDuration);

        iTween.ScaleTo(gameObject, iTween.Hash(
            "x", 0.0f,
            "y", 0.0f,
            "time", scaleDuration,
            "easetype", iTween.EaseType.easeInExpo
            ));

        yield return waitForScale;
    }

    public IEnumerator RunScaleUp()
    {
        waitForScale = new WaitForSeconds(scaleDuration);

        iTween.ScaleTo(gameObject, iTween.Hash(
            "x", 1.0f,
            "y", 1.0f,
            "time", scaleDuration,
            "easetype", iTween.EaseType.easeOutBounce
            ));

        yield return waitForScale;
    }
}

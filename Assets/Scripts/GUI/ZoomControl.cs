using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ZoomControl : MonoBehaviour {

    public CameraControl cameraControl;
    public Button[] roomButtons;

    void Awake()
    {
        if (!cameraControl)
            cameraControl = Camera.main.GetComponent<CameraControl>();
    }

    public void ZoomIn(int roomIndex)
    {
        ToggleButtons(false);
        cameraControl.ZoomIn(roomIndex);
    }

    public void ZoomOut()
    {
        ToggleButtons(true);
        cameraControl.ZoomOut();
    }

    void ToggleButtons(bool value)
    {
        foreach (Button button in roomButtons)
        {
            button.interactable = value;
        }
    }
}

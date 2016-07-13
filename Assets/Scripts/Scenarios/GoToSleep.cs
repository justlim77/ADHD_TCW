using UnityEngine;
using System.Collections;

public class GoToSleep : MonoBehaviour
{
    public BoxCollider2D bcBed;

    public void SetBoxCollider(bool isActive)
    {
        if(!GameManager.isTempPause)
            bcBed.enabled = isActive;
    }
}

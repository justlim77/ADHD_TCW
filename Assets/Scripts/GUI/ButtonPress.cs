using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    float sizeX, sizeY;

    // Use this for initialization
    void Start()
    {
        sizeX = transform.localScale.x;
        sizeY = transform.localScale.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", (sizeX - 0.1f), "y", (sizeY - 0.1f), "time", .3));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", sizeX, "y", sizeY, "time", .1));
    }
}

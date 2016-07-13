using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlaySFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    public AudioClip m_HoverSFX;
    public AudioClip[] m_ClickSFX;

    void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //AudioManager.Instance.PlaySFX(m_HoverSFX, 1, 0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < m_ClickSFX.Length; i++)
        {
            AudioManager.Instance.PlaySFX(m_ClickSFX[i], 1, 0.5f);
        }
    }

}

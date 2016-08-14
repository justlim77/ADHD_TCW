// Copyright (C) 2015, 2016 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// This class is based on the official source code for Unity's UI Button (which can
// be found here: https://bitbucket.org/Unity-Technologies/ui), but adds a delay before
// calling the button's on-clicked event. The reason for doing this lies in the fact
// that the demo buttons are mostly used to open popups o trigger transitions to new scenes,
// and it gives a nicer visual feeling to wait for the button animation to be played
// for a bit before executing those actions (as opposed to interrupting said animation).
public class AnimatedButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    private Animator m_animator;

    private bool m_pointerInside = false;
    private bool m_pointerPressed = false;

    override protected void Start()
    {
        base.Start();
        m_animator = GetComponent<Animator>();
    }

    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        m_pointerInside = true;
        if (m_pointerPressed)
            Press();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        m_pointerInside = false;
        Unpress();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        m_pointerPressed = false;
        Unpress();
        if (m_pointerInside)
            m_OnClick.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        m_pointerPressed = true;
        Press();
    }

    private void Press()
    {
        if (!IsActive())
            return;

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
            m_animator.Play("Pressed");
    }

    private void Unpress()
    {
        if (!IsActive())
            return;

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"))
            m_animator.CrossFade("Unpressed", 0.3f);
    }

    public void OnPressedAnimationFinished()
    {
        m_OnClick.Invoke();
    }

    public void ResetToNormalState()
    {
        m_animator.Play("Normal");
    }
}

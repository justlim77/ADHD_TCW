using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpriteButton : UIBehaviour
{
    [System.Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    [SerializeField]
    ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    bool m_PointerInside = false;
    bool m_PointerPressed = false;

    override protected void Start()
    {
        base.Start();
    }

    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    void OnMouseOver()
    {
        if (!Input.GetMouseButton(0))
            return;

        m_PointerInside = true;

        if (m_PointerPressed)
            Press();
    }

    void OnMouseExit()
    {
        if (!Input.GetMouseButton(0))
            return;

        m_PointerInside = false;

        Unpress();
    }

    void OnMouseUp()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        m_PointerPressed = false;
        Unpress();

        if (m_PointerInside)
            m_OnClick.Invoke();
    }

    void OnMouseDown()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        m_PointerPressed = true;
        Press();
    }       

    void Press()
    {
        if (!IsActive())
            return;
    }
    void Unpress()
    {
        if (!IsActive())
            return;
    }
}

/*
for (var i = 0; i < Input.touchCount; ++i) {
        if (Input.GetTouch(i).phase == TouchPhase.Began) {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
            if(hitInfo)
            {
                Debug.Log( hitInfo.transform.gameObject.name );
                // Here you can check hitInfo to see which collider has been hit, and act appropriately.
            }
        }
    }
*/

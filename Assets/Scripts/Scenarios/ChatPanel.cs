using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening;

public class ChatPanel : MonoBehaviour
{
    public TextTyper synopsisText;

    public GameObject parentResponse1;
    public GameObject parentResponse2;
    public GameObject childResponse;

    public Text parentText1;
    public Text parentText2;
    public Text childText;

    public GameObject parentIcon;
    public GameObject childIcon;

    public Color activeColor = Color.white;
    public Color inactiveColor = Color.gray;

    TextTyper m_ParentTextTyper1;
    TextTyper m_ParentTextTyper2;
    TextTyper m_ChildTextTyper;
    Image m_ParentIconImg;
    Image m_ChildIconImg;
    Button m_ParentResponseBtn1;
    Button m_ParentResponseBtn2;

    int responseIndex = 0;
    bool proceed = false;

    Graphic m_graphic;
    Graphic graphic
    {
        get
        {
            if (m_graphic == null)
            {
                m_graphic = GetComponent<Graphic>();
            }
            return m_graphic;
        }
    }

    void Awake()
    {
        m_ParentTextTyper1 = parentText1.GetComponent<TextTyper>();
        m_ParentTextTyper2 = parentText2.GetComponent<TextTyper>();
        m_ChildTextTyper = childText.GetComponent<TextTyper>();
        m_ParentIconImg = parentIcon.GetComponentInChildren<Image>();
        m_ChildIconImg = childIcon.GetComponentInChildren<Image>();
        m_ParentResponseBtn1 = parentResponse1.GetComponent<Button>();
        m_ParentResponseBtn2 = parentResponse2.GetComponent<Button>();
    }

    void Start ()
    {
        Initialize();
	}

    public void Initialize()
    {
        synopsisText.Clear();
        parentText1.text = string.Empty;
        parentText2.text = string.Empty;
        childText.text = string.Empty;

        parentResponse1.SetActive(false);
        parentResponse2.SetActive(false);
        childResponse.SetActive(false);

        parentIcon.SetActive(false);
        childIcon.SetActive(false);

        gameObject.SetActive(true);

        graphic.DOColor(activeColor, 0.5f).SetEase(Ease.InFlash);
    }

    public IEnumerator TypeSynopsis(string message)
    {
        yield return synopsisText.RunTypeText(message);
    }

    public IEnumerator ShowParentIcon()
    {
        parentIcon.SetActive(true);
        yield return 0;
    }

    public IEnumerator ShowChildIcon()
    {
        childIcon.SetActive(true);
        yield return 0;
    }

    public IEnumerator ShowParentResponses(string first, string second)
    {
        parentResponse1.SetActive(false);
        parentResponse2.SetActive(false);

        m_ParentIconImg.color = activeColor;
        m_ChildIconImg.color = inactiveColor;

        parentResponse1.SetActive(true);
        yield return m_ParentTextTyper1.RunTypeText(first);
        m_ParentResponseBtn1.interactable = true;

        parentResponse2.SetActive(true);
        yield return m_ParentTextTyper2.RunTypeText(second);
        m_ParentResponseBtn2.interactable = true;
    }

    public IEnumerator ShowChildResponse(string response)
    {
        m_ChildIconImg.color = activeColor;
        m_ParentIconImg.color = inactiveColor;

        childResponse.SetActive(true);
        yield return m_ChildTextTyper.RunTypeText(response);
    }

    public IEnumerator WaitForResponse()
    {
        while (!proceed)
            yield return null;
    }

    public void ChooseResponse(int index)
    {
        switch (index)
        {
            case 1:
                parentResponse2.SetActive(false);
                break;
            case 2:
                parentResponse1.SetActive(false);
                break;
            default:
                break;
        }

        responseIndex = index;
        proceed = true;
        m_ParentResponseBtn1.interactable = false;
        m_ParentResponseBtn2.interactable = false;
    }

    public void PrepareResponse()
    {
        proceed = false;
    }

    public int GetResponseIndex
    {
        get { return responseIndex; }
    }

    public void Lose()
    {
        gameObject.SetActive(false);
    }

    public void Done()
    {
        gameObject.SetActive(false);
    }
}

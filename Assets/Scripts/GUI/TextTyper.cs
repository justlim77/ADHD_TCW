using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextTyper : MonoBehaviour
{
    public string message;
    public float letterPause = 0.075f;
    public AudioClip typeSound1;
    public AudioClip typeSound2;

    private Text m_Text;
    private WaitForSeconds m_LetterPause;
    bool _skip = false;

    void Awake()
    {
        m_Text = GetComponent<Text>();
        m_LetterPause = new WaitForSeconds(letterPause);
    }

    void Start()
    {
        message = m_Text.text;
        m_Text.text = string.Empty;
    }

    public bool Initialize()
    {
        bool result = true;

        if (m_Text == null)
        {
            m_Text = GetComponent<Text>();
            if (m_Text == null)
            {
                result = false;
                Debug.Log("Failed to initialize TextTyper!");
            }
        }

        m_LetterPause = new WaitForSeconds(letterPause);

        return result;
    }

    public IEnumerator RunTypeText(string msg)
    {
        Clear();

        // Initialize
        message = msg;
        char[] messageArray = new char[0];
        messageArray = message.ToCharArray();

        // Type staggered chars
        foreach (char letter in messageArray)
        {
            m_Text.text += letter;
            if (typeSound1 && typeSound2)
                AudioManager.Instance.RandomizeSFX(typeSound1, typeSound2);
            if (_skip)
            {
                Skip();
                yield break;
            }
            else
                yield return m_LetterPause;
            //yield return 0;
            //yield return m_LetterPause;
        }
    }

    public void Clear()
    {
        if (m_Text != null)
            m_Text.text = string.Empty;
        message = string.Empty;
        _skip = false;
    }

    public void Skip()
    {
        m_Text.text = message;
    }

    public void FadeText()
    {
        m_Text.CrossFadeAlpha(0.0f, 1.0f, true);
    }
}
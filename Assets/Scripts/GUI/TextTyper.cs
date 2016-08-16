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

    Text _text;
    Text text
    {
        get
        {
            if (_text == null)
                _text = GetComponent<Text>();
            return _text;
        }
    }

    WaitForSeconds m_LetterPause;
    bool _skip = false;

    void OnEnable()
    {
        Initialize();
    }

    public bool Initialize()
    {
        bool result = true;

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
            text.text += letter;
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
        text.text = string.Empty;
        message = string.Empty;
        _skip = false;
    }

    public void Skip()
    {
        text.text = message;
    }

    public void FadeText()
    {
        text.CrossFadeAlpha(0.0f, 1.0f, true);
    }
}
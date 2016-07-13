using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartSequence : MonoBehaviour
{
    public CameraControl camControl;
    public float camSpeed = 3.0f;
    public GameObject gamePanel;

    public GameObject fadePanel;
    public float fadeDuration = 1.0f;
    public AudioClip bgm;

    public float showRoomDelay = 2.0f;
    
    RectTransform m_Rect;
    RectTransform m_FadePanelRect;

    void Awake ()
    {
        camControl = Camera.main.GetComponent<CameraControl>();
        m_Rect = GetComponent<RectTransform>();
        m_FadePanelRect = fadePanel.GetComponent<RectTransform>(); 
    }

	void Start ()
    {
        m_Rect.anchoredPosition = Vector2.zero;
        m_FadePanelRect.anchoredPosition = Vector2.zero;        

        AudioManager.Instance.PlayBGM(bgm);

        StartScene();
    }

    public void StartScene()
    {
        StartCoroutine(RunStartScene());
    }

    IEnumerator RunStartScene()
    {
        // Fade in scene
        yield return StartCoroutine(fadePanel.GetComponent<Fade>().FadeTo(0));

        // Move camera to play area
        iTween.MoveTo(gameObject, iTween.Hash("y", Screen.height * 1.5, "time", camSpeed));
        camControl.CameraToPlayArea();
        yield return new WaitForSeconds(camSpeed - 1.0f);

        // Set game panel active
        gamePanel.SetActive(true);
        gamePanel.GetComponent<GamePanelSequence>().Initialize();
       
        yield return null;
    }
}

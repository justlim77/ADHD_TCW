#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

using System;

[Serializable]
public class InputManager : Singleton<InputManager> {

    // Set to protected to prevent calling constructor
    protected InputManager() { }

    public Fade fade;

    Scene m_CurrentScene;

    void Start()
    {
        m_CurrentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        fade.FadeTo(1.0f);
#if UNITY_5_3
        SceneManager.LoadScene(m_CurrentScene.buildIndex);
#else
        Application.LoadLevel(Application.loadedLevel);   
#endif
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

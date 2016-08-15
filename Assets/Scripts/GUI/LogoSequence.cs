using UnityEngine;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

using System.Collections;

public class LogoSequence : MonoBehaviour
{
    public Fade fade;
    public float logoShowDuration = 1.0f;

    Scene m_CurrentScene;

	void Start ()
    {
        m_CurrentScene = SceneManager.GetActiveScene();

        StartCoroutine(RunLogoSequence());
	}

    IEnumerator RunLogoSequence()
    {
        yield return fade.FadeTo(0, 1);
        yield return new WaitForSeconds(logoShowDuration);
        yield return fade.FadeTo(1, 1);

#if UNITY_5_3_OR_NEWER
        SceneManager.LoadScene(m_CurrentScene.buildIndex + 1);
#else
        Application.LoadLevel(Application.loadedLevel + 1);
#endif
    }
}

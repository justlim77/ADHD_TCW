using UnityEngine;
using System.Collections;

public class DustRemover : MonoBehaviour
{
    public float slideDelay = 0.1f;

    SpriteRenderer sr;
    WaitForSeconds m_WaitSlideDelay;

    void OnMouseEnter ()
    {
        Destroy(gameObject);
        DustInitialize.DirtNumber();
    }

    void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        m_WaitSlideDelay = new WaitForSeconds(slideDelay);
        StartCoroutine(ShowShelf());
    }

    IEnumerator ShowShelf()
    {
        yield return m_WaitSlideDelay;

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            sr.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
}

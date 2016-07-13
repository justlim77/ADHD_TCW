using UnityEngine;
using System.Collections;

public class ShelfCleaning : MonoBehaviour
{
    public SpriteRenderer bgShelf, goShelf;
    public BoxCollider2D bcShelf;
    public float slideDelay = 0.1f;

    Color originalBG;
    WaitForSeconds m_WaitSlideDelay;

    void Start()
    {
        originalBG = bgShelf.color;
        m_WaitSlideDelay = new WaitForSeconds(slideDelay);
    }

    public void Initialize()
    {
        if (!GameManager.isTempPause)
        {
            StartCoroutine(ShowShelf());
            GameManager.isTempPause = true;
        }
    }

    public void DeInitialize()
    {
        StartCoroutine(HideSelf());
    }

    IEnumerator ShowShelf()
    {
        bgShelf.gameObject.SetActive(true);
        goShelf.gameObject.SetActive(true);

        yield return m_WaitSlideDelay;

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            bgShelf.color = new Color(originalBG.r, originalBG.g, originalBG.b, i);
            goShelf.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    IEnumerator HideSelf()
    {
        yield return m_WaitSlideDelay;

        for (float i = 1; i > 0; i -= Time.deltaTime)
        {
            bgShelf.color = new Color(originalBG.r, originalBG.g, originalBG.b, i);
            goShelf.color = new Color(1, 1, 1, i);
            yield return null;
        }

        bgShelf.gameObject.SetActive(false);
        goShelf.gameObject.SetActive(false);
    }

    public void SetBoxCollider(bool isActive)
    {
        bcShelf.enabled = isActive;
    }

    public void RestoreBoxCollider()
    {
        if(!GameManager.isCleanShelfDone)
            bcShelf.enabled = true;
    }
}

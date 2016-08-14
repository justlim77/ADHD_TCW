using UnityEngine;
using System.Collections;

public class ShakingAnimation : MonoBehaviour
{
    public float speed = 5;
    public bool useSmall = false;

    float m, i;
    float h = 20;
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void StartAnimation()
    {
        StartCoroutine("ShakeAnimation");
    }

    public void StopAnimation()
    { 
        StopCoroutine("ShakeAnimation");
        transform.localScale = originalScale;
    }

    IEnumerator ShakeAnimation()
    {
        while (true)
        {
            while ((i < 0.35f) && (!GameManager.isGamePause))
            {
                m += Time.deltaTime * speed;

                if (useSmall)
                {
                    i = m * 10;
                    h = (Mathf.PingPong(m, 0.006f) - 0.003f);
                }
                else
                {
                    i = m;
                    h = (Mathf.PingPong(m, 0.06f) - 0.03f);
                }

                transform.localScale = new Vector3((originalScale.x - h), (originalScale.y - h), originalScale.z);

                yield return null;
            }

            yield return new WaitForSeconds(2);
            m = 0;
            i = 0;
        }
    }
}

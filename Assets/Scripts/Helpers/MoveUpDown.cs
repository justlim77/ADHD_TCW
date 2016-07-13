using UnityEngine;
using System.Collections;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 100;
    Vector2 originalPos;

    void Start()
    {
        originalPos = transform.position;
        Debug.Log("XX");
    }

    public void MoveMe(bool isReturn)
    {
        if (!GameManager.isTempPause)
            StartCoroutine(MoveMe_(isReturn));
    }

    public void ResetPos()
    {
        transform.position = originalPos;
    }
   
    IEnumerator MoveMe_(bool isReturn)
    {
        float distance = 1;
        float t = 0;
        Vector2 pos, target;

        pos = transform.position;
        
        if(isReturn)
            target = originalPos;
        else
            target = new Vector2(transform.position.x, -5);

        yield return new WaitForSeconds(0.3f);

        while (distance > 0.1)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector2.MoveTowards(pos, target, t);
            distance = Vector2.Distance(transform.position, target);
            yield return null;
        }
        transform.position = target;
    }
}

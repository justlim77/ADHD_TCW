using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CleaningGame : MonoBehaviour
{
    public static event Action<string> OnDustCleared;

    public Transform dustPrefab;
    static Transform dirtParent;

    public float xBorderPercentage = 0.8f;
    public float yBorderPercentage = 0.8f;

    public float dustScale;
    static float timer;

    bool isLoadDone = false;

    Vector2 min, max;
    Transform dirt;
    SpriteRenderer sr;
    int dustSpawn;

    void Awake ()
    {
        dirtParent = transform;
    }

    void Start()
    {
        LoadDust();
    }

    protected static void DustCleared()
    {
        if (OnDustCleared != null)
            OnDustCleared("Dust cleared");
    }

    public void LoadDust()
    {
        if (!GameManager.isCleanShelfDone)
        {
            isLoadDone = false;
            Debug.Log("Loading Dust");

            transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);

            sr = GetComponent<SpriteRenderer>();
            min = sr.bounds.min;
            max = sr.bounds.max;
            float width = sr.bounds.size.x;
            float height = sr.bounds.size.y;

            if (GameManager.Instance.vitality <= 0)
                dustSpawn = 40;
            else
                dustSpawn = 20;

            for (int i = 0; i < dustSpawn; i++)
            {
                dirt = Instantiate(dustPrefab, new Vector2(
                    UnityEngine.Random.Range(min.x + (width * (1.0f - xBorderPercentage)) , max.x - (width * (1.0f - xBorderPercentage))), 
                    UnityEngine.Random.Range(min.y + (height * (1.0f - yBorderPercentage)), max.y - (height * (1.0f - yBorderPercentage)))), 
                    Quaternion.identity) as Transform;
                dirt.SetParent(transform);
                dirt.localScale = new Vector2(dustScale, dustScale);
            }

            timer = 0;
            isLoadDone = true;
        }
    }

    public static void DirtNumber()
    {
        Debug.Log(string.Format("{0} dust left", dirtParent.childCount));

        if(dirtParent.childCount <= 1)
        {
            DustCleared();

            GameManager.isCleanShelfDone = true;            

            if((timer < 5) && (DataManager.ReadIntData(DataManager.acTwo) == 0))
                DataManager.StoreIntData(DataManager.acTwo, 1);
        }
    }

    void Update()
    {
        if(isLoadDone)
            timer += Time.deltaTime;
    }
}

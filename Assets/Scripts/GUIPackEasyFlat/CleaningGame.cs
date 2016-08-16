using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CleaningGame : MonoBehaviour
{
    public static event Action<string> OnDustCleared;

    public SettingSequence shelfPanel;
    public Transform dustPrefab;

    static Transform dirtParent;
    static SettingSequence shelfPanel_;

    public float scale;
    static float timer;

    bool isLoadDone = false;

    Vector2 min, max;
    Transform dirt;
    SpriteRenderer sr;
    int dustSpawn;

    void Awake ()
    {
        dirtParent = transform;
        shelfPanel_ = shelfPanel;
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

            if (GameManager.lvlStamina <= 0)
                dustSpawn = 40;
            else
                dustSpawn = 20;

            for (int i = 0; i < dustSpawn; i++)
            {
                dirt = Instantiate(dustPrefab, new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y)), Quaternion.identity) as Transform;
                dirt.SetParent(transform);
                dirt.localScale = new Vector2(scale, scale);
            }

            timer = 0;
            isLoadDone = true;
        }
    }

    public static void DirtNumber()
    {
        if(dirtParent.childCount <= 1)
        {
            DustCleared();

            GameManager.SetInteractable(false);
            GameManager.isCleanShelfDone = true;
            shelfPanel_.OpenSettings(true); // Show dust cleaned



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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DustInitialize : MonoBehaviour
{
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

    public void LoadDust()
    {
        if (!GameManager.isCleanShelfDone)
        {
            isLoadDone = false;
            Debug.Log("Loading Dust");

            sr = GetComponent<SpriteRenderer>();
            min = sr.bounds.min;
            max = sr.bounds.max;

            if (GameManager.lvlStamina <= 0)
                dustSpawn = 40;
            else
                dustSpawn = 20;

            for (int i = 0; i < dustSpawn; i++)
            {
                dirt = Instantiate(dustPrefab, new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y)), Quaternion.identity) as Transform;
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
            GameManager.SetInteractable(false);
            GameManager.isCleanShelfDone = true;
            shelfPanel_.OpenSettings(true);

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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CameraControl : MonoBehaviour
{
    public delegate void CameraReachedEventHandler(object sender);
    public static event CameraReachedEventHandler OnCameraReached;

    public float cameraDepth = -10.0f;
    public Vector3 OriginalPos;
    public Vector3 StartPos = new Vector3(0.0f, 6.4f, -10f);
    public Vector3 PlayPos = new Vector3(0.0f, 0.0f, -10f);
    public bool animate = true;
    public float timeToReach = 2.0f; 
    public float CamSpeed = 5.0f;
    public float panSpeed = 1.0f;
    public float touchPanFactor = 0.1f;
    public float panLowerLimit = 0.0f;
    public float panUpperLimit = 10.0f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutQuart;
    public enum ZoomType { In, Out };
    public float zoomInterval = 1.0f;

    public Vector3 kidBedroomPos;
    public Vector3 parentBedroomPos;
    public Vector3 studyRoomPos;
    public Vector3 livingRoomPos;
    public Vector3 kitchenPos;
    public Vector3 laundryRoomPos;

    int m_RoomIndex;
    Dictionary<int, Vector3> locations = new Dictionary<int, Vector3>();

    ZoomType m_ZoomType;
    float m_OriginalOrthoSize;
    float m_ZoomedOrthoSize;
    bool m_allowTouchPanning = false;

    void OnEnable()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        ShelfCleaning.OnCleaningGameOpen += ShelfCleaning_OnCleaningGameOpen;
    }

    private void ShelfCleaning_OnCleaningGameOpen(string obj)
    {
        m_allowTouchPanning = false;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        ShelfCleaning.OnCleaningGameOpen -= ShelfCleaning_OnCleaningGameOpen;
    }

    private void GameManager_OnGameStateChanged(object sender, GameState e)
    {
        switch (e)
        {
            case GameState.Default:
                Initialize();
                StartCoroutine(RunCamToPlayArea());
                break;
            case GameState.Minigame:
                m_allowTouchPanning = false;
                break;
        }
    }

    bool Initialize()
    {
        Camera.main.transform.position = StartPos;  // Reset camera position to start
        m_allowTouchPanning = false;    // Disable camera touch panning
        return true;
    }

    protected void CameraReached()
    {
        Debug.Log("Camera reached destination.");
        m_allowTouchPanning = true;

        if (OnCameraReached != null)
            OnCameraReached(this);
    }

    void Awake () {
        OriginalPos = Camera.main.transform.position;
    }

	void Start () {
        // Initialize camera
        Camera.main.transform.position = StartPos;

        m_OriginalOrthoSize = Camera.main.orthographicSize;
        m_ZoomedOrthoSize = Camera.main.orthographicSize * 0.5f;

        // Initialize room zooming positions
        locations.Add(0, PlayPos);
        locations.Add(1, kidBedroomPos);
        locations.Add(2, parentBedroomPos);
        locations.Add(3, studyRoomPos);
        locations.Add(4, livingRoomPos);
        locations.Add(5, kitchenPos);
        locations.Add(6, laundryRoomPos);
	}

    Vector3 velocity = Vector3.zero;
    void Update()
    {
        // If touch panning isn't allowed, return
        if (!m_allowTouchPanning)
            return;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
                transform.Translate(0.0f, -touchDelta.y * (panSpeed * touchPanFactor) * Time.deltaTime, 0.0f);
                Vector3 newPos = transform.position;
                newPos.y = Mathf.Clamp(newPos.y, panLowerLimit, panUpperLimit);
                transform.position = newPos;
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetMouseButton(1))
            {
                var newPos = transform.position;

                if (Input.GetAxis("Mouse Y") > 0)
                {
                    newPos += new Vector3(0.0f, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * panSpeed, 0.0f);
                }
                else if (Input.GetAxis("Mouse Y") < 0)
                {
                    newPos += new Vector3(0.0f, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * panSpeed, 0.0f);
                }

                newPos.y = Mathf.Clamp(newPos.y, panLowerLimit, panUpperLimit);
                transform.DOMoveY(newPos.y, 0.1f);
                //transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, 0.3f);
            }
        }
    }

    public void CameraToPlayArea()
    {
        StartCoroutine(RunCamToPlayArea());
    }

    public void ZoomIn(int roomIndex)
    {
        m_RoomIndex = roomIndex;            // First room (1) to last room (6)
        m_ZoomType = ZoomType.In;
        StartCoroutine(RunZoom(m_ZoomType));
    }

    public void ZoomOut()
    {
        m_RoomIndex = 0;                    // Indication of outer house
        m_ZoomType = ZoomType.Out;
        StartCoroutine(RunZoom(m_ZoomType));
    }

    IEnumerator RunCamToPlayArea()
    {        
        yield return StartCoroutine(Transition.Fade(FadeType.ToClear, 1.0f));

        if (animate)
        {
            iTween.MoveTo(gameObject,
                iTween.Hash(
                "y", PlayPos.y,
                "time", timeToReach,
                "easetype", easeType,
                "oncomplete", "CameraReached"
                ));
        }
        else
        {
            transform.position = PlayPos;
            CameraReached();
        }

        yield return null;
    }

    IEnumerator RunZoom(ZoomType _zoomType)
    {
        float targetOrthoSize = 0;
        Vector3 targetPos = locations[m_RoomIndex];
         

        switch (_zoomType)
        {
            case ZoomType.In:
                targetOrthoSize = m_ZoomedOrthoSize;
                break;
            case ZoomType.Out:
                targetOrthoSize = m_OriginalOrthoSize;
                break;
        }

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", Camera.main.orthographicSize,
            "to", targetOrthoSize,
            "time", zoomInterval,
            "onupdatetarget", gameObject,
            "onupdate", "ZoomOnUpdateCallback",
            "easetype", iTween.EaseType.easeOutQuad
            )
        );

        while (Vector3.Distance(Camera.main.transform.position, targetPos) > 0.01f)
        {
            Camera.main.transform.position = iTween.Vector3Update(Camera.main.transform.position, targetPos, 5f);
            yield return null;
        }

        Camera.main.transform.position = targetPos;
        print("Cam reached: " + Camera.main.transform.position);

        yield return null;
    }

    // Zoom callback
    void ZoomOnUpdateCallback(float value)
    {
        Camera.main.orthographicSize = value;
    }

    // Move callback
    void MoveOnUpdateCallback(Vector2 value)
    {
        Camera.main.transform.position = value;
    }

    public void AllowPan(bool value)
    {
        m_allowTouchPanning = value;
    }
}

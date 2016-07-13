using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

    public float cameraDepth = -10.0f;
    public Vector3 OriginalPos;
    public Vector3 StartPos = new Vector3(0.0f, 6.4f, -10f);
    public Vector3 PlayPos = new Vector3(0.0f, 0.0f, -10f);
    public float timeToReach = 2.0f; 
    public float CamSpeed = 5.0f;
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
        iTween.MoveTo(gameObject, iTween.Hash("y", PlayPos.y, "time", timeToReach, "easetype", iTween.EaseType.easeOutQuart));

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
}

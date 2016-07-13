using UnityEngine;
using System.Collections;

public class OrthoResize : MonoBehaviour {

    public float PixelsPerUnit = 100.0f;

    void Awake () {
        Camera.main.orthographicSize = Screen.height / (2 * PixelsPerUnit);
    }

	void Start () {
	
	}
	
	void Update () {
    }
}

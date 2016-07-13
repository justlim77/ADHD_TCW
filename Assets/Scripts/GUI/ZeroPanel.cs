using UnityEngine;
using System.Collections;

public class ZeroPanel : MonoBehaviour {

    void Awake() {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}

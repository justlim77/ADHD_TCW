using UnityEngine;
using System.Collections;

public class DisableOnStart : MonoBehaviour {

    void OnEnable() {
        gameObject.SetActive(false);
    }
}

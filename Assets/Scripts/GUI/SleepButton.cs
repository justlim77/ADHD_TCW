using UnityEngine;
using System.Collections;
using System;

public class SleepButton : MonoBehaviour
{
    public static event Action<string> OnBedPressed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BedPressed()
    {
        if (OnBedPressed != null)
            OnBedPressed("Bed pressed");
    }
}

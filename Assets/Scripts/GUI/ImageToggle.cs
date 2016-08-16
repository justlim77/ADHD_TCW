using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ToggleType
{
    Default,
    Alternate
}
public class ImageToggle : MonoBehaviour
{
    public Image defaultImage;
    public Image alternateImage;

    public bool isDefault
    {
        get
        {
            return defaultImage.enabled;
        }
    }

    void Start()
    {

    }

    public void Toggle()
    {
        defaultImage.enabled = !defaultImage.enabled;
        alternateImage.enabled = !alternateImage.enabled;
    }
}

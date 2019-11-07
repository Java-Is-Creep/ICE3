using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameObject up;
    public GameObject down;
    public GameObject right;
    public GameObject left;
    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            up.SetActive(false);
            down.SetActive(false);
            right.SetActive(false);
            left.SetActive(false);
        }
    }
}

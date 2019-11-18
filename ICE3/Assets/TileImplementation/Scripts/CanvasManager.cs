using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button up;
    public Button down;
    public Button right;
    public Button left;
    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
       } else
        {
            CharacterController personaje = FindObjectOfType<CharacterController>();

            up.onClick.AddListener(personaje.w);
            down.onClick.AddListener(personaje.s);
            left.onClick.AddListener(personaje.a);
            right.onClick.AddListener(personaje.d);

        }
    }
}

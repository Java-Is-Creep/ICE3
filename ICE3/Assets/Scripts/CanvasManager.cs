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
        if (Application.isMobilePlatform)
        {
            up.gameObject.SetActive(true);
            down.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
            left.gameObject.SetActive(true);
       
            CharacterController personaje = FindObjectOfType<CharacterController>();

            up.onClick.AddListener(personaje.w);
            down.onClick.AddListener(personaje.s);
            left.onClick.AddListener(personaje.a);
            right.onClick.AddListener(personaje.d);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class moverCamaraFija : MonoBehaviour
{

    Animator anim;
    int cara;
    int lastCara;
    public CinemachineVirtualCamera topCam;
    public CinemachineVirtualCamera buttonCam;
    public CinemachineVirtualCamera frontCam;
    public CinemachineVirtualCamera backCam;
    public CinemachineVirtualCamera leftCam;
    public CinemachineVirtualCamera rightCam;

    public CinemachineFreeLook freeLookCam;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        cara = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hola");
        if (Input.GetMouseButtonDown(0))
        {
            lastCara = cara;
            if (cara == 0)
            {
                freeLookCam.transform.position = topCam.transform.position;
            }
            else if (cara == 1)
            {
                freeLookCam.transform.position = leftCam.transform.position;
            }
            else if (cara == 2)
            {
                freeLookCam.transform.position = buttonCam.transform.position; 
            }
            else if (cara == 3)
            {
                freeLookCam.transform.position = rightCam.transform.position;
            }
            else if (cara == 4)
            {
                freeLookCam.transform.position = frontCam.transform.position;
            }
            else if (cara == 5)
            {
                freeLookCam.transform.position = backCam.transform.position;
            }
            Free();
        } else if (Input.GetMouseButtonUp(0))
        {
            if (lastCara == 0)
            {
                top();
            } else if (lastCara == 1)
            {
                left();
            }
            else if (lastCara == 2)
            {
                button();
            }
            else if (lastCara == 3)
            {
                right();
            }
            else if (lastCara == 4)
            {
                front();
            }
            else if (lastCara == 5)
            {
                back();
            }
        } 
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("0");
            top();
        }
        else if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("1");
            left();
        } else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("2");
            button();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("3");
            right();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("4");
            front();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("5");
            back();
        } 
    }

    public void front()
    {
        cara = 4;
        anim.SetInteger("cara", 4);
    }

    public void back()
    {
        cara = 5;
        anim.SetInteger("cara", 5);
    }

    public void top()
    {
        cara = 0;
        anim.SetInteger("cara", 0);
    }

    public void button()
    {
        cara = 2;
        anim.SetInteger("cara", 2);
    }

    public void left()
    {
        cara = 1;
        anim.SetInteger("cara", 1);
    }

    public void right()
    {
        cara = 3;
        anim.SetInteger("cara", 3);
    }

    public void Free()
    {
        anim.SetInteger("cara", 6);
    }


}

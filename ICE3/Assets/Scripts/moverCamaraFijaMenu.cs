using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class moverCamaraFijaMenu : MonoBehaviour
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

    public Canvas cvTop;
    public Canvas cvFront;
    public Canvas cvBottom;
    public Canvas cvLeft;
    public Canvas cvRight;
    public Canvas cvBack;

    private Canvas activedCanvas;

    public AudioSource accion;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        cara = 0;
        activedCanvas = cvFront;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Keypad0))
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
        */
    }

    public void front()
    {
        //Debug.Log("Front");
        activedCanvas.gameObject.SetActive(false);
        cvFront.gameObject.SetActive(true);
        activedCanvas= cvFront;
        cara = 4;
        anim.SetInteger("cara", 4);
        accion.Play();
    }

    public void back()
    {
        activedCanvas.gameObject.SetActive(false);
        cvBack.gameObject.SetActive(true);
        activedCanvas = cvBack;
        cara = 5;
        anim.SetInteger("cara", 5);
        accion.Play();
    }

    public void top()
    {
        Debug.Log("Top");
        activedCanvas.gameObject.SetActive(false);
        cvTop.gameObject.SetActive(true);
        activedCanvas = cvTop;
        cara = 0;
        anim.SetInteger("cara", 0);
        accion.Play();
    }

    public void button()
    {
        activedCanvas.gameObject.SetActive(false); ;
        cvBottom.gameObject.SetActive(true);
        activedCanvas = cvBottom;
        cara = 2;
        anim.SetInteger("cara", 2);
        accion.Play();
    }

    public void left()
    {
        activedCanvas.gameObject.SetActive(false);
        cvLeft.gameObject.SetActive(true);
        activedCanvas = cvLeft;
        cara = 1;
        anim.SetInteger("cara", 1);
        accion.Play();
    }

    public void right()
    {
        activedCanvas.gameObject.SetActive(false);
        cvRight.gameObject.SetActive(true);
        activedCanvas = cvRight;
        cara = 3;
        anim.SetInteger("cara", 3);
        accion.Play();
    }

}

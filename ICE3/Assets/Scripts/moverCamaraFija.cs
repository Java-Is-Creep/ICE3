using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class moverCamaraFija : MonoBehaviour
{

    Animator anim;
    //int cara;
    //int lastCara;
    public CinemachineVirtualCamera topCam;
    public CinemachineVirtualCamera buttonCam;
    public CinemachineVirtualCamera frontCam;
    public CinemachineVirtualCamera backCam;
    public CinemachineVirtualCamera leftCam;
    public CinemachineVirtualCamera rightCam;

    public CinemachineFreeLook freeLookCam;

    public CharacterController personaje;

    public bool hecho = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        //cara = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hecho)
        {
            CharacterController[] cc = FindObjectsOfType<CharacterController>();
            foreach (CharacterController c in cc)
            {
                if (c.photonView.IsMine)
                {
                    personaje = c;
                }
            }
            hecho = true;
        }

        if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Free();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (personaje.cara == 0)
                {
                    top();
                }
                else if (personaje.cara == 4)
                {
                    left();
                }
                else if (personaje.cara == 5)
                {
                    button();
                }
                else if (personaje.cara == 3)
                {
                    right();
                }
                else if (personaje.cara == 2)
                {
                    front();
                }
                else if (personaje.cara == 1)
                {
                    back();
                }
            }
        }
        else
        {

        }
            //Debug.Log("Clic" + Input.GetMouseButtonDown(0));
            //Debug.Log("Estoy no tocando interfaz" + !EventSystem.current.IsPointerOverGameObject());
            //Debug.Log("Pos camara free: " + freeLookCam.transform.position);
            
            /*
            lastCara = cara;
            if (personaje.cara == 0)
            {
                Debug.Log("Cara top colocando camara");
                Debug.Log("Pos camara free antes: " + freeLookCam.transform.position);
                Debug.Log("Pos camara top antes: " + topCam.transform.position);
                freeLookCam.transform.position = topCam.transform.position;
                Debug.Log("Pos camara free despues: " + freeLookCam.transform.position);

            }
            else if (personaje.cara == 4)
            {
                freeLookCam.transform.position = leftCam.transform.position;
            }
            else if (personaje.cara == 5)
            {
                freeLookCam.transform.position = buttonCam.transform.position; 
            }
            else if (personaje.cara == 3)
            {
                freeLookCam.transform.position = rightCam.transform.position;
            }
            else if (personaje.cara == 2)
            {
                freeLookCam.transform.position = frontCam.transform.position;
            }
            else if (personaje.cara == 1)
            {
                freeLookCam.transform.position = backCam.transform.position;
            }*/
            
        /*
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
           
            Debug.Log("0");
            top();
        }
        else if(Input.GetKeyDown(KeyCode.Keypad5))
        {
            back();
            Debug.Log("1");
        } else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            front();
            Debug.Log("2");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("3");
            right();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            left();
            Debug.Log("4");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            button();
            Debug.Log("5");
        } */
    }

    public void front()
    {
        //cara = 4;
        anim.SetInteger("cara", 4);
    }

    public void back()
    {
        //cara = 5;
        anim.SetInteger("cara", 5);
    }

    public void top()
    {
        //cara = 0;
        anim.SetInteger("cara", 0);
    }

    public void button()
    {
        //cara = 2;
        anim.SetInteger("cara", 2);
    }

    public void left()
    {
        //cara = 1;
        anim.SetInteger("cara", 1);
    }

    public void right()
    {
        //cara = 3;
        anim.SetInteger("cara", 3);
    }

    public void Free()
    {
        anim.SetInteger("cara", 6);
    }


}

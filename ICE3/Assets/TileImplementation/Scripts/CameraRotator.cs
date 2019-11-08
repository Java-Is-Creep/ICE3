using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotator : MonoBehaviour
{
    private float speedRotation = 100f;

    private float xRot = 0f;
    private float yRot = 0f;

    private float distance = 30f;
    Quaternion rotation;
    Quaternion adjustmentRotation;
    private Vector3 pivot = new Vector3(7f, -7f, 7f);
    Camera camera;
    bool wasRotating;
    private float lastAngle;
    Quaternion lastCamara;
    Quaternion camaraAux;

    private float originalFOV = 60f;
    private float freeLookFOV = 75f;
    bool freeLook = false;

    float framesToRotate = 60;
    bool hasToRotate = false;
    float i = 0;
    Quaternion rotacionInicial;

    int cara = 0;
    int cara_anterior = 0;
    int direction;


    // Start is called before the first frame update
    void Start()
    {
        camera = this.transform.GetChild(0).GetComponent<Camera>();
        //camera.LookAt(pivot);

        wasRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasToRotate)
        {
            float aux = i / framesToRotate;
            //Debug.Log("a");
            //Debug.Log(aux);
            //EJE DE ROTACION 0
            if (cara == 1 && direction == 0)
            {
                this.transform.rotation = Quaternion.Euler(50 * aux + rotacionInicial.eulerAngles.x, 50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 2 && direction == 0)
            {
                this.transform.rotation = Quaternion.Euler(50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 3 && direction == 0)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 0 && direction == 0)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            //EJE DE ROTACION 1
            else if (cara == 3 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(+50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, +90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 2 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(+50 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, +90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 1 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, +90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 0 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, +90 * aux + rotacionInicial.eulerAngles.z);

            }
            //EJE DE ROTACION 2
            else if (cara == 5 && direction == 2)
            {
                this.transform.rotation = Quaternion.Euler(-100 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, 0 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 2 && direction == 2)
            {
                this.transform.rotation = Quaternion.Euler(+90 * aux + rotacionInicial.eulerAngles.x, -180 * aux + rotacionInicial.eulerAngles.y, +0 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 4 && direction == 2)
            {
                this.transform.rotation = Quaternion.Euler(-20 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, +180 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 0 && direction == 2)
            {
                this.transform.rotation = Quaternion.Euler(-90 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, 0 * aux + rotacionInicial.eulerAngles.z);
            }
            //EJE DE ROTACION 3
            else if (cara == 4 && direction == 3)
            {
                this.transform.rotation = Quaternion.Euler(+90 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, 0 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 2 && direction == 3)
            {
                this.transform.rotation = Quaternion.Euler(10 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, -180 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 5 && direction == 3)
            {
                this.transform.rotation = Quaternion.Euler(-200 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, +180 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 0 && direction == 3)
            {
                this.transform.rotation = Quaternion.Euler(-100 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, 0 * aux + rotacionInicial.eulerAngles.z);
            }
            //EJE DE ROTACION 4
            else if (cara == 3 && direction == 4)
            {
                this.transform.rotation = Quaternion.Euler(-40 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, 90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 5 && direction == 4)
            {
                this.transform.rotation = Quaternion.Euler(-150 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 1 && direction == 4)
            {
                //ESTE ESTA MAAAAAAAAAAAL
                this.transform.rotation = Quaternion.Euler(+150 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 4 && direction == 4)
            {
                this.transform.rotation = Quaternion.Euler(40 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y,+90 * aux + rotacionInicial.eulerAngles.z);
            }
            //EJE DE ROTACION 5
            else if (cara == 1 && direction == 5)
            {
                this.transform.rotation = Quaternion.Euler(-40 * aux + rotacionInicial.eulerAngles.x, 50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 5 && direction == 5)
            {
                this.transform.rotation = Quaternion.Euler(-150 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, 90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 3 && direction == 5)
            {
                //ESTE ESTA MAAAAAAAAAAAL
                this.transform.rotation = Quaternion.Euler(+150 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, 90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 4 && direction == 5)
            {
                this.transform.rotation = Quaternion.Euler(40 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }

            /*
            if (cara == 3 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);

            } else if (cara == 2 && direction == 0)
            {
                this.transform.rotation = Quaternion.Euler(50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 1 && direction == 0)
            {
                this.transform.rotation = Quaternion.Euler(50 * aux + rotacionInicial.eulerAngles.x, 50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 0 && direction == 0)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, 90 * aux + rotacionInicial.eulerAngles.z);
            }

            if (cara == 1 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, 90 * aux + rotacionInicial.eulerAngles.z);

            }
            else if (cara == 2 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(50 * aux + rotacionInicial.eulerAngles.x, 50 * aux + rotacionInicial.eulerAngles.y, 90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 3 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(50 * aux + rotacionInicial.eulerAngles.x, -50 * aux + rotacionInicial.eulerAngles.y, 90 * aux + rotacionInicial.eulerAngles.z);
            }
            else if (cara == 0 && direction == 1)
            {
                this.transform.rotation = Quaternion.Euler(-50 * aux + rotacionInicial.eulerAngles.x, +50 * aux + rotacionInicial.eulerAngles.y, -90 * aux + rotacionInicial.eulerAngles.z);

            }


            else if (cara == 4 && direction == 2)
            {
                this.transform.rotation = Quaternion.Euler(90 * aux + rotacionInicial.eulerAngles.x, 0 * aux + rotacionInicial.eulerAngles.y, 0 * aux + rotacionInicial.eulerAngles.z);
            }
            */
            i++;
            if ( i> 60)
            {
                hasToRotate = false;
                i = 0;
            }
        }                                                               //Izquierda                 Derecha
        // cara 0:  -50, 0, 0
        //cara 1:  0, 50, -90                                           //X: +50, +50, -50, -50       x: +50, +50, -50, -50
        //cara 2:  50, 0, -180                                          //Y: +50, -50, -50, +50       y: -50, +50, -50, +50
        // cara 3: 0, -50, 90                                        //Z: -90, -90, -90, -90       z: +90, +90, +90, +90
        //cara 4: 40, 0, 0
        //cara 5: -150, 0, 0

        //goToNewFace();

        if (!hasToRotate)
        {
            goToNewFace();
            /*
            if (Input.GetKeyDown("a"))
            {
                cara--;
                direction = 0;
                if (cara == -1)
                {
                    cara = 3;
                }
                i = 0;
                rotacionInicial = this.transform.rotation;
                hasToRotate = true;
                Debug.Log(cara);
            }
            else if (Input.GetKeyDown("d"))
            {
                direction = 1;
                cara = Mathf.Abs((cara + 1) % 4);
                Debug.Log(cara);
                i = 0;
                rotacionInicial = this.transform.rotation;
                hasToRotate = true;
                //this.transform.Rotate(-50, -90, 0);

            } else if (Input.GetKeyDown("w"))
            {
                direction = 2;
                cara = 4;
                i = 0;
                rotacionInicial = this.transform.rotation;
                hasToRotate = true;
            }
            */

            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("Guardamos rotacion de la camara");
                    lastCamara = this.transform.rotation;
                    //Debug.Log("Hemos guardado " + lastCamara.eulerAngles);
                    freeLook = true;
                    //camera.fieldOfView = freeLookFOV;
                }
                if (Input.GetMouseButton(0))
                {
                    if (freeLook == true)
                    {
                        if (camera.fieldOfView < freeLookFOV)
                        {
                            camera.fieldOfView += 1f;
                            if (camera.fieldOfView > freeLookFOV)
                            {
                                camera.fieldOfView = freeLookFOV;
                            }
                        }
                        //Debug.Log(this.transform.eulerAngles.x);
                        if (transform.eulerAngles.x <= 75f || transform.eulerAngles.x >= 285f)
                        {
                            //Debug.Log("Puedes mover");
                            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * Time.deltaTime * speedRotation, Input.GetAxis("Mouse X") * Time.deltaTime * speedRotation, 0));
                            float z = transform.eulerAngles.z;
                            transform.Rotate(0, 0, -z);
                            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, lastCamara.z);
                        }
                        else
                        {
                            //Debug.Log("No mover");
                            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * Time.deltaTime * speedRotation, Input.GetAxis("Mouse X") * Time.deltaTime * speedRotation, 0));
                            float z1 = transform.eulerAngles.z;
                            transform.Rotate(0, 0, -z1);
                            if (transform.eulerAngles.x < 100)
                            {
                                // Debug.Log("Estoy abajo: ahora: " + transform.eulerAngles.x + " antes: " + lastAngle);
                                if (transform.eulerAngles.x - lastAngle < 0)
                                {
                                    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * Time.deltaTime * speedRotation, Input.GetAxis("Mouse X") * Time.deltaTime * speedRotation, 0));
                                    float z = transform.eulerAngles.z;
                                    transform.Rotate(0, 0, -z);
                                }
                                else
                                {
                                    transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Time.deltaTime * speedRotation, -Input.GetAxis("Mouse X") * Time.deltaTime * speedRotation, 0));
                                    float z = transform.eulerAngles.z;
                                    transform.Rotate(0, 0, -z);
                                }
                            }
                            else if (transform.eulerAngles.x > 250)
                            {
                                //Debug.Log("Estoy arriba: ahora: " + transform.eulerAngles.x + " antes: " + lastAngle);
                                if (transform.eulerAngles.x - lastAngle > 0)
                                {
                                    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * Time.deltaTime * speedRotation, Input.GetAxis("Mouse X") * Time.deltaTime * speedRotation, 0));
                                    float z = transform.eulerAngles.z;
                                    transform.Rotate(0, 0, -z);
                                }
                                else
                                {
                                    transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Time.deltaTime * speedRotation, -Input.GetAxis("Mouse X") * Time.deltaTime * speedRotation, 0));
                                    float z = transform.eulerAngles.z;
                                    transform.Rotate(0, 0, -z);
                                }
                            }

                        }


                        lastAngle = transform.eulerAngles.x;

                        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                        //transform.rotation =  Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0) ;
                        //camera.LookAt(pivot);
                        //transform.RotateAround(new Vector3 (7f, 7f, 7f), new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speedRotation);
                        //transform.RotateAround(new Vector3(7f, 7f, 7f), new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), speedRotation*Time.deltaTime);
                        //transform.LookAt(new Vector3(7f, 7f, 7f));
                        //transform.RotateAround(new Vector3(7f, 7f, 7f), Vector3.up, speedRotation * Time.deltaTime);

                        /*
                        xRot += Input.GetAxis("Mouse Y") * speedRotation * Time.deltaTime;
                        yRot += Input.GetAxis("Mouse X") * speedRotation * Time.deltaTime;


                        if (xRot > 90f)
                        {
                            xRot = 90f;
                        }
                        else if (xRot < -90f)
                        {
                            xRot = -90f;
                        }

                        transform.position = target + Quaternion.Euler(xRot, yRot, 0f) * (distance * -Vector3.back);

                        transform.LookAt(target, Vector3.up);
                        */
                        /*
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Set distance to the current distance of the target
                            distance = Vector3.Distance(transform.position, pivot);

                            // Set the x and y rotation to the new rotation relative to the pivot
                            Vector3 pivotToHere = transform.position - pivot;
                            Vector3 tempVec = Vector3.ProjectOnPlane(pivotToHere, Vector3.up);
                            if (pivotToHere.x > 0f)
                                yRot = Vector3.Angle(Vector3.forward, tempVec) + 180f;
                            else
                                yRot = -Vector3.Angle(Vector3.forward, tempVec) + 180f;

                            if (pivotToHere.y > 0f)
                                xRot = Vector3.Angle(tempVec, pivotToHere);
                            else
                                xRot = -Vector3.Angle(tempVec, pivotToHere);

                            rotation = Quaternion.Euler(xRot, yRot, 0);
                            adjustmentRotation = Quaternion.FromToRotation(rotation * Vector3.forward, transform.forward);

                        }

                        if (Input.GetMouseButton(0))
                        {
                            xRot -= Input.GetAxis("Mouse Y") * Time.deltaTime * speedRotation;
                            yRot += Input.GetAxis("Mouse X") * Time.deltaTime * speedRotation;

                            rotation = Quaternion.Euler(xRot, yRot, 0);

                            transform.position = pivot - rotation * (Vector3.forward * distance);

                            transform.rotation = Quaternion.LookRotation(adjustmentRotation * (rotation * Vector3.forward), Vector3.up);
                        }*/
                        //transform.RotateAround(pivot, Vector3.forward, Time.deltaTime * speedRotation);
                        /*
                        if (Input.GetAxisRaw("Mouse Y") >= 0.1)
                        {
                            transform.RotateAround(pivot, Vector3.right, Time.deltaTime * speedRotation);
                        }
                        if (Input.GetAxisRaw("Mouse X") >= 0.1)
                        {
                            transform.RotateAround(pivot, Vector3.up, Time.deltaTime * speedRotation);
                        }*/
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (freeLook == true)
                    {
                        Debug.Log("Soltar");
                        Debug.Log("Last camara es: " + lastCamara.eulerAngles);
                        Debug.Log(this.transform.rotation);
                        this.transform.rotation = lastCamara;
                        Debug.Log(this.transform.rotation);
                        freeLook = false;
                        camera.fieldOfView = originalFOV;
                    }
                }
            }
            else if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            wasRotating = false;
                        }

                        if (touch.phase == TouchPhase.Moved)
                        {

                            transform.Rotate(touch.deltaPosition.y * speedRotation * Time.deltaTime, touch.deltaPosition.x * speedRotation * Time.deltaTime, 0, Space.World);
                            wasRotating = true;
                        }
                    }
                }
            }
        }
    }

    private void goToNewFace()
    {
        cara_anterior = cara;
        if (Input.GetKeyDown("a"))
        {
            rotacionInicial = this.transform.rotation;
            hasToRotate = true;
            if (cara == 0)
            {
                cara = 1;
                direction = 0;
            } else if (cara == 1)
            {
                cara = 2;
                direction = 0;
            }
            else if (cara == 2)
            {
                cara = 3;
                direction = 0;
            }
            else if (cara == 3)
            {
                cara = 0;
                direction = 0;
            }
            else if (cara == 4)
            {
                cara = 1;
                direction = 5;
            } else if (cara == 5)
            {
                cara = 1;
                direction = 4;
            }
        }
        if (Input.GetKeyDown("d"))
        {
            rotacionInicial = this.transform.rotation;
            hasToRotate = true;
            if (cara == 0)
            {
                cara = 3;
                direction = 1;
            }
            else if (cara == 1)
            {
                cara = 0;
                direction = 1;
            }
            else if (cara == 2)
            {
                cara = 1;
                direction = 1;
            }
            else if (cara == 3)
            {
                cara = 2;
                direction = 1;
            }
            else if (cara == 4)
            {
                cara = 3;
                direction = 4;
            }
            else if (cara == 5)
            {
                cara = 3;
                direction = 5;
            }
        }
        if (Input.GetKeyDown("w"))
        {
            rotacionInicial = this.transform.rotation;
            hasToRotate = true;
            if (cara == 0)
            {
                cara = 5;
                direction = 2;
            }
            else if (cara == 1)
            {
                cara = 5;
                direction = 5;
            }
            else if (cara == 2)
            {
                cara = 4;
                direction = 2;
            }
            else if (cara == 3)
            {
                cara = 5;
                direction = 4;
            }
            else if (cara == 4)
            {
                cara = 0;
                direction = 2;
            }
            else if (cara == 5)
            {
                cara = 2;
                direction = 2;
            }
        }
        if (Input.GetKeyDown("s"))
        {
            rotacionInicial = this.transform.rotation;
            hasToRotate = true;
            if (cara == 0)
            {
                cara = 4;
                direction = 3;
            }
            else if (cara == 1)
            {
                cara = 4;
                direction = 4;
            }
            else if (cara == 2)
            {
                cara = 5;
                direction = 3;
            }
            else if (cara == 3)
            {
                cara = 4;
                direction = 5;
            }
            else if (cara == 4)
            {
                cara = 2;
                direction = 3;
            }
            else if (cara == 5)
            {
                cara = 0;
                direction = 3;
            }
        }

    }
}

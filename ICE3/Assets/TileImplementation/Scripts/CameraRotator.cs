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
    Transform camera;
    bool wasRotating;
    

    // Start is called before the first frame update
    void Start()
    {
        camera = this.transform.GetChild(0);
        camera.LookAt(pivot);

        wasRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetMouseButton(0))
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speedRotation);
                float z = transform.eulerAngles.z;
                transform.Rotate(0, 0, -z);
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

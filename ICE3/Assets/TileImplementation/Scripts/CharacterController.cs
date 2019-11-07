using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private float velocity = 5f;

    private int lastMovement = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(1, 1.03f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastMovement == 0)
        {
            if (Input.GetKeyDown("a"))
            {
                lastMovement = 1;
            }
            else if (Input.GetKeyDown("s"))
            {
                lastMovement = 2;
            }
            else if (Input.GetKeyDown("d"))
            {
                lastMovement = 3;
            }
            else if (Input.GetKeyDown("w"))
            {
                lastMovement = 4;
            }
        }
        if (lastMovement == 1)
        {
            this.transform.position = new Vector3(transform.position.x + 1 * velocity *Time.deltaTime, transform.position.y, transform.position.z);
        } else if (lastMovement == 2)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1 * velocity * Time.deltaTime);
        }
        else if (lastMovement == 3)
        {
            this.transform.position = new Vector3(transform.position.x - 1 * velocity * Time.deltaTime, transform.position.y, transform.position.z);
        } else if (lastMovement == 4)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1 * velocity * Time.deltaTime);
        }
    }

    public void w()
    {
        if (lastMovement == 0)
        {
            lastMovement = 4;
        }
    }

    public void a()
    {
        if (lastMovement == 0)
        {
            lastMovement = 1;
        }
    }

    public void s()
    {
        if (lastMovement == 0)
        {
            lastMovement = 2;
        }
    }

    public void d()
    {
        if (lastMovement == 0)
        {
            lastMovement = 3;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Rock"){
            lastMovement = 0;
            this.transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z - 1 * velocity * Time.deltaTime));
        }
    }
}

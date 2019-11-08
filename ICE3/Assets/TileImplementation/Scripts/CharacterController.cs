using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class CharacterController : NetworkBehaviour
{

    private float velocity = 5f;

    private int lastMovement = 0;

    public int Cara;
    public int indexX;
    public int indexY;

    public Vector3 target;
    public bool mooving = false;

     float increment = 10f;

    Cube cubo;

    // Start is called before the first frame update
    void Start()
    {
        cubo = FindObjectOfType<Cube>();
        indexX = 2;
        indexY = 10;
        Cara = 0;
        this.transform.position = cubo.faces[Cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
        this.transform.position = this.transform.position + new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float incrementAux = increment * Time.deltaTime;
        if (!isLocalPlayer)
        {
            return;
        }
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
            if(indexY> 0)
            {
                if (mooving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z - incrementAux);
                    if ( Mathf.Abs(this.transform.position.z - target.z) < 0.5f)
                    {
                        
                        this.transform.position = target;
                        Debug.Log("Acaba Casilla");
                        mooving = false;
                    }
                } else
                {
                    indexY--;
                    TileScript tile = cubo.faces[Cara].tiles[indexX, indexY].GetComponent<TileScript>();
                    if (tile.tileType == TileScript.type.ICE)
                    {
                        Debug.Log("Siguien casilla hielo");
                        target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                        Debug.Log(target);
                        mooving = true;

                    }
                    else
                    {
                        Debug.Log("Siguien casilla Roca");
                        mooving = false;
                        lastMovement = 0;
                    }
                }
               
            }
        }/* else if (lastMovement == 2)
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
        */
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
    {/*
        if (collision.collider.tag == "Rock"){
            lastMovement = 0;
            this.transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z - 1 * velocity * Time.deltaTime));
        }
        */
    }
}

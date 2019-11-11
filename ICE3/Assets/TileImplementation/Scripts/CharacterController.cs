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
        indexX = 3;
        indexY = 3;
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
        
        if (lastMovement == 1) // izq
        {


            if (indexX > 0)
            {
                if (mooving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.5f)
                    {

                        this.transform.position = target;
                        Debug.Log("Acaba Casilla Izq");
                        mooving = false;
                    }
                }
                else
                {
                    
                    TileScript tile = cubo.faces[Cara].tiles[indexX-1, indexY].GetComponent<TileScript>();
                    if (tile.tileType == TileScript.type.ICE)
                    {
                        Debug.Log("Siguien casilla hielo Izq");
                        target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                        Debug.Log(target);
                        mooving = true;
                        indexX--;

                    }
                    else
                    {
                        Debug.Log("Siguien casilla Roca Izq");
                        mooving = false;
                        lastMovement = 0;
                    }
                }

            }



        } else if (lastMovement == 2) // abajo
        {


            if (indexY > 0)
            {
                if (mooving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.5f)
                    {

                        this.transform.position = target;
                        Debug.Log("Acaba Casilla Aba");
                        mooving = false;
                    }
                }
                else
                {
                    
                    TileScript tile = cubo.faces[Cara].tiles[indexX, indexY-1].GetComponent<TileScript>();
                    if (tile.tileType == TileScript.type.ICE)
                    {
                        Debug.Log("Siguien casilla hielo Aba");
                        target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                        Debug.Log(target);
                        mooving = true;
                        indexY--;

                    }
                    else
                    {
                        Debug.Log("Siguien casilla Roca Aba");
                        mooving = false;
                        lastMovement = 0;
                    }
                }

            }
        }
        else if (lastMovement == 3)// derecha
        {

            if (indexX < cubo.width -1)
            {
                if (mooving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.5f)
                    {

                        this.transform.position = target;
                        Debug.Log("Acaba Casilla Izq");
                        mooving = false;
                    }
                }
                else
                {

                    TileScript tile = cubo.faces[Cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                    if (tile.tileType == TileScript.type.ICE)
                    {
                        Debug.Log("Siguien casilla hielo Izq");
                        target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                        Debug.Log(target);
                        mooving = true;
                        indexX++;

                    }
                    else
                    {
                        Debug.Log("Siguien casilla Roca Izq");
                        mooving = false;
                        lastMovement = 0;
                    }
                }

            }

        }
        else if (lastMovement == 4) // arriba
        {
            if (indexY <cubo.heigth -1)
            {
                if (mooving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.5f)
                    {

                        this.transform.position = target;
                        Debug.Log("Acaba Casilla Aba");
                        mooving = false;
                    }
                }
                else
                {

                    TileScript tile = cubo.faces[Cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                    if (tile.tileType == TileScript.type.ICE)
                    {
                        Debug.Log("Siguien casilla hielo Aba");
                        target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                        Debug.Log(target);
                        mooving = true;
                        indexY++;

                    }
                    else
                    {
                        Debug.Log("Siguien casilla Roca Aba");
                        mooving = false;
                        lastMovement = 0;
                    }
                }

            }
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
    {/*
        if (collision.collider.tag == "Rock"){
            lastMovement = 0;
            this.transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z - 1 * velocity * Time.deltaTime));
        }
        */
    }
}

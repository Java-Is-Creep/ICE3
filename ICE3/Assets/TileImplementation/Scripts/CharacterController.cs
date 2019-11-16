using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterController : MonoBehaviourPunCallbacks
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

    moverCamaraFija camaraScript;

    bool hecho = false;

    // Start is called before the first frame update
    void Start()
    {
        camaraScript = FindObjectOfType<moverCamaraFija>();

    }

    // Update is called once per frame
    void Update()
    {

      

        // no dejamos controlar las cosas si no somos el cliente;
        if (!photonView.IsMine)
        {
            return;
        }

        if (!hecho)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Server");
                indexX = 3;
                indexY = 3;
                Cara = 0;
            }
            else
            {
                Debug.Log("Cliente");
                indexX = 5;
                indexY = 5;
                Cara = 0;
            }

            cubo = FindObjectOfType<Cube>();



            this.transform.position = cubo.faces[Cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
            this.transform.position = this.transform.position + new Vector3(0, 1, 0);
            hecho = true;
        }

        float incrementAux = increment * Time.deltaTime;

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

        switch (Cara)
        {
            case 0:
                MovimientoCaraTop(incrementAux);
                break;
            case 1:
                MovimientoCaraRigth(incrementAux);
                break;
        }

    }


    public void MovimientoCaraTop(float incrementAux)
    {

        if (lastMovement == 1) // izq
        {


            if (indexX >= 0)
            {
                if (mooving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.5f)
                    {

                        this.transform.position = target;
                        Debug.Log("Acaba Casilla Izq 1");
                        mooving = false;
                    }
                }
                else
                {

                    if (indexX > 0)
                    {
                        TileScript tile = cubo.faces[Cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {
                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                 Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                                mooving = true;
                                indexX--;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                mooving = false;
                                lastMovement = 0;
                            }


                        }
                        else
                        {
                            Debug.Log("suelo Piedra");
                            mooving = false;
                            lastMovement = 0;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cambio de cara");
                        mooving = false;
                        lastMovement = 0;
                    }


                }

            }



        }
        else if (lastMovement == 2) // abajo
        {


            if (indexY >= 0)
            {
                if (mooving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.5f)
                    {

                        this.transform.position = target;
                        Debug.Log("Acaba Casilla Aba 1");
                        mooving = false;
                    }
                }
                else
                {
                    if (indexY > 0)
                    {
                        TileScript tile = cubo.faces[Cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                                mooving = true;
                                indexY--;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                mooving = false;
                                lastMovement = 0;
                            }

                        }
                        else
                        {
                            Debug.Log("Suelo Piedra");
                            mooving = false;
                            lastMovement = 0;
                        }
                    }
                    else
                    {


                        Debug.LogWarning("Cambio de cara");
                        mooving = false;
                        lastMovement = 0;
                    }


                }

            }
        }
        else if (lastMovement == 3)// derecha
        {

            if (indexX < cubo.width)
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

                    if (indexX < cubo.width - 1)
                    {

                        TileScript tile = cubo.faces[Cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                //Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);

                                mooving = true;
                                indexX++;
                            }
                            else
                            {
                                //Debug.Log("Hay Roca");
                                mooving = false;
                                lastMovement = 0;
                            }

                        }
                        else
                        {
                            //Debug.Log("Suelo Piedra");
                            mooving = false;
                            lastMovement = 0;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cambio de cara");
                        mooving = false;
                        lastMovement = 0;
                    }
                }

            }

        }
        else if (lastMovement == 4) // arriba
        {
            if (indexY < cubo.heigth)
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
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile = cubo.faces[Cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);

                                mooving = true;
                                indexY++;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                mooving = false;
                                lastMovement = 0;
                            }



                        }
                        else
                        {
                             Debug.Log("Siguien casilla suelo piedra");
                            mooving = false;
                            lastMovement = 0;
                        }
                    }
                    else
                    {

                        
                        Debug.Log("Me salgo por la cara topa la cara de arriba");
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(Vector3.zero, Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1, 0), Space.World);
                        Cara = 1;
                        indexY = 0;
                                
                        
                        Debug.LogWarning("Cambio de cara");
                        mooving = false;
                        lastMovement = 0;
                    }

                }

            }
        }
    }
    public void MovimientoCaraRigth(float incrementAux)
    {
        if (lastMovement == 3) // izq
        {


            if (indexX >= 0)
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

                    if (indexX > 0)
                    {
                        TileScript tile = cubo.faces[Cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {
                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                 Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                                mooving = true;
                                indexX--;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                mooving = false;
                                lastMovement = 0;
                            }


                        }
                        else
                        {
                            Debug.Log("suelo Piedra");
                            mooving = false;
                            lastMovement = 0;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Cambio de cara");
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

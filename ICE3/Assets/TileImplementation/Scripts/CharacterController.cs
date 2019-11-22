﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterController : MonoBehaviourPunCallbacks
{

    private float velocity = 5f;

    private int lastMovement = 0;

    public int cara;
    public int indexX;
    public int indexY;

    public Vector3 target;
    public bool moving = false;

    float increment = 5f;

    Cube cubo;

    moverCamaraFija camaraScript;

    bool hecho = false;
    bool hayCambioCara;
    public bool isFiring;
    public float timeBetweenShots;
    public float timeWaitingShots = 0;

    public GameObject bolaDeNieve;


    bool ab = false;
    bool wb = false;
    bool sb = false;
    bool db = false;

    GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        model = this.transform.GetChild(0).gameObject;
        Debug.Log(model);
        camaraScript = FindObjectOfType<moverCamaraFija>();

    }

    // Update is called once per frame
    void Update()
    {


        if (!photonView.IsMine)
        {
            return;
        }
        //actualizacion de variables
        timeWaitingShots += Time.deltaTime;
        isFiring = false;


        if (!hecho)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Server");
                indexX = 3;
                indexY = 3;
                cara = 0;
            }
            else
            {
                Debug.Log("Cliente");
                indexX = 3;
                indexY = 4;
                cara = 0;
            }

            cubo = FindObjectOfType<Cube>();

            hayCambioCara = false;
            CubeFace face = cubo.faces[cara];
            GameObject au = face.tiles[indexX, indexY];
            TileScript ts = au.GetComponent<TileScript>();
            this.transform.position = ts.AbsolutePos;

            this.transform.position = this.transform.position + new Vector3(0, 1, 0);
            hecho = true;
        }

        float incrementAux = increment * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (timeBetweenShots < timeWaitingShots)
            {
                Debug.Log("Disparando");
                this.photonView.RPC("Shot", RpcTarget.All);
                timeWaitingShots = 0;
            }
        }



        if (Input.GetKeyDown("a") || ab)
        {
            if (cara == 0 || cara == 2 )
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (lastMovement == 0)
            {
                lastMovement = 1;
                ab = false;
            }
        }
        else if (Input.GetKeyDown("s") || sb)
        {
            if (cara == 0 || cara == 2)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            if (lastMovement == 0)
            {
                lastMovement = 2;
                sb = false;
            }
        }
        else if (Input.GetKeyDown("d") || db)
        {

            if (cara == 0 || cara == 2)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (lastMovement == 0)
            {
                lastMovement = 3;
                db = false;
            }
        }
        else if (Input.GetKeyDown("w") || wb)
        {

            if (cara == 0 || cara == 2)
            {
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (cara == 3)
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cara == 4)
            {
                model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (cara == 1)
            {
                model.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            if (lastMovement == 0)
            {
                lastMovement = 4;
                wb = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (timeBetweenShots < timeWaitingShots)
            {
                Debug.Log("Di`parando");
                this.photonView.RPC("Shot", RpcTarget.All, this.photonView.GetInstanceID());
                timeWaitingShots = 0;
            }
        }



        switch (cara)
        {
            case 0:
                //Debug.Log("Indice cara top: " + indexX + ", " + indexY);
                MovimientoCaraTop(incrementAux);
                break;
            case 3:
                //Debug.Log("Indice cara right: " + indexX + ", " + indexY);
                MovimientoCaraRigth(incrementAux);
                break;
            case 2:
                //Debug.Log("Indice cara front: " + indexX + ", " + indexY);
                MovimientoCaraFront(incrementAux);
                break;
            case 4:
                Debug.Log("Indice cara left: " + indexX + ", " + indexY);
                MovimientoCaraLeft(incrementAux);
                break;
            case 1:
                Debug.Log("Indice cara back: " + indexX + ", " + indexY);
                MovimientoCaraBack(incrementAux);
                break;

        }

    }


    #region Funciones Remotas

    ///
    [PunRPC]
    void Shot()
    {
        Debug.Log("Han disparado");
        /* if(targetID == photonView.GetInstanceID())
         {
             GameObject aux = Instantiate(bolaDeNieve, this.transform.position + (Vector3.forward * 0.2f), Quaternion.identity);
             aux.GetComponent<Proyectil>().initDireccion(this.gameObject.transform.TransformDirection(Vector3.forward), this.gameObject);
         }*/
        Transform childTransform = this.gameObject.transform.GetChild(0);
        Debug.Log(childTransform);
        GameObject aux = Instantiate(bolaDeNieve, this.transform.position + (childTransform.TransformDirection(Vector3.back * 0.2f)), Quaternion.identity);
        aux.GetComponent<Proyectil>().initDireccion(childTransform.TransformDirection(Vector3.back), this.gameObject,cubo.heigth);

    }




    #endregion

    public void MovimientoCaraTop(float incrementAux)
    {

        //Arriba
        if (lastMovement == 4) // arriba
        {
            //Debug.Log("Arriba");
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.1f)
                    {

                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;

                        if (hayCambioCara)
                        {
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3(-0.5f, cubo.width - 1.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 1;
                            moving = false;
                            //del 0,1 al 1,7
                            lastMovement = 2;
                            //indexX = indexY;
                            indexX = 7;
                            hayCambioCara = false;

                            for (int i = 0; i < cubo.width; i++)
                            {
                                for (int j = 0; j < cubo.width; j++)
                                {
                                    Debug.Log(i + " " + j + " " + cubo.faces[cara].tiles[i, j].GetComponent<TileScript>().myObjectType);
                                }
                            }
                        }
                    }
                }
                else
                {

                    if (indexX > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x - iteracion, this.transform.position.y, this.transform.position.z);


                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        //moving = false;
                        //lastMovement = 0;
                        camaraScript.back();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                        this.gameObject.transform.Translate(new Vector3(-0.5f, cubo.width - 1.5f, 0), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 1;
                        moving = false;
                        //del 0,1 al 1,7
                        lastMovement = 2;
                        //indexX = indexY;
                        indexX = 7;
                        hayCambioCara = false;
                    }


                }

            }



        }

        //Izquierda
        else if (lastMovement == 1) // izqda
        {
            //Debug.Log("Izqda");

            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {

                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;

                        if (hayCambioCara)
                        {
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, -0.5f), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 4;
                            moving = false;
                            lastMovement = 2;
                            indexY = 7;
                            hayCambioCara = false;
                            /*
                            for (int i = 0; i < cubo.width; i++)
                            {
                                for (int j = 0; j < cubo.width; j++)
                                {
                                    Debug.Log(cubo.faces[cara].tiles[i, j].GetComponent<TileScript>().myObjectType);
                                }
                            }*/
                        }

                    }
                }
                else
                {
                    if (indexY > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - iteracion);


                    }
                    else
                    {
                        // Debug.LogWarning("Cambio de cara");
                        //moving = false;
                        //lastMovement = 0;
                        camaraScript.left();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                        this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, -0.5f), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 4;
                        moving = false;
                        lastMovement = 2;
                        indexY = 7;
                        hayCambioCara = false;
                    }


                }

            }
        }

        //Abajo
        else if (lastMovement == 2)
        {
            //Debug.Log("abajo");
            if (indexX < cubo.width)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.1f)
                    {

                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(0.5f, cubo.width - 1.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 2;
                            moving = false;
                            //lastMovement = 0;
                            indexX = 0;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {

                    if (indexX < cubo.width - 1)
                    {
                        int iteracion = 0;
                        TileScript tile;

                        do
                        {
                            // Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);


                    }
                    else
                    {
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                        this.gameObject.transform.Translate(new Vector3(0.5f, cubo.width - 1.5f, 0), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 2;
                        moving = false;
                        //lastMovement = 0;
                        indexX = 0;
                        hayCambioCara = false;
                        //Debug.LogWarning("Cambio de cara");

                    }
                }

            }

        }

        //Dcha
        else if (lastMovement == 3)
        {

            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {

                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, 0.5f), Space.World);
                            cara = 3;
                            //indexX = 1;
                            indexY = 0;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 2;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + iteracion);

                    }
                    else
                    {
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1.5f, 0.5f), Space.World);
                        cara = 3;
                        //indexX = 1;
                        indexY = 0;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;


                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 2;
                        hayCambioCara = false;

                    }

                }

            }
        }
    }

    public void MovimientoCaraRigth(float incrementAux)
    {
        // izq
        if (lastMovement == 1)
        {
            //Debug.Log("Izq");
            if (indexX <= cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, cubo.width - 1f), Space.World);
                            cara = 2;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, cubo.width - 1f), Space.World);
                        cara = 2;
                        //indexX = 1;
                        int aux = indexX;
                        indexX = indexY;
                        indexY = aux;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;


                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }


                }

            }

        }

        // derecha
        else if (lastMovement == 3)
        {
            //Debug.Log("Dcha");
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        // Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, (cubo.width - 1f)), Space.World);
                            cara = 1;
                            //indexX = 1;
                            indexX = ((int)(cubo.width - 1)) - indexY;
                            indexY = 7;
                            

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    // Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x - iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }
                    //Debug.Log("Hola");

                }

            }

        }

        //Abajo
        else if (lastMovement == 2)
        {
            //Debug.Log("Abajo");
            //Debug.Log("Estoy en " + indexY);
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                    }
                }
                else
                {

                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.Log(indexY);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }
                }

            }

        }

        //Arriba
        else if (lastMovement == 4)
        {
            //Debug.Log("Arriba");
            //Debug.Log("Estoy en " + indexY);
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;

                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, cubo.width - 1.5f), Space.World);
                            cara = 0;
                            //indexX = 1;
                            indexY = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {

                    if (indexY > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.Log(indexY);
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                        this.gameObject.transform.Translate(new Vector3(0, -0.5f, cubo.width - 1.5f), Space.World);
                        cara = 0;
                        indexY = 7;


                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }
                }

            }

        }

    }

    public void MovimientoCaraFront(float incrementAux)
    {

        //Dcha
        if (lastMovement == 3)
        {
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY+1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + iteracion);

                    }
                    else
                    {
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                        cara = 3;
                        //indexX = 1;
                        int aux = indexX;
                        indexX = indexY;
                        indexY = aux;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;


                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                        //Debug.LogWarning("Cambio de cara");
                    }

                }

            }
        }

        //Izquierda
        if (lastMovement == 1)
        {
            //Debug.Log("SALE");
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 1f), 0, 0), Space.World);
                            cara = 4;
                            //indexX = 1;
                            indexY = ((int)(cubo.width - 1)) - indexX;
                            indexX = 7;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - iteracion);

                    }
                    else
                    {

                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }

                }

            }
        }

        //Abajo
        if (lastMovement == 2)
        {

            if (indexX < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            // Debug.Log("Leyendo casilla: " + (indexX + 1) + ", " + (indexY ));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                // Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {

                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }

                }

            }
        }

        //Arriba
        if (lastMovement == 4)
        {

            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 1.5f), -0.5f, 0), Space.World);
                            cara = 0;
                            //indexX = 1;
                            indexX = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 4;
                            hayCambioCara = false;
                        }

                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            // Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, 90);
                        this.gameObject.transform.Translate(new Vector3((cubo.width - 1.5f), -0.5f, 0), Space.World);
                        cara = 0;
                        indexX = 7;
                        moving = false;
                        lastMovement = 4;
                        hayCambioCara = false;
                    }

                }

            }
        }
    }

    public void MovimientoCaraLeft(float incrementAux)
    {

        //izqda
        if (lastMovement == 1)
        {
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        // Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {
                            camaraScript.back();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                            cara = 1;
                            //indexX = 1;
                            indexX = /*((int)(cubo.width - 1)) -*/ indexY;
                            indexY = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    // Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x - iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.back();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                        cara = 1;
                        //indexX = 1;
                        indexX = /*((int)(cubo.width - 1)) -*/ indexY;
                        indexY = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                        model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }
                    //Debug.Log("Hola");

                }

            }
        }

        //Abajo
        else if (lastMovement == 2)
        {
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;

                        if (hayCambioCara)
                        {
                            /*
                            camaraScript.button();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, cubo.width - 1.5f), Space.World);
                            cara = 0;
                            //indexX = 1;
                            indexY = 7;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;


                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;*/
                        }
                    }
                }
                else
                {

                    if (indexY > 0)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");

                                //hayCambioCara = true;

                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        // Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.Log(indexY);
                        //Debug.LogWarning("Cambio de cara");
                        /*
                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, -90);
                        this.gameObject.transform.Translate(new Vector3(0, -0.5f, cubo.width - 1.5f), Space.World);
                        cara = 0;
                        indexY = 7;

                    */
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                        hayCambioCara = false;
                    }
                }

            }
        }

        //derecha
        else if (lastMovement == 3)
        {
            if (indexX <= cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                        if (hayCambioCara)
                        {

                            camaraScript.front();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                            cara = 2;
                            //indexX = 1;
                            indexX = ((int)(cubo.width - 1)) - indexY;
                            indexY = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }
                    }
                    else
                    {
                        //Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);

                    }
                    else
                    {
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, -(cubo.width - 1f)), Space.World);
                        cara = 2;
                        //indexX = 1;
                        indexX = ((int)(cubo.width - 1)) - indexY;
                        indexY = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                        model.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                        /*
                        //Debug.LogWarning("Cambio de cara");
                        camaraScript.front();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(0, 0, cubo.width - 1f), Space.World);
                        cara = 2;
                        //indexX = 1;
                        int aux = indexX;
                        indexX = indexY;
                        indexY = aux;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                    
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                        hayCambioCara = false;*/
                    }


                }

            }
        }

        //arriba
        else if (lastMovement == 4)
        {
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    //Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                            this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 1.5f)), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 0;
                            moving = false;
                            lastMovement = 3;
                            indexY = 0;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {

                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;
                        int iteracion = 0;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.heigth)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            //Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        //Debug.Log(indexY);
                        //Debug.LogWarning("Cambio de cara");
                        //moving = false;
                        //lastMovement = 0;

                        camaraScript.top();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, -0.5f, -(cubo.width - 1.5f)), Space.World);
                        //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                        cara = 0;
                        moving = false;
                        lastMovement = 3;
                        indexY = 0;
                        hayCambioCara = false;
                    }
                }

            }
        }
    }

    public void MovimientoCaraBack(float incrementAux)
    {
        //izquierda
        if (lastMovement == 1)
        {
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            indexY = ((int)(cubo.width - 1)) - indexX;
                            indexX = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    // Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                // Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + iteracion);

                    }
                    else
                    {
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                        this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                        cara = 3;
                        //indexX = 1;
                        indexY = ((int)(cubo.width - 1)) - indexX;
                        indexX = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                        model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 1;
                        hayCambioCara = false;
                    }

                }

            }
        }

        //abajo
        else if (lastMovement == 2)
        {

            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y - incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            /*
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, 90);
                            this.gameObject.transform.Translate(new Vector3((cubo.width - 1f), 0, 0), Space.World);
                            cara = 4;
                            //indexX = 1;
                            indexY = ((int)(cubo.width - 1)) - indexX;
                            indexX = 7;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 1;
                            hayCambioCara = false;*/
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                //hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y - iteracion, this.transform.position.z);

                    }
                    else
                    {

                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }

                }

            }
        }

        //derecha
        else if (lastMovement == 3)
        {
            //Debug.Log("SALE");
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.left();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                            cara = 4;
                            //indexX = 1;
                            indexY = /*((int)(cubo.width - 1)) -*/ indexX;
                            indexX = 0;
                            //Vennimos del 7,4
                            //Hay que ir al 4,0

                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;

                            this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexY > 0)
                    {
                        TileScript tile;

                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexY - 1 < 0)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - iteracion);

                    }
                    else
                    {
                        camaraScript.left();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1f), 0, 0), Space.World);
                        cara = 4;
                        //indexX = 1;
                        indexY = /*((int)(cubo.width - 1)) -*/ indexX;
                        indexX = 0;
                        //Vennimos del 7,4
                        //Hay que ir al 4,0

                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;

                        this.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        model.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                    }

                }

            }
        }

        //arriba
        else if (lastMovement == 4)
        {
            if (indexX < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.1f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                        if (hayCambioCara)
                        {
                            camaraScript.top();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.forward, -90);
                            this.gameObject.transform.Translate(new Vector3(-(cubo.width - 1.5f), -0.5f, 0), Space.World);
                            //this.gameObject.transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
                            cara = 0;
                            moving = false;
                            //del 0,1 al 1,7
                            lastMovement = 2;
                            //indexX = indexY;
                            indexX = 0;
                            hayCambioCara = false;
                            /*
                            camaraScript.right();
                            this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                            this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                            cara = 3;
                            //indexX = 1;
                            int aux = indexX;
                            indexX = indexY;
                            indexY = aux;
                            //indexX = ((int)cubo.width) - 1 - indexX;       
                            //indexX = 0;
                            this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                            model.transform.localRotation = Quaternion.Euler(0, 90, 0);
                            //Debug.LogWarning("Cambio de cara");
                            moving = false;
                            lastMovement = 3;
                            hayCambioCara = false;*/
                        }
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth - 1)
                    {
                        TileScript tile;
                        do
                        {
                            //Debug.Log("Iteracion");
                            if (indexX + 1 >= cubo.width)
                            {
                                //Debug.Log("Es la ultima casilla");
                                hayCambioCara = true;
                                break;
                            }
                            tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX + 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    //Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    //Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        //Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                        lastMovement = 0;
                                    }
                                    else
                                    {
                                        moving = true;
                                    }
                                    //
                                    break;
                                }
                            }
                            else
                            {
                                //Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                }
                                else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }

                        } while (true);
                        target = new Vector3(this.transform.position.x, this.transform.position.y + iteracion, this.transform.position.z);

                    }
                    else
                    {
                        /*
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(new Vector3(3.5f, -3.5f, 3.5f), Vector3.up, -90);
                        this.gameObject.transform.Translate(new Vector3(cubo.width - 1f, 0, 0), Space.World);
                        cara = 3;
                        //indexX = 1;
                        int aux = indexX;
                        indexX = indexY;
                        indexY = aux;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;


                        //Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 3;
                        hayCambioCara = false;
                        //Debug.LogWarning("Cambio de cara");*/
                        lastMovement = 0;
                        moving = false;
                    }

                }

            }
        }
    }

    #region botones para movil
    public void w()
    {
        if (lastMovement == 0)
        {
            wb = true;
        }
    }

    public void a()
    {
        if (lastMovement == 0)
        {
            ab = true;
        }
    }

    public void s()
    {
        if (lastMovement == 0)
        {
            sb = true;
        }
    }

    public void d()
    {
        if (lastMovement == 0)
        {
            db = true;
        }
    }
    #endregion


    private void OnCollisionEnter(Collision collision)
    {/*
        if (collision.collider.tag == "Rock"){
            lastMovement = 0;
            this.transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z - 1 * velocity * Time.deltaTime));
        }
        */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class CharacterController : NetworkBehaviour
{

    private float velocity = 5f;

    private int lastMovement = 0;

    public int cara;
    public int indexX;
    public int indexY;

    public Vector3 target;
    public bool moving = false;

     float increment = 10f;

    Cube cubo;

    moverCamaraFija camaraScript;

    // Start is called before the first frame update
    void Start()
    {
        camaraScript = FindObjectOfType<moverCamaraFija>();
        cubo = FindObjectOfType<Cube>();
        indexX = 3;
        indexY = 3;
        cara = 0;
        this.transform.position = cubo.faces[cara].tiles[indexX, indexY].GetComponent<TileScript>().AbsolutePos;
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

        switch (cara)
        {
            case 0:
                Debug.Log("Indice cara top: " + indexX + ", " + indexY);
                MovimientoCaraTop(incrementAux);
                break;
            case 3:
                Debug.Log("Indice cara right: " + indexX + ", " + indexY);
                MovimientoCaraRigth(incrementAux);
                break;
        }

    }


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
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.5f)
                    {

                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                    }
                }
                else
                {

                    if (indexX > 0)
                    {
                        TileScript tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {
                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                // Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                                moving = true;
                                indexX--;
                            }
                            else
                            {
                                //Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }


                        }
                        else
                        {
                            //Debug.Log("suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }
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
        
        //Izquierda
        else if (lastMovement == 1) // izqda
        {
            //Debug.Log("Izqda");

            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.5f)
                    {

                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                    }
                }
                else
                {
                    if (indexY > 0)
                    {
                        TileScript tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                //Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);
                                moving = true;
                                indexY--;
                            }
                            else
                            {
                                //Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }

                        }
                        else
                        {
                            //Debug.Log("Suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }
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
        else if (lastMovement == 2)
        {
            //Debug.Log("abajo");
            if (indexX < cubo.width)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) < 0.5f)
                    {

                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
                    }
                }
                else
                {

                    if (indexX < cubo.width - 1)
                    {

                        TileScript tile = cubo.faces[cara].tiles[indexX + 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                //Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);

                                moving = true;
                                indexX++;
                            }
                            else
                            {
                                //Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }

                        }
                        else
                        {
                            //Debug.Log("Suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }
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
        
        //Dcha
        else if (lastMovement == 3) 
        {
            
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementAux);
                    if (Mathf.Abs(this.transform.position.z - target.z) < 0.5f)
                    {

                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Aba");
                        moving = false;
                    }
                }
                else
                {
                    if (indexY < cubo.heigth - 1)
                    {
                        TileScript tile = cubo.faces[cara].tiles[indexX, indexY + 1].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                //Debug.Log("Siguien casilla sin obstaculos");
                                target = new Vector3(tile.AbsolutePos.x, transform.position.y, tile.AbsolutePos.z);

                                moving = true;
                                indexY++;
                            }
                            else
                            {
                                //Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }



                        }
                        else
                        {
                            // Debug.Log("Siguien casilla suelo piedra");
                            moving = false;
                            lastMovement = 0;
                        }
                    }
                    else
                    {

                        
                        Debug.Log("Me salgo por la cara topa la cara de arriba");
                        camaraScript.right();
                        this.gameObject.transform.RotateAround(Vector3.zero, Vector3.right, 90);
                        this.gameObject.transform.Translate(new Vector3(0, cubo.width - 1, 0), Space.World);
                        cara = 3;
                        indexX = 1;
                        indexY = 0;
                        //indexX = ((int)cubo.width) - 1 - indexX;       
                        //indexX = 0;
                        
                        
                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }

                }

            }
        }  
    }

    public void MovimientoCaraRigth(float incrementAux)
    {
        if (lastMovement == 1) // izq
        {
            Debug.Log("Izq");
            if (indexX <= cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.5f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                    }
                    else
                    {
                        Debug.Log("No llegamos a la casilla");
                    }
                }
                else
                {
                    int iteracion = 0;
                    if (indexX < cubo.heigth-1)
                    {
                        TileScript tile;

                        do
                        {
                            Debug.Log("Iteracion");
                            tile = cubo.faces[cara].tiles[indexX+1 , indexY].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX+1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX++;
                                    iteracion++;
                                }
                                else
                                {
                                    Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        Debug.Log("iteracion menor o igual que 0");
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
                                Debug.Log("suelo Piedra");
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
                        /*
                        TileScript tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {
                            
                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                Debug.Log("Siguien casilla sin obstaculos");
                                //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                target = new Vector3(this.transform.position.x +1 , this.transform.position.y, this.transform.position.z);
                                moving = true;
                                indexX--;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }
                        }
                        else
                        {
                            Debug.Log("suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }*/
                    }
                    else
                    {
                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }
                    Debug.Log("Hola");

                }

            }
            /*
            Debug.Log("Izq");
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.5f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                    } else
                    {
                        Debug.Log("No llegamos a la casilla");
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
                            Debug.Log("Iteracion");
                            tile = cubo.faces[cara].tiles[indexX -1, indexY].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + indexY);
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                    } else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                } else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }
                            
                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);
                        /*
                        TileScript tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {
                            
                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                Debug.Log("Siguien casilla sin obstaculos");
                                //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                target = new Vector3(this.transform.position.x +1 , this.transform.position.y, this.transform.position.z);
                                moving = true;
                                indexX--;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }
                        }
                        else
                        {
                            Debug.Log("suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }*/
            /*
        }
        else
        {
            Debug.LogWarning("Cambio de cara");
            moving = false;
            lastMovement = 0;
        }
        Debug.Log("Hola");

    }

}*/



        }

        if (lastMovement == 3) // derecha
        {
            Debug.Log("Izq");
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    Debug.Log("Moving");
                    Debug.Log("Pos: " + this.transform.position);
                    Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x - incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.5f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                    }
                    else
                    {
                        Debug.Log("No llegamos a la casilla");
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
                            Debug.Log("Iteracion");
                            tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + (indexY));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        Debug.Log("iteracion menor o igual que 0");
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
                                Debug.Log("suelo Piedra");
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
                        /*
                        TileScript tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {
                            
                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                Debug.Log("Siguien casilla sin obstaculos");
                                //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                target = new Vector3(this.transform.position.x +1 , this.transform.position.y, this.transform.position.z);
                                moving = true;
                                indexX--;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }
                        }
                        else
                        {
                            Debug.Log("suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }*/
                    }
                    else
                    {
                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }
                    Debug.Log("Hola");

                }

            }
            /*
            Debug.Log("Izq");
            if (indexX >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    Debug.Log("Moving");
                    //Debug.Log("Pos: " + this.transform.position);
                    Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x + incrementAux, transform.position.y, transform.position.z);
                    if (Mathf.Abs(this.transform.position.x - target.x) <= 0.5f)
                    {
                        this.transform.position = target;
                        target = this.transform.position;
                        Debug.Log("Acaba Casilla Izq");
                        moving = false;
                        lastMovement = 0;
                    } else
                    {
                        Debug.Log("No llegamos a la casilla");
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
                            Debug.Log("Iteracion");
                            tile = cubo.faces[cara].tiles[indexX -1, indexY].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX - 1) + ", " + indexY);
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexX--;
                                    iteracion++;
                                }
                                else
                                {
                                    Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        Debug.Log("iteracion menor o igual que 0");
                                        moving = false;
                                    } else
                                    {
                                        moving = true;
                                    }
                                    //lastMovement = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Debug.Log("suelo Piedra");
                                if (iteracion <= 0)
                                {
                                    moving = false;
                                } else
                                {
                                    moving = true;
                                }
                                lastMovement = 0;
                                break;
                            }
                            
                        } while (true);
                        target = new Vector3(this.transform.position.x + iteracion, this.transform.position.y, this.transform.position.z);
                        /*
                        TileScript tile = cubo.faces[cara].tiles[indexX - 1, indexY].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {
                            
                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                Debug.Log("Siguien casilla sin obstaculos");
                                //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                target = new Vector3(this.transform.position.x +1 , this.transform.position.y, this.transform.position.z);
                                moving = true;
                                indexX--;
                            }
                            else
                            {
                                Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }
                        }
                        else
                        {
                            Debug.Log("suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }*/
            /*
        }
        else
        {
            Debug.LogWarning("Cambio de cara");
            moving = false;
            lastMovement = 0;
        }
        Debug.Log("Hola");

    }

}*/



        }

        else if (lastMovement == 2)         //Abajo
        {
            Debug.Log("Abajo");
            //Debug.Log("Estoy en " + indexY);
            if (indexY < cubo.heigth)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x , transform.position.y - incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.5f)
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
                            Debug.Log("Iteracion");
                            tile = cubo.faces[cara].tiles[indexX , indexY + 1].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY + 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY++;
                                    iteracion++;
                                }
                                else
                                {
                                    Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        Debug.Log("iteracion menor o igual que 0");
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
                                Debug.Log("suelo Piedra");
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
                        target = new Vector3(this.transform.position.x , this.transform.position.y - iteracion, this.transform.position.z);

                        /*
                        TileScript tile = cubo.faces[cara].tiles[indexX, indexY +1].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                //Debug.Log("Siguien casilla sin obstaculos");
                                //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                                target = new Vector3(this.transform.position.x , this.transform.position.y -1, this.transform.position.z);
                                moving = true;
                                indexY++;
                            }
                            else
                            {
                                //Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }

                        }
                        else
                        {
                            //Debug.Log("Suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }*/
                    }
                    else
                    {
                        Debug.Log(indexY);
                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
                    }
                }

            }

        }

        else if (lastMovement == 4)         //Arriba
        {
            Debug.Log("Arriba");
            //Debug.Log("Estoy en " + indexY);
            if (indexY >= 0)
            {
                if (moving) //mas eficiente, mirar todas las casillas y ver hasta cualpuedes ir
                {
                    //Debug.Log("Pos: " + this.transform.position);
                    Debug.Log("Pos target: " + target);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y + incrementAux, transform.position.z);
                    if (Mathf.Abs(this.transform.position.y - target.y) < 0.5f)
                    {
                        this.transform.position = target;
                        //Debug.Log("Acaba Casilla Izq");
                        moving = false;
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
                            Debug.Log("Iteracion");
                            tile = cubo.faces[cara].tiles[indexX, indexY - 1].GetComponent<TileScript>();
                            Debug.Log("Leyendo casilla: " + (indexX) + ", " + (indexY - 1));
                            if (tile.tileType == TileScript.type.ICE)
                            {
                                if (tile.myObjectType == TileScript.tileObject.NULL)
                                {
                                    Debug.Log("Siguien casilla sin obstaculos");
                                    //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y,this.transform.position.z);
                                    //target = new Vector3(this.transform.position.x + 1, target.y, target.z);
                                    moving = true;
                                    indexY--;
                                    iteracion++;
                                }
                                else
                                {
                                    Debug.Log("Hay Roca");
                                    if (iteracion <= 0)
                                    {
                                        Debug.Log("iteracion menor o igual que 0");
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
                                Debug.Log("suelo Piedra");
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

                        /*
                        TileScript tile = cubo.faces[cara].tiles[indexX, indexY +1].GetComponent<TileScript>();
                        if (tile.tileType == TileScript.type.ICE)
                        {

                            if (tile.myObjectType == TileScript.tileObject.NULL)
                            {
                                //Debug.Log("Siguien casilla sin obstaculos");
                                //target = new Vector3(tile.AbsolutePos.x, tile.AbsolutePos.y, this.transform.position.z);
                                target = new Vector3(this.transform.position.x , this.transform.position.y -1, this.transform.position.z);
                                moving = true;
                                indexY++;
                            }
                            else
                            {
                                //Debug.Log("Hay Roca");
                                moving = false;
                                lastMovement = 0;
                            }

                        }
                        else
                        {
                            //Debug.Log("Suelo Piedra");
                            moving = false;
                            lastMovement = 0;
                        }*/
                    }
                    else
                    {
                        Debug.Log(indexY);
                        Debug.LogWarning("Cambio de cara");
                        moving = false;
                        lastMovement = 0;
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

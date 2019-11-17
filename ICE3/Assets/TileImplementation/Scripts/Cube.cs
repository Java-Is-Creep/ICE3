using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cube : MonoBehaviourPunCallbacks
{
    //anchura y altua de las caras
    public float width;
    public float heigth;

    // prefabs de objetos de las caras
    public GameObject prefabIce;
    public GameObject prefabRock;
    public GameObject prefabTile;
    public GameObject prefabFace;
    int probRock = 10;

    public CubeFace[] faces = new CubeFace[6];
    public GameObject[] facesObject = new GameObject[6];
    // Start is called before the first frame update
    public TextAsset MapInfo;

    string[] separacion1;

    float tamañoCara = 1;

    string[,] caraTop; // cara 0 a la hora de girar
    string[,] caraArriba;//1
    string[,] caraAbajo;//2
    string[,] caraDerecha;//3
    string[,] caraIzquierda;//4
    string[,] caraBottom;//5


    void Start()
    {
        

        readCSV();


        GameObject[,] cubeFace;
        for (int i = 0; i < 6; i++)
        {
            cubeFace = new GameObject[(int)width, (int)heigth];
            facesObject[i] = Instantiate(prefabFace, GetComponentInParent<Transform>());


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    int random = Random.Range(0, 100);
                    TileScript.type tileType = TileScript.type.NULL;
                    GameObject tileGround = null;
                    TileScript.tileObject tileObjectType = TileScript.tileObject.NULL;
                    GameObject objectInTile = null;

                    string casilla = "none";

                    switch (i){
                        case 0:
                            casilla = caraTop[x, y];
                            break;
                        case 1:
                            casilla = caraArriba[x, y];
                            break;
                        case 2:
                            casilla = caraAbajo[x, y];
                            break;
                        case 3:
                            casilla = caraDerecha[x, y];
                            break;
                        case 4:
                            casilla = caraIzquierda[x, y];
                            break;
                        case 5:
                            casilla = caraBottom[x, y];
                            break;
                    }


                    switch (casilla)
                    {
                        case "SH":
                            tileType = TileScript.type.ICE;
                            tileGround= prefabIce;
                            break;
                        case "SH.P":
                            tileType = TileScript.type.ICE;
                            tileGround = prefabIce;
                            objectInTile = prefabRock;
                            tileObjectType = TileScript.tileObject.ROCK;
                            break;
                        
                    }
                    //TODO LO DE LOS TILES YA NO VALE PARA NADA
                    /*
                    if (y == 0 || x == 0 || x == width - 1 || y == heigth - 1)
                    {
                        tileType = TileScript.type.ROCK;
                        tileTypeObject = prefabRock;
                    }
                    else if (random < probRock)
                    {
                        tileType = TileScript.type.ROCK;
                        tileTypeObject = prefabRock;
                    }
                    else
                    {
                        tileType = TileScript.type.ICE;
                        tileTypeObject = prefabIce;
                    }
                    */

                    GameObject tileObject = Instantiate(prefabTile, facesObject[i].GetComponent < Transform>());
                    tileObject.transform.position = new Vector3(x, 0, y);
                    TileScript tile = tileObject.GetComponent<TileScript>();
                    tile.initTile(x, y, i, x, y, tileGround, tileType);
                    if(objectInTile != null)
                    {
                        tile.hasObject(objectInTile, tileObjectType);
                    }
                    cubeFace[x, y] = tileObject;
                }
            }
            faces[i] = facesObject[i].GetComponent<CubeFace>();
            faces[i].initCube(width, heigth, i, cubeFace,this.gameObject);
                 
        }

        //this.transform.position = new Vector3(-width / 2, heigth / 2, -heigth / 2 +tamañoCara/2);

        updateFaces();
    }

    /// <summary>
    /// Se rotan las caras para ponerlas en su posicion
    /// </summary>
    public void updateFaces()
    {
        for (int i = 0; i < faces.Length; i++)
        {
            faces[i].reloadTilePos();
        }
    }

        // Update is called once per frame
        void Update()
        {

        }

    public void readCSV()
    {
        int tamaño = 0;
        separacion1 = MapInfo.text.Split('\n');
        for ( int i = 0; i < separacion1.Length-1; i++)
        {
            
            string[] linea = separacion1[i].Split(';');
            for(int j = 0; j< linea.Length-1; j++)
            {

                if (i == 0 && j == 0)
                {
                    tamaño = int.Parse(linea[i]);
                    width = tamaño;
                    heigth = tamaño;
                    caraTop = new string[tamaño, tamaño];
                    caraArriba = new string[tamaño, tamaño];
                    caraAbajo = new string[tamaño, tamaño];
                    caraDerecha = new string[tamaño, tamaño];
                    caraIzquierda = new string[tamaño, tamaño];
                    caraBottom = new string[tamaño, tamaño];
                }

                if(i < tamaño)// si estamos en la primera cara
                {
                    if(j >= tamaño && j< tamaño*2) // leemos a partir del tamaño ya que ahi no hay info;
                    {
                        caraArriba[i, j - tamaño] = linea[j];
                    }
                    
                }else if(i >= tamaño && i < tamaño * 2) // casillas centrales del excel
                {
                    if(j < tamaño)
                    {
                        caraIzquierda[i - tamaño,j] = linea[j];
                    }  else if( j >= tamaño && j < tamaño * 2)
                    {
                        caraTop[ i - tamaño, j - tamaño] = linea[j];
                    } else if( j >= tamaño*2 && j< tamaño * 3)
                    {

                        caraDerecha[ i - tamaño, j - tamaño*2] = linea[j];

                    } else if (j>= tamaño*3 && j< tamaño*4)
                    {
                        if(linea[j].CompareTo("null") == 0)
                        {
                            //Debug.Log("i es: " + i + " j es: " + j);
                        }
 
                        caraBottom[ i - tamaño,j - tamaño*3] = linea[j];
                    }

                } else 
                {
                    if (j >= tamaño && j < tamaño * 2) 
                    {
                        //Debug.Log(i + " i es: " + j + " j es:");
                        caraAbajo[i - tamaño * 2, j - tamaño] = linea[j];
                        //Debug.Log("Sin errores");
                    }

                }
            }
        }
    }
    
}

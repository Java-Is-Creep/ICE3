using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFace : MonoBehaviour
{
     float width;
     float heigth;

    public int faceNum; //enumerador que sirve para saber que cara es;

    

    float prefWidth = 1;

    public GameObject[,] tiles;

    GameObject cubo;


    public void initCube (float width, float heigth, int faceNum, GameObject[,] tiles, GameObject cubo)
    {
        this.width = width;
        this.heigth = heigth;
        this.faceNum = faceNum;
        this.tiles = tiles;
        this.cubo = cubo;
        changeOrientation();
    }
    



    // Se crea la cara del cubo correspondiente.
    //Se crean obstaculos aleatorios
    // La cara se recubre de cubos para no caerte.
    void Start()
    {
    }

    public void changeOrientation()
    {
        switch (faceNum)
        {
            case 0: // cara Top
                break;
            case 1: //cara arriba
                this.transform.Translate(-(prefWidth / 2), -width + (prefWidth / 2), width - prefWidth);
                this.transform.Rotate(0, 0, 90, Space.World);
                this.transform.Translate(0, 0, -width + prefWidth);
                /*this.transform.Rotate(-90, 0, 0, Space.World);
                this.transform.Rotate(90, 0, 0, Space.World);
                */
                break;
            case 2: // cara abajo
                this.transform.Rotate(0, 0, -90, Space.World);
                this.transform.Translate((prefWidth / 2), width - (prefWidth / 2), 0);
                
                //this.transform.Rotate(-90, 0, 0, Space.World);
                break;
            case 3:// cara derecha
                this.transform.Rotate(90, 0, 0, Space.World);
                this.transform.Translate(0, width - (prefWidth / 2), (prefWidth / 2));
                break;
            case 4: // cara izquierda
                this.transform.Rotate(-90,0, 0, Space.World);
                this.transform.Translate(0,  + (prefWidth / 2), -width+(prefWidth / 2));

                break;
            case 5: // cara bottom
                this.transform.Rotate(180, 0, 0, Space.World);
                this.transform.Translate(0,width, -width+prefWidth);
                
                //this.transform.Rotate(0, 180, 0, Space.World);
                break;


        }
        
    }

    public void reloadTilePos()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < heigth; j++)
            {
                GameObject aux = tiles[i, j];
                TileScript tileS = aux.GetComponent<TileScript>();
                tileS.reloadPos();
                //tileS.AbsolutePos = transformPos/2;
                /*
                transformPos = this.gameObject.transform.TransformPoint(transformPos);


                tileS.AbsolutePos = cubo.transform.TransformPoint(transformPos);

                */
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

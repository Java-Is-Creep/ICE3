using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFace : MonoBehaviour
{
    public float width;
    public float heigth;
    public int cubeId;

    public GameObject prefabIce;
    public GameObject prefabRock;
    public GameObject prefabTile;

    public int FaceNum; //enumerador que sirve para saber que cara es;

    int probRock = 10;

    float prefWidth = 1;

    GameObject[,] cubeFace;
    // Start is called before the first frame update
    void Start()
    {
        cubeFace = new GameObject[(int)width, (int)heigth];

        for(int x = 0; x<width; x++)
        {
            for (int y = 0; y <heigth; y++)
            {
                int random = Random.Range(0, 100);
                TileScript.type tileType;
                GameObject tileTypeObject;

                if (y == 0 || x == 0 || x == width-1 || y == heigth - 1)
                {
                    tileType = TileScript.type.ROCK;
                    tileTypeObject = prefabRock;
                }
                 else if(random < probRock)
                {
                    tileType = TileScript.type.ROCK;
                    tileTypeObject = prefabRock;
                } else
                {
                    tileType = TileScript.type.ICE;
                    tileTypeObject = prefabIce;
                }

                GameObject tileObject = Instantiate(prefabTile,GetComponentInParent<Transform>());
                tileObject.transform.position = new Vector3(x, 0, y);                
                TileScript tile = tileObject.GetComponent<TileScript>();
                    tile.initTile(x, y, cubeId, x, y, tileTypeObject,tileType);
                cubeFace[x, y] = tileObject;
            }
        }

        switch (FaceNum)
        {
            case 0: // cara arriba
                break;
            case 1: //cara derecha
                this.transform.Translate(0, -(prefWidth / 2), width - (prefWidth / 2));
                this.transform.Rotate(90, 0, 0);
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

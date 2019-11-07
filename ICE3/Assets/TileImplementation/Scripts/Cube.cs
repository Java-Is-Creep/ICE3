using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
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
    void Start()
    {
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
                    TileScript.type tileType;
                    GameObject tileTypeObject;

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

                    GameObject tileObject = Instantiate(prefabTile, facesObject[i].GetComponent < Transform>());
                    tileObject.transform.position = new Vector3(x, 0, y);
                    TileScript tile = tileObject.GetComponent<TileScript>();
                    tile.initTile(x, y, i, x, y, tileTypeObject, tileType);
                    cubeFace[x, y] = tileObject;
                }
            }
            faces[i] = facesObject[i].GetComponent<CubeFace>();
            faces[i].initCube(width, heigth, i, cubeFace);
                 
        }
        this.transform.position = new Vector3(-width/2, heigth/2, -heigth/2);
    }

        // Update is called once per frame
        void Update()
        {

        }
    
}

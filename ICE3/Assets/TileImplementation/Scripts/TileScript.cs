using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    int indexX;
    int indexY;
    int cubeId;

    GameObject myObject;

    public float posX;
    public float posY;
    public float posZ;

    public Vector3 AbsolutePos;

    public enum type { ICE,ROCK}

    public type tileType;


    private void Start()
    {
        
    }

    public void initTile(int indexX, int indexY, int cubeId, int posX, int posY, GameObject prefab, type tipe)
    {
        this.indexX = indexX;
        this.indexY = indexY;
        this.cubeId = cubeId;
        myObject = Instantiate(prefab,this.GetComponentInParent<Transform>());
        tileType = tipe;
    }
}

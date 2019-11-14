using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    int indexX;
    int indexY;
    int cubeId;

    GameObject myGround;
    GameObject myObject;

    public Vector3 AbsolutePos;

    public enum type { ICE,ROCK,NULL}

    public enum tileObject { NULL, ROCK}

    public type tileType;

    public tileObject myObjectType;


    private void Start()
    {
        
    }


    public void reloadPos()
    {
        AbsolutePos = this.transform.TransformPoint(this.transform.position)/2;
    }



    public void initTile(int indexX, int indexY, int cubeId, int posX, int posY, GameObject prefab, type tipe)
    {
        this.indexX = indexX;
        this.indexY = indexY;
        this.cubeId = cubeId;
        myGround = Instantiate(prefab,this.GetComponentInParent<Transform>());
        tileType = tipe;
    }

    public void hasObject(GameObject prefab, tileObject type)
    {
        myObject = Instantiate(prefab, this.GetComponentInParent<Transform>());
        myObjectType = type;
    }

    public void removeObject()
    {
        myObject.transform.SetParent(null);
        myObject = null;
        myObjectType = tileObject.NULL;
        
    }


}

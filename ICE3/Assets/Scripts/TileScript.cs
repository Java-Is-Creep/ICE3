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
        CubeFace cf = this.transform.parent.GetComponent<CubeFace>();
        switch (cf.faceNum)
        {
            case (0):
                AbsolutePos = cf.transform.position + new Vector3(indexX, 0, indexY);
                break;
            case (1):
                AbsolutePos = cf.transform.position + new Vector3(0,indexX, indexY);
                break;
            case (2):
                AbsolutePos = cf.transform.position + new Vector3(0, -indexX, indexY);
                break;
            case (3):
                AbsolutePos = cf.transform.position + new Vector3(indexX, -indexY,0 );
                break;
            case (4):
                AbsolutePos = cf.transform.position + new Vector3(indexX, indexY, 0);
                Debug.Log(indexX + ", " + indexY + "  " + AbsolutePos);
                break;
            case (5):
                AbsolutePos = cf.transform.position + new Vector3(indexX, 0, -indexY);
                break;
            default:
                AbsolutePos = cf.transform.TransformPoint(this.transform.TransformPoint(this.transform.position) / 2);
                break;
        }
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

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
        if (cf.faceNum == 0)
        {
            AbsolutePos = this.transform.TransformPoint(this.transform.position) / 2;
        }
        else if (cf.faceNum == 3)
        {
            /*
            Transform auxCara = this.transform.parent.transform;
            Transform auxCubo = this.transform.parent.transform.parent.transform;
            Quaternion a = auxCara.rotation;
            Quaternion b = auxCubo.rotation;
            auxCara.rotation = Quaternion.identity;
            auxCubo.rotation = Quaternion.identity;
            AbsolutePos = auxCara.position - auxCubo.position;
            auxCara.rotation = a;
            auxCubo.rotation = b;*/
            AbsolutePos = this.transform.TransformPoint(this.transform.position) / 2;

        } else
        {
            AbsolutePos = this.transform.position - this.transform.parent.transform.position - this.transform.parent.transform.parent.transform.position;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoPiedra : MonoBehaviour
{
    int casillaX;
    int casillaY;
    int numeroCara;
    Cube cubo;
    GameObject cuboObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void init(int casillaX, int casillaY, int numCara, GameObject cubo )
    {
        this.casillaX = casillaX;
        this.casillaY = casillaY;
        this.numeroCara = numCara;
        this.cuboObject = cubo;
        this.cubo = cubo.GetComponent<Cube>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

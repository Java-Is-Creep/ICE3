using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFace : MonoBehaviour
{
     float width;
     float heigth;

    int faceNum; //enumerador que sirve para saber que cara es;

    

    float prefWidth = 1;

    GameObject[,] cubeFace;

    
    public void initCube (float width, float heigth, int faceNum, GameObject[,] cubeFace)
    {
        this.width = width;
        this.heigth = heigth;
        this.faceNum = faceNum;
        this.cubeFace = cubeFace;
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
                this.transform.Translate(width - prefWidth, -width + (prefWidth / 2), width - (prefWidth / 2));
                this.transform.Rotate(90, 0, 0, Space.Self);
                this.transform.Rotate(0, 0, 180, Space.World);
                break;
            case 2: // cara abajo
                this.transform.Translate(0, -width + (prefWidth / 2), -(prefWidth / 2));
                this.transform.Rotate(-90, 0, 0);
                break;
            case 3:// cara derecha
                this.transform.Translate(-(prefWidth / 2), -width + (prefWidth / 2), width - prefWidth);
                this.transform.Rotate(0, 0, 90, Space.Self);
                this.transform.Rotate(-90, 0, 0, Space.World);
                break;
            case 4: // cara izquierda
                this.transform.Translate(width - (prefWidth / 2), -width + (prefWidth / 2), 0);
                this.transform.Rotate(0, 0, -90, Space.Self);
                this.transform.Rotate(-90, 0, 0, Space.World);
                break;
            case 5: // cara bottom
                this.transform.Translate(width - prefWidth, -width, 0);
                this.transform.Rotate(180, 0, 0, Space.Self);
                this.transform.Rotate(0, 180, 0, Space.World);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

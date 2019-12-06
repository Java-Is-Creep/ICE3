using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntuacion {
    public string nombre;
    public int puntuacion;

    public Puntuacion(string name)
    {
        nombre = name;
        puntuacion = 0;
    }

    public void aumentarPuntuacion()
    {
        puntuacion++;
    }

    public void disminuirPuntuacion()
    {
        puntuacion--;
    }
    
}

public class ComparadorPuntuacion : IComparer<Puntuacion>
{
    public int Compare(Puntuacion x, Puntuacion y)
    {
        return  y.puntuacion - x.puntuacion;
    }
}

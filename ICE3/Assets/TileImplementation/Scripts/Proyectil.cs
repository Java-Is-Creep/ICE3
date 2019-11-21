﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Vector3 direccion;
    public float incrementoPosicion;
    public GameObject dueño;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += direccion *incrementoPosicion;
    }

    public void initDireccion(Vector3 direccion, GameObject dueño)
    {
        this.direccion = direccion.normalized;
        this.dueño = dueño;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag.CompareTo("Rock") == 0)
        {
            Destroy(this.gameObject);
        } else if (other.gameObject.tag.CompareTo("Player") == 0)
        {
            if(dueño == other.gameObject)
            {
                Debug.Log("choque contigo mismo");
            } else
            {
                Debug.Log("Choque con enemigo");
            }
            
        }
    }
}
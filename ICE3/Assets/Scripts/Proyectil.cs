
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Vector3 direccion;
    public float incrementoPosicion;
    GameObject dueño;
    public float height;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += direccion *incrementoPosicion;
        if(Vector3.Distance(this.transform.position, dueño.transform.position) > 2 * height)
        {
            Destroy(this.gameObject);
        }
    }

    public void initDireccion(Vector3 direccion, GameObject dueño, float height)
    {
        this.direccion = direccion.normalized;
        this.dueño = dueño;
        this.height = height;
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Vector3 direccion;
    public float incrementoPosicion;
    public GameObject dueño;
    public float height;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += direccion * incrementoPosicion;
        if (Vector3.Distance(this.transform.position, dueño.transform.position) > 2 * height)
        {
            Destroy(this.gameObject);
        }
    }

    public void initDireccion(Vector3 direccion, GameObject dueño, float height)
    {
        this.direccion = direccion.normalized;
        this.dueño = dueño;
        Debug.Log("mi dueño es el master: " + dueño.gameObject.GetComponent<CharacterController>().soyMaster());
        this.height = height;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.CompareTo("Rock") == 0)
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag.CompareTo("CharacterCollider") == 0)
        {
            if (other.gameObject.transform.parent.gameObject == dueño)
            {

                Debug.Log("choque contigo mismo");
            }
            else
            {
                dueño.GetComponent<CharacterController>().sumarPuntosBolas();
                Debug.Log("Choque con enemigo");
                Destroy(this.gameObject);
            }
        }
    }




}


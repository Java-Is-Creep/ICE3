using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTimeWater : MonoBehaviour
{

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetFloat("velocity", Random.Range(0.8f, 1.5f));
        anim.SetFloat("time", Random.Range(0.0f, 10.0f));
    }

}

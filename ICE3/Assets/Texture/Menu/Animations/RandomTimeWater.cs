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

        anim.SetFloat("velocity", Random.Range(0.6f, 2.0f));
        anim.SetFloat("time", Random.Range(0.0f, 0.3f));
    }

}

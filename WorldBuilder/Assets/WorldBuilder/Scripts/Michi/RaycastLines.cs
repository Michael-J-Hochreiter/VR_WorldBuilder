using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLines : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        //pee pee poo poo
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

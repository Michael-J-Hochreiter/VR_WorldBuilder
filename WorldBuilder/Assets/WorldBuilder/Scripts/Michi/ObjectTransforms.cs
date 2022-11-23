using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransforms : MonoBehaviour
{
    [HideInInspector] public Vector3 scale;
    [HideInInspector] public Vector3 rotation;
    // Start is called before the first frame update
    void Awake()
    {
        scale = Vector3.one;
        rotation = Vector3.zero;
    }
}

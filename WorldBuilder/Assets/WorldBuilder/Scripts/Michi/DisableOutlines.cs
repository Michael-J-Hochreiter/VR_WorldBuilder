// AUTHOR: MICHAEL HOCHREITER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOutlines : MonoBehaviour
{
    void Start()
    {
        transform.GetComponent<Outline>().enabled = false;
    }

}

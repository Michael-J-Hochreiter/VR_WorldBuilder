//@Author Philipp Thayer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateParentPosition : MonoBehaviour
{
    private GameObject modificationParent;

    private StateMachine stateMachine;

    private bool update = true;

    // Start is called before the first frame update
    void Awake()
    {
        modificationParent = GameObject.FindWithTag("ModificationParent");
        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();
    }

    // Update is called once per frame
    
    void Update()
    {
        if (modificationParent.transform.childCount != 0 && stateMachine.rightGrabPressed &&
            stateMachine.leftGrabPressed && update)
        {
            updateParent();
            update = false;
        }else if (stateMachine.rightGrabReleased && stateMachine.leftGrabReleased && !update)
        {
            update = true;
        }
    }


    public void updateParent()
    {
        List<GameObject> objects = new List<GameObject>();
        List<Vector3> objectPosition = new List<Vector3>();
        //List<Vector3> objectScale = new List<Vector3>();
        List<Quaternion> objectRotation = new List<Quaternion>();
        Vector3 newPosition = Vector3.zero;
        
        foreach (Transform child in modificationParent.transform)
        {
            newPosition += child.position;
            objects.Add(child.gameObject);
            objectPosition.Add(child.position);
            objectRotation.Add(child.rotation);
            //objectScale.Add(child.localScale);
        }
        foreach (GameObject child in objects)
        {
            child.transform.parent = null;
        }

        newPosition /= objects.Count;
        modificationParent.transform.position = newPosition;
        modificationParent.transform.rotation = Quaternion.identity;
        modificationParent.transform.localScale = Vector3.one;
        
        foreach (GameObject block in objects)
        {
            block.transform.parent = modificationParent.transform;
        }
        int i = 0;
        foreach (GameObject block in objects)
        {
            //block.transform.localScale = objectScale[i];
            block.transform.position = objectPosition[i];
            block.transform.localRotation = objectRotation[i];
            i++;
        }
    }
}

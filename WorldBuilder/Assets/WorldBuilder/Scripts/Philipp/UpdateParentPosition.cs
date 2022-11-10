//@Author Philipp Thayer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateParentPosition : MonoBehaviour
{
    private GameObject modificationParent;

    private StateMachine stateMachine;

    private bool update = true;

    private List<Vector3> objectPositions = new List<Vector3>();
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
            print("updating position");
            updatePosition();
            update = false;
        }else if (stateMachine.rightGrabReleased && stateMachine.leftGrabReleased && !update)
        {
            update = true;
        }
    }

    private void updatePosition()
    {
        Vector3 newPosition = Vector3.zero;
        foreach (Transform child in modificationParent.transform)
        {
            newPosition += child.position;
            objectPositions.Add(child.position);
        }

        newPosition /= modificationParent.transform.childCount;
        modificationParent.transform.position = newPosition;
        for (int i = 0; i < modificationParent.transform.childCount; i++)
        {
            modificationParent.transform.GetChild(i).transform.position = objectPositions[i];
        }
    }
}

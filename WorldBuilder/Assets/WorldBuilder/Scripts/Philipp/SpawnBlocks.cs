//@Author Philipp Thayer

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnBlocks : MonoBehaviour
{
    private StateMachine stateMachine;

    public GameObject spawnPoint;
    public GameObject blockToSpawn;
    private GameObject modificationParent;
    private GameObject staticBlockParent;
    private OutlineManager outlineManager;


    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        modificationParent = GameObject.FindWithTag("ModificationParent");
        staticBlockParent = GameObject.FindWithTag("StaticBlockParent");
        outlineManager = GameObject.FindWithTag("Player").GetComponent<OutlineManager>();

    }

    private void Update()
    {
        if (stateMachine.state == StateMachine.State.Idle && stateMachine.primaryPressed)
        {
            var block = Instantiate(blockToSpawn, spawnPoint.transform.position, Quaternion.identity);
            //block.transform.parent = modificationParent.transform;
            stateMachine.primaryPressed = false;
        }
        if (stateMachine.state != StateMachine.State.Idle && stateMachine.primaryPressed)
        {
            // List<GameObject> objects = new List<GameObject>();
            // foreach (Transform child in modificationParent.transform)
            // {
            //     objects.Add(child.gameObject);
            // }
            // foreach (GameObject obj in objects)
            // {
            //     obj.transform.parent = staticBlockParent.transform;
            // }
            // stateMachine.state = StateMachine.State.Idle;
            // stateMachine.primaryPressed = false;
            //
            // outlineManager.UpdateOutlines();

            MoveObjectsToStaticBlockParent();
        }
        
    }

    public void MoveObjectsToStaticBlockParent()
    {
        List<GameObject> objects = new List<GameObject>();
        foreach (Transform child in modificationParent.transform)
        {
            objects.Add(child.gameObject);
        }
        foreach (GameObject obj in objects)
        {
            obj.transform.parent = staticBlockParent.transform;
        }
        stateMachine.state = StateMachine.State.Idle;
        stateMachine.primaryPressed = false;
            
        outlineManager.UpdateOutlines();
    }
    
    
    
}

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

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        modificationParent = GameObject.FindWithTag("ModificationParent");
    }

    private void Update()
    {
        if (stateMachine.state == StateMachine.State.Idle && stateMachine.primaryPressed)
        {
            var block = Instantiate(blockToSpawn, spawnPoint.transform.position, Quaternion.identity);
            block.transform.parent = modificationParent.transform;
            stateMachine.primaryPressed = false;
        }
    }
    
}

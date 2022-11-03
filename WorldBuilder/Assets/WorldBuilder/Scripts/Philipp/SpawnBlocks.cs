using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnBlocks : MonoBehaviour
{
    private StateMachine stateMachine;

    public GameObject spawnPoint;
    public GameObject blockToSpawn;

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        if (stateMachine.state == StateMachine.State.Idle && stateMachine.primaryPressed)
        {
            Instantiate(blockToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateBlock : MonoBehaviour
{
    private StateMachine stateMachine;

    public GameObject spawnPoint;
    public GameObject blockToSpawn;
    private GameObject staticBlockParent;
    private GameObject modificationParent;

    // Start is called before the first frame update
    void Awake()
    {   
        stateMachine = GetComponent<StateMachine>();
        staticBlockParent = GameObject.FindWithTag("StaticBlockParent");
        modificationParent = GameObject.FindWithTag("ModificationParent");
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.primaryPressed && modificationParent.transform.childCount == 1)
        {
            GameObject objectToDuplicate = modificationParent.transform.GetChild(0).gameObject;
            Vector3 scale = modificationParent.transform.GetChild(0).localScale;
            Quaternion rotation = modificationParent.transform.GetChild(0).transform.rotation;
            var block = Instantiate(objectToDuplicate, spawnPoint.transform.position, rotation);
            block.transform.localScale = scale;
            block.transform.parent = staticBlockParent.transform;
            objectToDuplicate.transform.parent = staticBlockParent.transform;
            stateMachine.primaryPressed = false;
        }
    }
}

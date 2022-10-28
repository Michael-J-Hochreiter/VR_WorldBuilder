// AUTHOR: MICHAEL HOCHREITER

using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    public float distanceToPlayer = 2f;
    
    private Transform player;
    [HideInInspector] public Transform buildingBlock;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        // always rotate towards player
        transform.LookAt(player);
        
        // always set position between player and buildingBlock;
        transform.position = player.position + Vector3.Normalize(buildingBlock.position - player.position) * distanceToPlayer;
    }
}

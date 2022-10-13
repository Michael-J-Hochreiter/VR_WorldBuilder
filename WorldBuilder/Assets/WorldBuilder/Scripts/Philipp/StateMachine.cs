using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [HideInInspector] public GameObject currentObject;
    public enum State
    {
        Idle,
        EditingRotation,
        EditingScaleAllAxis,
        EditingScaleIndividualAxis,
        EditingTranslation
    }

    [HideInInspector] public State state;
    
    // Start is called before the first frame update
    void Awake()
    {
        state = State.Idle;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class RaycastLines : MonoBehaviour
{
    public bool debug = false;
    
    private LineRenderer lineRenderer;
    private StateMachine stateMachine;
    private Color aimedAtBlockEndColor = new Color(0, 1, 0);
    private Color aimedAtBlockStartColor = new Color(0.7f, 1, 0.7f);

    void Awake()
    {
        if (transform.GetComponent<LineRenderer>() == null)
        {
            transform.AddComponent<LineRenderer>();
        }

        lineRenderer = transform.GetComponent<LineRenderer>();

        if (!debug)
        {
            stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();
        }
    }

    private void Start()
    {
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startColor = new Color(1,1,1);
        lineRenderer.endColor = new Color(1,1,1);
        lineRenderer.loop = false;
    }

    void Update()
    {
 
        if (debug)
        {
            DrawRaycastLine();
        }
        else
        {
            if (stateMachine.state == StateMachine.State.Idle ||
                stateMachine.state == StateMachine.State.EditingTranslation)
            {
                DrawRaycastLine();
            }
        }
    }

    void DrawRaycastLine()
    {
        var points = new Vector3[2];
        points[0] = transform.position;
        points[1] = transform.position + Vector3.Normalize(transform.forward) * 100f; // by default the end point is just very far away
        
        // do raycast and set the endpoint of the linerenderer to where the first hit is (for example a UI)
        Ray ray = new Ray(
            transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            points[1] = hit.point;
            if (hit.collider.gameObject.CompareTag("BuildingBlock"))
            {
                lineRenderer.endColor = aimedAtBlockEndColor;
                lineRenderer.startColor = aimedAtBlockStartColor;
            }
            else
            {
                lineRenderer.endColor = Color.white;
                lineRenderer.startColor = Color.white;
            }
        }
        else
        {
            lineRenderer.endColor = Color.white;
            lineRenderer.startColor = Color.white;
        }
        lineRenderer.SetPositions(points);
    }
}
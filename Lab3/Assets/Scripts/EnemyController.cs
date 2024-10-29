using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent enemyAgent;
    Transform enemyTransform;
    public Transform target;
    [SerializeField]
    private int currentPosition = 0;

    public List<Transform> positions = new List<Transform>();

    
    void Start()
    {

        enemyAgent = GetComponent<NavMeshAgent>();
        enemyTransform = GetComponent<Transform>();
    }
    void Update()
    {
        
            enemyAgent.destination = positions[currentPosition].transform.position;
            //Debug.LogWarning(enemyAgent.remainingDistance.ToString());
            if(!enemyAgent.pathPending && enemyAgent.remainingDistance < 0.6f)
            {
                currentPosition++;
                if(currentPosition > positions.Count - 1)
                {
                    currentPosition = 0;
                }
            }
    }
}

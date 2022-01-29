using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PointClickMovement : MonoBehaviour
{
    [SerializeField]
    private Camera localCamera;
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private Animator animator;


    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if(Physics.Raycast(ray, out hitPoint)){
                navMeshAgent.SetDestination(hitPoint.point);
            }
        }
    }
}

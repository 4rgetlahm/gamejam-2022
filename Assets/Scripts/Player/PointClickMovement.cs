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

    public Animator animator;

    public float animationSpeed;

    private Vector3 LastPosition;

    public float PlayerMovementSpeed;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.speed = animationSpeed;
        LastPosition = GetPlayerPosition();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint))
            {
                navMeshAgent.speed = PlayerMovementSpeed;
                navMeshAgent.SetDestination(hitPoint.point);
                animator.SetBool("IsWalking", true);
            }
        }
        var newPosition = GetPlayerPosition();
        if (LastPosition == newPosition)
            animator.SetBool("IsWalking", false);
        else
            animator.SetBool("IsWalking", true);

        LastPosition = newPosition;
    }

    private Vector3 GetPlayerPosition()
    {
        var playerObject = GameObject.Find("Player");
        return playerObject.transform.position;
    }
}

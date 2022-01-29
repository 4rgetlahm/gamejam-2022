using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InteractionHandler : MonoBehaviour
{

    [SerializeField]
    private List<InteractableObject> interactables;

    [SerializeField]
    private Camera localCamera;

    public float MinimumDistance;

    void Start()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Tree"))
        {
            interactables.Add(new InteractableObject(gameObject, InteractionType.WOOD));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint))
            {
                InteractableObject interactableObject = interactables.Where(i => i.gameObject.Equals(hitPoint.transform.gameObject)).FirstOrDefault();
                if (interactableObject == null){
                    return;
                }
                GameObject.FindGameObjectWithTag("DialogHandler").GetComponent<DialogHandler>().OpenDialog("Toss a coin to determine the result of your action?", OnGoodClick, delegate { });
               
                if (IsNear(interactableObject))
                {
                    GameObject.Destroy(interactableObject.gameObject);
                }
            }
        }
    }


    public void OnGoodClick()
    {
        Debug.Log(":)))))))");
        //GameObject.Destroy(interactableObject.gameObject);
    }
    private bool IsNear(InteractableObject intObj)
    {
        return Vector3.Distance(Player.GetPosition(), intObj.gameObject.transform.position) <= MinimumDistance;
    }
}

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

    void Start(){
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Tree")){
            interactables.Add(new InteractableObject(gameObject, InteractionType.WOOD));
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if(Physics.Raycast(ray, out hitPoint)){
                InteractableObject interactableObject = interactables.Where(i => i.gameObject.Equals(hitPoint.transform.gameObject)).FirstOrDefault();
                if(interactableObject == null){
                    return;
                }

                GameObject.Destroy(interactableObject.gameObject);
            }
        }
    }
}

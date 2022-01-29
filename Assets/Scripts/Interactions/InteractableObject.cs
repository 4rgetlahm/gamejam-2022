using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractableObject
{
    public GameObject gameObject;
    public InteractionType interactionType;

    public InteractableObject(GameObject gameObject, InteractionType interactionType){
        this.gameObject = gameObject;
        this.interactionType = interactionType;
    }
}

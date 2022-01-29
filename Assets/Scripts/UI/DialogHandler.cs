using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject dialog;

    public void OpenDialog(string text, Action primaryAction, Action secondaryAction){
        if (GameObject.FindGameObjectWithTag("Dialog") == null) { 
            GameObject copiedDialog = GameObject.Instantiate(dialog, this.transform);
            copiedDialog.GetComponent<DialogData>().ChangeText(text);
            copiedDialog.GetComponent<DialogData>().ChangeActions(primaryAction, secondaryAction);
            
        }
    }

}

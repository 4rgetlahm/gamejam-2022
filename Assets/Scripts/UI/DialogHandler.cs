using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject dialog;

    public void OpenDialog(string text, Action primaryAction, Action secondaryAction){
        GameObject copiedDialog = GameObject.Instantiate(dialog, this.transform);
        copiedDialog.tag = "Dialog";
        copiedDialog.GetComponent<DialogData>().ChangeText(text);
        copiedDialog.GetComponent<DialogData>().ChangeActions(primaryAction, secondaryAction);
    }
    
    public static bool IsAnyDialogOpen()
    {
        return GameObject.FindGameObjectsWithTag("Dialog").Length != 0;
    }

}

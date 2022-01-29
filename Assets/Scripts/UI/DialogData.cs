using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogData : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogText;

    private Action acceptAction;
    private Action denyAction;

    public void ChangeText(string text){
        dialogText.text = text;
    }

    public void ChangeActions(Action primaryAction, Action secondaryAction)
    {
        acceptAction = primaryAction;
        denyAction = secondaryAction;
    }

    public void OnAcceptButtonClick()
    {
        acceptAction.Invoke();
        GameObject.Destroy(this.gameObject);
    }
    public void OnDenyButtonClick()
    {
        denyAction.Invoke();
        GameObject.Destroy(this.gameObject);
    }
}

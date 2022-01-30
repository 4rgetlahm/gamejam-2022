using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject alertPrefab;

    public void ShowAlert(string text, float time){
        GameObject alertCopy = GameObject.Instantiate(alertPrefab, this.transform);
        alertCopy.GetComponent<AlertData>().StartAlert(text, time);
    }
}

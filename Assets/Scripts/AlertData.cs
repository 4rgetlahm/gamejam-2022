using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertData : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textObject;
    
    private float timeLeft = 5f;

    public void StartAlert(string text, float timeUntilRemoved){
        textObject.text = text;
        this.timeLeft = timeUntilRemoved;
    }

    // Update is called once per frame
    void Update()
    {
        this.timeLeft -= Time.deltaTime;
        if(this.timeLeft <= 0){
            GameObject.Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject dialog;
    void Start()
    {

    }

    public void OpenDialog(){
        GameObject.Instantiate(dialog, this.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

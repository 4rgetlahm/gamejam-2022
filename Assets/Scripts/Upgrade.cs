using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if ( playerData.bones >= 10
            && playerData.wood >= 10
            && playerData.stone >= 10)
            {

                playerData.bones -= 10;
                playerData.wood -= 10;
                playerData.stone -= 10;
               // Debug.Log(":)))))))");
            }
            Debug.Log(":)))))))");
        }
            
        
    }
}

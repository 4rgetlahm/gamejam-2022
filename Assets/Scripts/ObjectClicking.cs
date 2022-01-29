using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicking : MonoBehaviour
{
    public int Probability;
    public CoinToss coinTosser;
    void Start()
    {
        coinTosser = new CoinToss();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    print(coinTosser.TossCoin(Probability).ToString());
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCoin : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(this.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}

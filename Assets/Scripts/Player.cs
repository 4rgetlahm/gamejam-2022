using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player
{
    public static Vector3 GetPosition()
    {
        var playerObject = GameObject.Find("Player");
        return playerObject.transform.position;
    }
}

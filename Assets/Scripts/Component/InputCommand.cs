using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InputCommand : NetworkBehaviour
{
    [SyncVar]
    public Vector2 input;

    [SyncVar]
    public bool isFire;
    //otherButton

    [Command]
    public void SetInput(Vector2 vc)
    { 
        input = vc;
    }

    [Command]
    public void SetButton(int id)
    {
        isFire = true;
    }
}

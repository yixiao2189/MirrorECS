using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AniState : NetworkBehaviour
{
    [SyncVar]
    public bool Moving;
    [SyncVar]
    public bool Shoot;
   
}

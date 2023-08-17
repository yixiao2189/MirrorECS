using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderConfig : NetworkBehaviour
{
    [SyncVar]
    public float radius;
}

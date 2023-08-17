using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMove : NetworkBehaviour
{
    [SyncVar]
    public Vector3 force;
}

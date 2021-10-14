using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NewBehaviourScript : MonoBehaviour
{
    public uint netId;


    // Update is called once per frame
    void Update()
    {
        netId = GetComponent<Mirror.NetworkIdentity>().netId;
    }
}

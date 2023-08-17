using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Globalization;
using UnityEngine.AI;
using System;

public class BattleInput : MonoBehaviour
{
    public const KeyCode shootKey = KeyCode.Space;
    void Update()
    {
        if (NetworkClient.localPlayer == null)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 inputMove = new Vector2(horizontal, vertical);

        MoveLogic(transform,inputMove);

        var eComp = transform.GetComponent<EgoComponent>();
        var command = EgoHelper.AddNetComponentCommad<InputCommand>(eComp);
        var isFire = Input.GetKeyDown(shootKey);
        if (isFire)
        {
            command.SetButton(0);
        }
    }


    void MoveLogic(Transform viewGoTrans,Vector2 inputMove)
    {
        var model = GetComponent<Model>();
        if (model == null || model.viewGO == null)
            return;
        var agent = viewGoTrans.GetComponent<NavMeshAgent>();
        if(agent == null)
            return;

        viewGoTrans.Rotate(0, inputMove.x * 100 * Time.deltaTime, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (agent != null)
            agent.velocity = forward * Mathf.Max(inputMove.y, 0) * agent.speed;
    }
}

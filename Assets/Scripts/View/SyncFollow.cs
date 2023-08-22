using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncFollow : MonoBehaviour
{
    public Vector3 targetPos;
    public Transform target;
    public float maxAngularSpeed = 280;
    public float minSmoothSpeed = 1f;
    public float targetCatchupTime = 0.1f;

    float m_SmoothedSpeed;
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }



        VisualUtils.SmoothMove(transform,target,Time.deltaTime,ref m_SmoothedSpeed, maxAngularSpeed, minSmoothSpeed, targetCatchupTime);

        targetPos = target.position;
        float dif = (targetPos - transform.position).magnitude;
        if(dif > 0)
            Debug.Log($"dif : {dif}");
    }
}

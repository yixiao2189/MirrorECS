using UnityEngine;
using System.Collections.Generic;

public class EgoInterface : MonoBehaviour
{

    public virtual void Start()
    {
    	EgoSystems.Start();
	}

    public virtual void Update()
	{
		EgoSystems.Update();
	}

    public virtual void FixedUpdate()
	{
		EgoSystems.FixedUpdate();
	}
}

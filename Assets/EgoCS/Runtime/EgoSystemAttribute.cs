using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EgoSide
{
    ALL,
    SERVER,
    CLIENT
}

public enum EgoLayer
{
    NONE = 0,
    Front = -1,
    Unsoted = 0,
    Back  = 1
}

[AttributeUsage(AttributeTargets.Class)]
public class EgoSystemAttribute : Attribute
{

    public EgoSystemAttribute(EgoSide side,EgoLayer layer = EgoLayer.NONE ,int priority = 0,bool active = true) {
        this.side = side;
        this.priority = priority;
        this.layer = layer;
        this.acitve = active;
    }


    public  EgoSide side;
    public EgoLayer layer;
    public int priority = 0;
    public bool acitve = true;
}

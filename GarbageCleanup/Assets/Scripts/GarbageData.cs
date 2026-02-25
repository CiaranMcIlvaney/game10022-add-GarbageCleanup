using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Garbage { Waste, Plastic, Paper, Electronic }

[Serializable]

public struct GarbageData
{
    public Garbage garbageType;
}
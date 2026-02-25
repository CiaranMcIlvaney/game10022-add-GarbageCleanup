using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Garbage { Waste, Plastic, Paper, Electronic}

[Serializable]

public struct TetrominoData
{
    public Garbage garbageType;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityToNumerics //wrapper to translate Unity Vectors to Numerics Vectors so that I don't have to add any new constructors to standart classes
{
    public static System.Numerics.Vector2 UtoN(UnityEngine.Vector3 v)
    {
        return new System.Numerics.Vector2(v.x, v.y);
    }

    public static UnityEngine.Vector3 NtoU(System.Numerics.Vector2 v)
    {
        return new UnityEngine.Vector3(v.X, v.Y, 0);
    }
}


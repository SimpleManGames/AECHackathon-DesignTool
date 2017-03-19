using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMethods {

    public static Vector3 AverageVector(Vector3[] vectors)
    {
        Vector3 tmp = Vector3.zero;

        foreach (Vector3 v in vectors) tmp += v;

        return tmp / vectors.Length;
    }
	
}

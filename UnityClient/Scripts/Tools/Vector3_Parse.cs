using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3_Parse
{
    public static Vector3 Parse(string s)
    {
        string[] strs = s.Split(',');
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);
        return new Vector3(x, y, z);
    }
}

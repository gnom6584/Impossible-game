using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Lighting : MonoBehaviour
{
    public static List<Vector4> lights = new List<Vector4>();
    public float intensivity = 1;
    private void Start()
    {
        Shader.SetGlobalInt("count", 0);
        Vector4[] temp = new Vector4[256];
        Shader.SetGlobalVectorArray("pointLights", temp);
    }
    void Update()
    {
  
        Shader.SetGlobalVectorArray("pointLights", lights);
        Shader.SetGlobalInt("count", lights.Count);
    }
}

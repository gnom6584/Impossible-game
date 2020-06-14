using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : Lighting
{
    int id;
    private void Awake()
    {
        lights.Add(new Vector3(transform.position.x, transform.position.y, intensivity));
        id = lights.Count - 1;
    }
    private void OnDestroy()
    {
        lights.RemoveAt(id);
    }
}

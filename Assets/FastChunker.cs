using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastChunker : MonoBehaviour
{
    [SerializeField]
    GameObject chunk;
    Vector3[,] obj;

    void Start()
    {
        obj = new Vector3[25, 25];
        for (int i = 0; i < 25; i++)
        {
            for (int j = 0; j < 25; j++)
                obj[i, j] = new Vector3(i * 32, j * 32,0);
        }
    }

    void Update()
    {
        for(int i = 0; i < 25;i++)
        {
            for (int j = 0; j < 25; j++)
            {
                if (Vector3.Distance(new Vector3(Camera.main.transform.position.x-16, Camera.main.transform.position.y-16,0), obj[i, j]) < 64)
                {
                    if (obj[i, j].z == 0)
                    {
                        Instantiate(chunk, obj[i, j], transform.rotation);
                        obj[i, j] = new Vector3(obj[i, j].x, obj[i, j].y, 1);
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTransform : MonoBehaviour
{

    void Update()
    {
        if (transform.position.x > (Camera.main.orthographicSize * 2)) transform.position = new Vector2(-(Camera.main.orthographicSize * 2), transform.position.y);
        if (transform.position.x < -(Camera.main.orthographicSize * 2)) transform.position = new Vector2((Camera.main.orthographicSize * 2), transform.position.y);
    }
}

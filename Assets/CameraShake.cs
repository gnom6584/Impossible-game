using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 position;
    bool playing = false;
    private void Start()
    {
        position = Camera.main.transform.localPosition;
    }
    public IEnumerator Shake(float duration,float magnitude)
    {
        if (!playing)
        {
            playing = true;
            float rand = Random.Range(0,361)*Mathf.Deg2Rad;
            float x = Mathf.Sin(rand) * magnitude;
            if (Mathf.Abs(x) > 0.5f) x = 0.5f;
            float y =  Mathf.Cos(rand) * magnitude;

            transform.localPosition = new Vector3(x, y, -10);
            
            playing = false;
            yield return null;
        }
    }
    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime*10);
    }
}
 
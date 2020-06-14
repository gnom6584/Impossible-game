using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField]
    Transform pos;
    void Start()
    {
        
    }
    [SerializeField]TextMesh text;
    void Update()
    {
        Vector3 c =  Vector3.Cross(transform.forward, (pos.position - transform.position).normalized);
       
      //  if(Vector3.SignedAngle(transform.forward, (pos.position - transform.position).normalized, transform.up)< 0)
            text.text = " " + (Vector3.SignedAngle(transform.forward, (pos.position - transform.position).normalized, transform.up));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public int hp = 100;
    public ParticleSystem collideParticle;
    public bool rigidbodyForces = false;
    public AudioClip destroyClip;
    public float tverdost = 1;
    void Update()
    {

    }
    public void PartPlay(Vector3 pos)
    {
        collideParticle.transform.position = pos;
        collideParticle.Play();
    }
    public void PlayAudio()
    {
        GameObject temp = new GameObject();
        temp.AddComponent<AudioSource>().clip = destroyClip;
        temp.GetComponent<AudioSource>().Play();
        Destroy(temp, 5);
    }
}

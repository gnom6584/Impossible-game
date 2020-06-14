using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesCollision : MonoBehaviour
{
    public int damage;
    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        
        if (other.GetComponent<DestroyableObject>())
        {
            DestroyableObject obj = other.GetComponent<DestroyableObject>();
            obj.PartPlay(collisionEvents[0].intersection);
            obj.hp-=damage;
            if(obj.rigidbodyForces)
            {
                obj.gameObject.GetComponent<Rigidbody2D>().AddForce((-collisionEvents[0].intersection + other.transform.position).normalized*60);
            }
        }            
    }
}

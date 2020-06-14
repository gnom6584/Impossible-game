using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    public int maxStars = 1000;
    public int universeSize = 10;
    ParticleSystem theParticleSystem;
    private ParticleSystem.Particle[] points;

    private void Create()
    {
        theParticleSystem = GetComponent<ParticleSystem>();
        points = new ParticleSystem.Particle[maxStars];

        for (int i = 0; i < maxStars; i++)
        {
            points[i].position = Random.insideUnitSphere * universeSize;
            points[i].startSize = Random.Range(0.05f, 0.05f);
            points[i].startColor = new Color(1, 1, 1, 1);
        }

    }

    void Start()
    {

        Create();
    }

    // Update is called once per frame
    void Update()
    {
        if (points != null)
        {

            theParticleSystem.SetParticles(points, points.Length);

        }
    }
}

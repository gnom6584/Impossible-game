using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField]
    Sprite[] clouds;
    public static int maxClouds = 10;
    [SerializeField]
    public static float speed = 0.5f;

    void Start()
    {
        for(int i = 0; i < maxClouds; i++)
        {
            cloud = new GameObject("Cloud").AddComponent<SpriteRenderer>();
            cloud.sprite = clouds[Random.Range(0, clouds.Length)];
            cloud.transform.position = new Vector3(Random.Range(-12,16), Random.Range(30, 55) / 10f, 0);
            StartCoroutine(CloudMove(cloud.transform));
        }
        StartCoroutine(CloudGenerator());
    }
    SpriteRenderer cloud;
    IEnumerator CloudGenerator()
    {
        while(enabled)
        {
            cloud = new GameObject("Cloud").AddComponent<SpriteRenderer>();
            cloud.sprite = clouds[Random.Range(0, clouds.Length)];
            cloud.transform.position = new Vector3(12, Random.Range(30,55)/10f,0);
            StartCoroutine(CloudMove(cloud.transform));
            yield return new WaitForSeconds(Random.Range(4,8));
        }
        yield return null;
    }
    IEnumerator CloudMove(Transform cloud)
    {
        while(cloud.position.x > -12)
        {
            cloud.transform.position -= new Vector3(speed/100f, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(cloud.gameObject);
    }
}

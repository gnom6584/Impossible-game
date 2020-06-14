using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        time = Random.Range(10, 30) / 10f;
        boxCount = 0;
    }
    float time;
    public static int boxCount = 0;
    public static int max;
    // Update is called once per frame
    void Update()
    {
        if (!Chuck.afk)
        {
       //     Debug.Log("huivrotlog");
            time -= Time.deltaTime;
            if (time <= 0)
            {
                if (boxCount < max)
                {
                    boxCount++;
                    Instantiate(box, transform.position + new Vector3(Random.Range(-100, 110)/10f, 0, 0), transform.rotation);
                }
                time = Random.Range(10, 30)/10f;
            }
        }
    }
}

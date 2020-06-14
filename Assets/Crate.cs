using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : DestroyableObject
{
    public static GameObject[] items;
    [SerializeField]
    Sprite brokenCrate;
    int id;
    public bool dropItem;
    bool once;
    float lifeTime = 10;
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            gameObject.tag = "Untagged";
            if (GetComponent<SpriteRenderer>().color.a > 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, GetComponent<SpriteRenderer>().color.a - Time.deltaTime);
            }
            else
            {
                BoxSpawner.boxCount--;
                Destroy(gameObject);
            }
        }
        if (hp <= 0 && !once)
        {
            once = true;
            PlayAudio();
            BoxSpawner.boxCount--;
            GetComponent<SpriteRenderer>().sprite = brokenCrate;
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.3f);
            GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y/5);
            transform.rotation = Quaternion.identity;      
            gameObject.tag = "Untagged";
            Destroy(gameObject,2f);
            if(dropItem)
                if(Random.Range(0,3) == 0) Instantiate(items[Random.Range(0,items.Length)], transform.position, Quaternion.identity);
        }
    }
}

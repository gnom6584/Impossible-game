using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string name;
    [SerializeField]
    AudioClip audio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
            Destroy(gameObject);
            PlayAudio();
        }
    }
    void PlayAudio()
    {
        AudioSource a = new GameObject().AddComponent<AudioSource>();
        a.clip = audio;
        a.Play();
        Destroy(a.gameObject, a.clip.length);

    }
}

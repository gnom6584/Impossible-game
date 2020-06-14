using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [Header("SwitchSettings")]
    [SerializeField]
    Image sceneSwitcher;
    [Header("Keyboard")]
    [SerializeField]
    KeyCode ReloadScene = KeyCode.R;
    [SerializeField]
    KeyCode Exit = KeyCode.Escape;
    [Header("Static variables")]
    [SerializeField]
    GameObject[] items;
    int maxBoxCount = 10;
    [SerializeField]
    int maxCloudsCount = 10;
    Transform player;
    enum Action
    {
        None,LoadSelf,Quit
    };
    bool oCwork = false;
    bool soundplay;
    void Awake()
    {
      //  Cursor.lockState = CursorLockMode.Locked;
       // Cursor.visible = false;
        player = GameObject.FindWithTag("Player").transform;
        Crate.items = items;
        BoxSpawner.max = maxBoxCount;
        Clouds.maxClouds = maxCloudsCount;
        StartCoroutine(Open());      
    }
    void LoadSelf()
    {
        if(Player.dead)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Quit()
    {
        StartCoroutine(Close(Action.Quit));
    }
    bool once = false;
    void Update()
    {
        if(Player.close)
        {
            if (!once)
            {   
                once = true;
                StartCoroutine(Close(Action.None));
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadSelf();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
        if(!Chuck.afk)
        {
            if (Camera.main.GetComponent<AudioSource>() && !soundplay)
            {
                Camera.main.GetComponent<AudioSource>().Play();
                soundplay = true;
            }
        }
    }
    IEnumerator Open()
    {
        if (!oCwork)
        {
            oCwork = true;
            float i = 1.0f;
            while (i >= 0)
            {
                if (AudioListener.volume < 1) AudioListener.volume += Game.switchSpeed;
                else AudioListener.volume = 1;
                Vector3 pos = player.position;
                pos.Set(pos.x / (10 * Screen.width / Screen.height), pos.y / 10, pos.z);
                pos.Set(pos.x + 0.5f, pos.y + 0.5f, i);
                sceneSwitcher.material.SetVector("_Value1", pos);
                i -= Game.switchSpeed * Time.deltaTime;
                yield return null;
            }
            sceneSwitcher.material.SetVector("_Value1", Vector4.zero);
            oCwork = false;
        }
    }
    IEnumerator Close(Action action)
    {
        if (!oCwork)
        {
            oCwork = true;
            float i = 0;
            while (i < 1)
            {
                if (action == Action.LoadSelf)
                {
                    AudioListener.volume = 1 - i;
                }
                Vector3 pos = player.transform.position;
                pos.Set(pos.x / (10 * Screen.width / Screen.height), pos.y / 10, pos.z);
                pos.Set(pos.x + 0.5f, pos.y + 0.5f, i);
                sceneSwitcher.material.SetVector("_Value1", pos);
                i += Game.switchSpeed * Time.deltaTime;
                yield return null;
            }
            oCwork = false;
            if(action == Action.None)
            {
                i = 0;
                while(i < 1)
                {
                    GameObject.Find("Text228").GetComponent<Text>().color = new Color(1,1,1,i);
                    GameObject.Find("F").GetComponent<Text>().color = new Color(GameObject.Find("F").GetComponent<Text>().color.r, GameObject.Find("F").GetComponent<Text>().color.g, GameObject.Find("F").GetComponent<Text>().color.b, i);
                    i += 0.0075f*Time.deltaTime;
                }
            }
            if (action == Action.LoadSelf)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (action == Action.Quit)
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }
    }
    private void OnDestroy()
    {
        sceneSwitcher.material.SetVector("_Value1", new Vector4(0,0,0,0));
    }
}

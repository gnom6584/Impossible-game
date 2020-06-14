using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiButton : MonoBehaviour
{
    enum Action
    {
        None,Play
    }
    [SerializeField]
    Action action;
    [SerializeField]
    Color pressColor;
    Color startColor;
    [SerializeField]
    Image screenSwitcher;
    private void Awake()
    {  
        screenSwitcher.color = new Color(0,0,0,1);
        StartCoroutine(Open());
        startColor = GetComponent<Image>().material.GetColor("_Healthbar");
        GetComponent<Image>().material.SetColor("_Healthbar", startColor);
    }
    bool over;
    void OnMouseOver()
    {
        if (over == false)
        {
            over = true;
            GetComponent<AudioSource>().Play();
        }
        GetComponent<Image>().material.SetColor("_Healthbar", pressColor);
    }
    private void OnMouseExit()
    {
        over = false;
        GetComponent<Image>().material.SetColor("_Healthbar", startColor);
    }
    private void OnMouseDown()
    {
        GetComponents<AudioSource>()[1].Play();
        switch (action)
        {
            case Action.Play:
            {
                    StartCoroutine(Close());
                break;
            }
        }
    }
    IEnumerator Open()
    {
        if (!oCwork)
        {
            oCwork = true;
            float i = 1;
            while (i >= 0)
            {
                screenSwitcher.color = new Color(0, 0,0, i);
                i -= Game.switchSpeed * Time.deltaTime;
                yield return null;
            }
            screenSwitcher.color = new Color(0, 0, 0, 0);

           oCwork = false;
        }
    }
    bool oCwork = false;
    IEnumerator Close()
    {
        if (!oCwork)
        {
            oCwork = true;
            float i = 0;
            while (i < 1)
            {
                screenSwitcher.color = new Color(0, 0, 0, i);
                i += Game.switchSpeed * Time.deltaTime;
                yield return null;
            }
            SceneManager.LoadScene("LEVEL1");
            GetComponent<Image>().material.SetColor("_Healthbar", Color.black);
        }
    }
}

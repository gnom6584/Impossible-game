using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AttemptsCounter : MonoBehaviour
{
    [SerializeField]
    string playerPrefs;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = playerPrefs + "\n" + PlayerPrefs.GetInt(playerPrefs);
    }
}

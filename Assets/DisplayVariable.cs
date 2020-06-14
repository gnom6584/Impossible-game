using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayVariable : MonoBehaviour
{
    [SerializeField]
    string playerPrefsVariable;
    [SerializeField]
    VariableType var = VariableType.Int;
    enum VariableType
    {
        Int,Float,String
    }
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    void Update()
    {
        switch (var)
        {
            case VariableType.Int:
            {
                text.text = "" + PlayerPrefs.GetInt(playerPrefsVariable);
                break;
            }
            case VariableType.Float:
            {
                    text.text = "" + PlayerPrefs.GetFloat(playerPrefsVariable);
                break;
            }
            case VariableType.String:
            {
                text.text = PlayerPrefs.GetString(playerPrefsVariable);
                break;
            }
        }
    }
}

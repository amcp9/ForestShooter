using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour 
{
    public Text labelText, msgText;

    public void SetSystemText(string msg)
    {
        labelText.color = new Color32(100, 71, 73, 255);
        msgText.color = new Color32(176, 105, 90, 255);
        labelText.text = "系统" + ":";
        msgText.text = msg;
    }

    public void SetUserText(string name,string msg)
    {
        labelText.color = new Color32(223,225,255,255);
        msgText.color = new Color32(251,252,134,255);
        labelText.text = name + ":";
        msgText.text = msg;
    }
}

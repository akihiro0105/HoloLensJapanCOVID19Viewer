using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class backPanel : MonoBehaviour
{
    // max line 25
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI subText;

    public void Init()
    {
        text.text = "";
        subText.text = "";
    }

    public void SetDictionaryText(Dictionary<string, string> dic)
    {
        Init();
        var count = 0;
        foreach (var item in dic)
        {
            var line = $"{item.Key} : {item.Value}" + Environment.NewLine;
            if (count < 25) text.text += line;
            else subText.text += line;
            count++;
        }
    }
}

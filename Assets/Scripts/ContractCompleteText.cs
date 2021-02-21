using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractCompleteText : MonoBehaviour
{
    public ShipController ship;

    private Text text;
    private string initialText;

    void Start()
    {
        text = GetComponent<Text>();
        initialText = text.text;
    }

    void Update()
    {
        var time = Format.Duration(ship.ContactTime);
        text.text = initialText.Replace("00:00", time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferringOreText : MonoBehaviour
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
        var percentage = ((1f - (ship.ore / ship.capacity)) * 100f).ToString("0");
        text.text = initialText.Replace("99", percentage);
    }
}

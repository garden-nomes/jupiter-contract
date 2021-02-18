using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    private enum State
    {
        Default,
        Mining,
        Stabilizing
    }

    public ShipController ship;

    private TextMeshProUGUI text;
    private float flashTimer = 0f;
    private State state = State.Default;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        var newState = State.Default;
        if (ship.IsMining) newState = State.Mining;
        if (ship.IsStabilizing) newState = State.Stabilizing;

        if (newState != state)
        {
            flashTimer = 0f;
        }

        state = newState;

        flashTimer += Time.deltaTime;
        text.text = "";
        if (state == State.Stabilizing) text.text = "stabilizers engaged";
        if (state == State.Mining) text.text = "ore extraction in progress";
    }
}

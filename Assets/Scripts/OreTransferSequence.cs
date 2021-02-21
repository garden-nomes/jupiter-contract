using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreTransferSequence : MonoBehaviour
{
    enum OreTransferState
    {
        Idle,
        StationInRange,
        TransferringOre,
        Time,
    }

    public ShipController ship;

    public float transferRate = 200f;
    public float stationInRangeDuration = 4f;
    public float minimumTimeDuration = 4f;
    public float transferringOreTimePadding = 1f;

    public GameObject stationInRange;
    public GameObject transferringOre;
    public GameObject time;

    public bool IsRunning => state != OreTransferState.Idle;

    private OreTransferState state = OreTransferState.Idle;
    private float timer = 0f;

    void Start()
    {
        stationInRange.SetActive(false);
        transferringOre.SetActive(false);
        time.SetActive(false);
    }

    private void Update()
    {

        if (state == OreTransferState.StationInRange)
        {
            timer += Time.deltaTime;
            if (timer > stationInRangeDuration)
            {
                timer = 0f;
                state = OreTransferState.TransferringOre;
                stationInRange.SetActive(false);
                transferringOre.SetActive(true);
            }
        }
        else if (state == OreTransferState.TransferringOre)
        {
            if (timer < transferringOreTimePadding)
            {
                timer += Time.deltaTime;
            }
            else if (ship.ore > 0f)
            {
                ship.ore -= transferRate * Time.deltaTime;
                if (ship.ore < 0f) ship.ore = 0f;
            }
            else
            {
                timer += Time.deltaTime;

                if (timer >= transferringOreTimePadding * 2f)
                {
                    state = OreTransferState.Time;
                    timer = 0f;
                    transferringOre.SetActive(false);
                    time.SetActive(true);
                }
            }
        }
        else if (state == OreTransferState.Time)
        {
            timer += Time.deltaTime;
        }
    }

    public void StartSequence()
    {
        state = OreTransferState.StationInRange;
        timer = 0f;
        stationInRange.SetActive(true);
    }

    public void Cancel()
    {
        if (state == OreTransferState.Time && timer > minimumTimeDuration)
        {
            state = OreTransferState.Idle;
            ship.ResetContract();
            time.SetActive(false);
        }
    }
}

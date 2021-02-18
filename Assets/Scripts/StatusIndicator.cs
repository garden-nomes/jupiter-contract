using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusIndicator : MonoBehaviour
{
    public GameObject okLight;
    public GameObject notOkLight;
    public bool isOk;

    void Update()
    {
        if (isOk)
        {
            okLight.SetActive(true);
            notOkLight.SetActive(false);
        }
        else
        {
            okLight.SetActive(false);
            notOkLight.SetActive(true);
        }
    }
}

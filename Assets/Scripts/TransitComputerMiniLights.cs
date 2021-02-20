using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitComputerMiniLights : MonoBehaviour
{
    public TransitComputer transitComputer;

    public GameObject distAlignLight;
    public GameObject speedAccLight;
    public GameObject autobrakeLight;
    public GameObject engFailureLight;

    void LateUpdate()
    {
        distAlignLight.SetActive(
            transitComputer.distText.text != "" || transitComputer.alignText.text != "");
        speedAccLight.SetActive(
            transitComputer.speedText.text != "" || transitComputer.accText.text != "");
        autobrakeLight.SetActive(transitComputer.autobrakeEngagedLight.gameObject.activeSelf);
        engFailureLight.SetActive(transitComputer.engFailureLight.gameObject.activeSelf);
    }
}

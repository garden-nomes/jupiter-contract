using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitComputerMiniLights : MonoBehaviour
{
    public TransitComputer transitComputer;

    public GameObject distAlignLight;
    public GameObject speedAccLight;
    public GameObject stage0Light;
    public GameObject stage1Light;

    void LateUpdate()
    {
        distAlignLight.SetActive(
            transitComputer.distText.text != "" || transitComputer.alignText.text != "");
        speedAccLight.SetActive(
            transitComputer.speedText.text != "" || transitComputer.accText.text != "");
        stage0Light.SetActive(transitComputer.stage0Light.gameObject.activeSelf);
        stage1Light.SetActive(transitComputer.stage1Light.gameObject.activeSelf);
    }
}

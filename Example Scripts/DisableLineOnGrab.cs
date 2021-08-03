using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableLineOnGrab : MonoBehaviour
{
    private XRInteractorLineVisual line;

    void Start()
    {
        line = GetComponent<XRInteractorLineVisual>();
    }

    public void DisableVisualOnGrab()
    {
        line.enabled = false;
    }

    public void EnableVisualOnRelease()
    {
        line.enabled = true;
    }
}

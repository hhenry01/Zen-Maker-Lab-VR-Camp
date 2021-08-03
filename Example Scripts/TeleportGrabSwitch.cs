using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TeleportGrabSwitch : MonoBehaviour
{
    // Allows dropdown selection of whether this script is put on the left or right controller
    // from the Unity Editor.
    public enum LeftOrRight
    {
        None,
        Left,
        Right
    };

    public LeftOrRight Controller;

    public bool Teleport = true; 
    
    private XRIDefaultInputActions controls;
    private XRRayInteractor Ray;
    private LayerMask originalLayers;
    private LayerMask grabLayer;

    // Awake executes BEFORE the Start() function. This is necessary to setup the event handlers.
    private void Awake()
    {
        if (Controller == LeftOrRight.None)
            throw new System.ArgumentException("Specify Left or Right Controller");

        // Need to create a new instance of our controls class as a workaround for a bug
        controls = new XRIDefaultInputActions();
        // Default button to switch between teleport and grab is the primary face button
        /*
         * The following code has weird syntax, but it adds our SwitchAction() functionto the event handler queue.
         * Essentially, the event handler responds to our inputs when they happen, and 
         * does not innefficiently poll like with the old input system. "ctx" is an unused
         * variable in this case, and is not important.
         */
        if (Controller == LeftOrRight.Left)
            controls.XRILeftHand.TeleportModeActivate.performed += ctx => SwitchAction();
        else
            controls.XRIRightHand.TeleportModeActivate.performed += ctx => SwitchAction();

        
    }
    // Start is called before the first frame update
    void Start()
    {
        // Get the original bitmask for the ray interactor and save it.
        Ray = GetComponent<XRRayInteractor>();
        originalLayers = Ray.raycastMask.value;
        // Acquire the bit position of the grab layer.
        grabLayer = LayerMask.NameToLayer("Grab");
        Debug.Log("Grab layer: " + grabLayer.value);

        if (Teleport)
            Ray.lineType = XRRayInteractor.LineType.ProjectileCurve;
    }

    public void SwitchAction()
    {
        Debug.Log("Switched action");
        Teleport = !Teleport;
        if (Teleport)
        {
            Ray.raycastMask = originalLayers;
            Debug.Log("Layer: " + Ray.raycastMask.value);
            Ray.lineType = XRRayInteractor.LineType.ProjectileCurve;
        }
        else
        {
            // Switch our raycast mask to grab only by bit shifting 1 with its bit position.
            Ray.raycastMask = 1 << grabLayer;
            Debug.Log("Layer: " + Ray.raycastMask.value);
            Ray.lineType = XRRayInteractor.LineType.StraightLine;
        }
    }

    // These two functions are IMPORTANT!!!
    // Will not work without them.
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


}

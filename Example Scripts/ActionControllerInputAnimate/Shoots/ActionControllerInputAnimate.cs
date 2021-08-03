using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActionControllerInputAnimate : MonoBehaviour
{

    [Header("Controller Model - Overrides model in XR Controller Component")]
    public GameObject Model = null;
    public bool isHand = false; // In the Unitor Editor, mark if the above model is an animated hand model

    private ActionBasedController actionController;
    private Animator handAnimator;
    private float gripAction = 0;
    private float trigAction = 0;

    [Header("For shooting projectiles when unarmed")]
    public float speed = 40;
    public GameObject projectile;
    public float lifeTime = 2;
    public AudioSource audioSource = null;
    public AudioClip audioClip = null;

    // Start is called before the first frame update
    void Start()
    {
        if (!Model)
            throw new System.ArgumentException("Provide a model in the ActionControllerInput prefab");

        // Access the Action Based Controller component of our controller
        actionController = GetComponent<ActionBasedController>();
        // Replace whatever model it has with our prefab model.
        // The .transform is necessary as actionController.model is apparently
        // not a model, but a transform
        actionController.model = Instantiate(Model, transform).transform;

        if (isHand)
        {
            handAnimator = actionController.model.GetComponent<Animator>();
            if (!handAnimator)
                throw new System.ArgumentException("Controller marked as hand but has no animations");

            // The following four lines are event handlers that wait for
            // when a button is pressed or released respectively. When 
            // the events are triggered, their corresponding functions
            // are added to Unity's action queue.
            actionController.selectAction.action.started += gripPressed;
            actionController.selectAction.action.canceled += gripReleased;
            actionController.activateAction.action.started += trigPressed;
            actionController.activateAction.action.canceled += trigReleased;
        }

    }

    /*
     * The following four functions serve similar purposes. They are activated
     * whenever their button event happens and set the animation of the hand model
     * accordingly using relevant float values.
     */
    private void trigReleased(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Trigger Released");
        handAnimator.SetFloat("Trigger", 0);
    }

    private void gripReleased(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Grip Released");
        handAnimator.SetFloat("Grip", 0);
    }

    private void gripPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gripAction = actionController.selectAction.action.ReadValue<float>();
        Debug.Log("Grip Pressed: " + gripAction);
        handAnimator.SetFloat("Grip", gripAction);
    }

    private void trigPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        trigAction = actionController.activateAction.action.ReadValue<float>();
        Debug.Log("Trigger Pressed: " + trigAction);
        handAnimator.SetFloat("Trigger", trigAction);
        GameObject spawnProjectile = Instantiate(projectile, actionController.model.transform.position, actionController.model.transform.rotation);
        spawnProjectile.GetComponent<Rigidbody>().velocity = speed * actionController.model.transform.forward;

        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);
        Destroy(spawnProjectile, lifeTime);
    }

}

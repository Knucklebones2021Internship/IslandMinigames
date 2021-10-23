using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(Rigidbody))]
public class Scripts_MiniGolf_BallController_Zach : MonoBehaviour
{
    /*[SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject areaAffector;
    [SerializeField] private float maxForce, forceModifier;*/

    private float force;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("touch position: " + Scripts_InputManager_Wyatt.touchPosition.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        // when input first happens - for instance, 
        Scripts_InputManager_Wyatt.touchPress.started += Shoot;
    }

    private void OnDisable()
    {
        Scripts_InputManager_Wyatt.touchPress.started -= Shoot;
    }

    void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot");

        print("zach position: " + Scripts_InputManager_Wyatt.touchPosition.ReadValue<Vector2>());
        print("zach shoot!");
    }
}

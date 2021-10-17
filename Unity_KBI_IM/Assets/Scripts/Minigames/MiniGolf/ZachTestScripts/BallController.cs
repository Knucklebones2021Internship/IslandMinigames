using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public static BallController instance;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject areaAffector;
    [SerializeField] private float maxForce, forceModifier;

    private float force;
    private Rigidbody rb;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody>();

        Scripts_InputManager_Wyatt.EnableAttitudeSensor();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Scripts_InputManager_Wyatt.touchPosition.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        // when input first happens - for instance, 
        Scripts_InputManager_Wyatt.touchPress.started += Shoot;
    }

    private void OnDisable()
    {
        
    }

    void Shoot(InputAction.CallbackContext context)
    {

    }
}

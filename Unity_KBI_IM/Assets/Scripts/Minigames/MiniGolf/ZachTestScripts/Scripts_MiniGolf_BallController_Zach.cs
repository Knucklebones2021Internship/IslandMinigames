using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Scripts_MiniGolf_BallController_Zach : MonoBehaviour
{
    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = 0.05f;

    [SerializeField] private LineRenderer lineRenderer;

    private bool isIdle;
    private bool isAiming;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        isAiming = false;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < stopVelocity)
            Stop();

        if (isIdle)
            if (Input.touchCount > 0)
                isAiming = true;

        ProcessAim();
    }

    /*private void OnMouseDown()
    {
        if (isIdle)
            isAiming = true;
    }*/

    private void ProcessAim()
    {
        if (!isAiming || !isIdle)
            return;

        Vector3? worldPoint = CastMouseClickRay();

        // Debug.Log("World Point/Ray: " + worldPoint);

        if (!worldPoint.HasValue)
            return;

        DrawLine(worldPoint.Value);

        if (Input.touchCount > 0)
            if (Input.touches[0].phase == TouchPhase.Ended)
                Shoot(worldPoint.Value);
    }

    private void Shoot(Vector3 worldPoint)
    {
        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        rb.AddForce(direction * strength * shotPower);

        isIdle = false;
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = { transform.position, worldPoint };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private Vector3? CastMouseClickRay()
    {
        /*Vector3 screenMousePositionFar = new Vector3(
            Input.mousePosition.x,                                                                  // input
            Input.mousePosition.y,                                                                  // input
            Camera.main.farClipPlane);
        Vector3 screenMousePositionNear = new Vector3(
            Input.mousePosition.x,                                                                  // input
            Input.mousePosition.y,                                                                  // input
            Camera.main.nearClipPlane);*/

        if (Input.touchCount > 0)
        {
            Vector3 screenMousePositionFar = new Vector3(
                Input.GetTouch(0).position.x,
                Input.GetTouch(0).position.y,
                Camera.main.farClipPlane);
            Vector3 screenMousePositionNear = new Vector3(
                Input.GetTouch(0).position.x,
                Input.GetTouch(0).position.y,
                Camera.main.nearClipPlane);
                 Vector3 worldMousePositionFar = Camera.main.ScreenToWorldPoint(screenMousePositionFar);
            Vector3 worldMousePositionNear = Camera.main.ScreenToWorldPoint(screenMousePositionNear);
            RaycastHit hit;
            if (Physics.Raycast(worldMousePositionNear, worldMousePositionFar - worldMousePositionNear, out hit, float.PositiveInfinity))
                return hit.point;
            else
                return null;
        }
        return null;
    }
}

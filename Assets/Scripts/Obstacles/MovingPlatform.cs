using System.Collections;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float maxSpeed = 5f;
    public float waitTime = 1f;
    public float brakeDistance = 2f;

    private Transform target;
    private bool isWaiting = false;
    private Rigidbody rb;

    void Start()
    {
        target = pointB;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isWaiting)
            return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Calculate speed scaling
        float speedMultiplier = Mathf.Clamp01(distanceToTarget / brakeDistance);
        float currentSpeed = maxSpeed * speedMultiplier;

        // Move using Rigidbody
        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPos);

        // Reached target?
        if (distanceToTarget < 0.05f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        target = (target == pointA) ? pointB : pointA;
        isWaiting = false;
    }
}

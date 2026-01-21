using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    private Vector3 movement;

    private Rigidbody rb;
    public GameObject poker;
    public Camera playerCamera;

    public LayerMask garbage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // On left click
        if (Input.GetMouseButtonDown(0))
        {
            // TEMP raycast checking
            CheckGarbage();
            // TODO:
            // Add different actions for different raycasts
            // only check one, return what it hits, then switch statement for what happens
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Create two temp float variables to hold inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        movement = (transform.forward * v * moveSpeed) + (transform.right * h * moveSpeed);

        // Normalize so the movement doesn't get added incorrectly
        movement = Vector3.ClampMagnitude(movement, moveSpeed);

        // Take current position, add the calc above
        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }

    private void CheckGarbage()
    {
        RaycastHit hit;

        // Sends a ray to what the player is looking at. If it's a piece of garbage, delete it
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f, garbage))
        {
            // If collision, destroy the object it hit
            Destroy(hit.collider.gameObject);
        }
    }
}

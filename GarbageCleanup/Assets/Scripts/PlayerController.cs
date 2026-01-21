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

    public LayerMask garbage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckGarbage();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Create two temp float variables to hold inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Normalize so the movement doesn't get added incorrectly
        movement.Normalize();

        movement = (transform.forward * v * moveSpeed) + (transform.right * h * moveSpeed);

        // Take current position, add the calc above
        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }

    private void CheckGarbage()
    {
        RaycastHit hit;

        if (Physics.Raycast(poker.transform.position, Vector3.forward, out hit, 100f, garbage))
        {
            Debug.Log("Hit");
            // If collision, return true
            Destroy(gameObject);
        }
    }
}

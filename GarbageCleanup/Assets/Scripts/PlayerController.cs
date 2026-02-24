using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector3 movement;
    private Rigidbody rb;
    private bool isGrounded;
    public LayerMask ground;
    public float jumpHeight = 10f;

    [Header("Raycasting")]
    public GameObject poker;
    public Camera playerCamera;
    public LayerMask garbage;
    private Vector3 boxSize = new Vector3(0.5f, 0.3f, 0.5f);

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        rb = GetComponent<Rigidbody>();

        //scoreText.text = $"Current Garbage: {score}";
    }

    private void Update()
    {
        isGrounded = IsPlayerGrounded();

        // On left click
        if (Input.GetMouseButtonDown(0))
        {
            // TEMP raycast checking
            CheckGarbage();
            // TODO:
            // Add different actions for different raycasts
            // only check one, return what it hits, then switch statement for what happens
        }

        // Jumping code
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            isGrounded = false;
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

    private bool IsPlayerGrounded()
    {
        if (Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, 1f, ground))
        {
            return true;
        }

        return false;
    }

    // Draws the BoxCast used for jump detection
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position - transform.up * 1f, boxSize);
    //}

    private void Jump()
    {
        // Make the player jump
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    private void CheckGarbage()
    {
        RaycastHit hit;

        // Sends a ray to what the player is looking at. If it's a piece of garbage, delete it
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f, garbage))
        {
            // If collision, destroy the object it hit
            Destroy(hit.collider.gameObject);

            // Update score
            score++;
            scoreText.text = $"Current Garbage: {score}";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public Camera playerCamera;
    public LayerMask garbage;
    private Vector3 boxSize = new Vector3(0.5f, 0.3f, 0.5f);
    private float sphereRadius = 10f;

    [Header("Poker")]
    public GameObject poker;
    public GameObject pokerExtension;
    public LayerMask grapplePoint;
    private float pokerRange = 5f;
    private bool isPokerExtended;
    public bool isPlayerGrappled { get; private set; }
    private GameObject currentGrapplePoint;
    private float pokerCounter = 0;
    private float pokerCooldown = 0.5f;

    public Material blackMaterial;
    public Material yellowMaterial;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    private int score;

    [Header("Animation")]
    public Animator animator;

    [Header("Inventory")]
    public InventoryController inventory;

    [Header("Bins")]
    public LayerMask binLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        isPokerExtended = false;

        // scoreText.text = $"Current Garbage: {score}";
    }

    private void Update()
    {
        isGrounded = IsPlayerGrounded();
        //CheckIfGrappleIsNear();

        // On left click, check if player is close enough to garbage
        if (Input.GetMouseButtonDown(0))
        {
            CheckGarbage();
        }

        // Pole extending and retracting on right click
        if (Input.GetMouseButtonDown(1))
        {
            // If the poker gets extended, check if it hits a grapple point
            // Only allow poker state change if the cooldown is over
            if (!isPokerExtended && pokerCounter >= pokerCooldown)
            {
                animator.Play("PokerExtend");
                isPokerExtended = !isPokerExtended;
                pokerCounter = 0f;

                // Extend poker range
                pokerRange = 10f;
                // Determine if the player should be grappled
                CheckGrapple();
            }
            else if (isPokerExtended && pokerCounter >= pokerCooldown)
            {
                animator.Play("PokerRetract");
                isPokerExtended = !isPokerExtended;
                pokerCounter = 0f;

                // If the player was grappled, pull them toward the grapple point
                if (isPlayerGrappled)
                {
                    GrapplePull();
                }

                // Reset poker range
                pokerRange = 5f;
                isPlayerGrappled = false;
            }
        }

        // Jumping code
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            isGrounded = false;
        }

        // When to player gets too far from the grapple point, auto retract the poker
        if (isPlayerGrappled && isPlayerTooFar())
        {
            animator.Play("PokerRetract");
            pokerRange = 5f;
            isPlayerGrappled = false;
        }

        pokerCounter += Time.deltaTime;
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

    private bool isPlayerTooFar()
    {
        // Check how far the player is from the current grapple point
        float dist = Vector3.Distance(currentGrapplePoint.transform.position, transform.position);

        // When the player is too far, return true
        if (dist >= 15f)
        {
            return true;
        }

        return false;
    }

    private void CheckIfGrappleIsNear()
    {
        var colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == "GrapplePoint")
            {
                // Add colour/material changing
                collider.gameObject.GetComponent<MeshRenderer>().material = yellowMaterial;
            }
        }
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

    private void GrapplePull()
    {
        // Pull the player towards the gameObject they are "grappled" to
        rb.AddForce((currentGrapplePoint.transform.position - transform.position) * 2, ForceMode.Impulse);
    }

    private void CheckGarbage()
    {
        RaycastHit hit;

        // Raycast forward to find garbage
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pokerRange, garbage))
        {
            if (inventory == null)
            {
                Debug.LogError("InventoryController not assigned on PlayerController.");
                return;
            }

            // Try add to inventory instead of destroying
            bool added = inventory.TryAdd(hit.collider.gameObject);

            // If you still want score for pickup, do it here (optional)
            // if (added) score++;

            return;
        }

        // If we didn't hit garbage, we can check bins with the same left click
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pokerRange, binLayer))
        {
            var bin = hit.collider.GetComponentInParent<BinController>();
            if (bin != null)
            {
                bin.TryDeposit(inventory);
            }
            else
            {
                Debug.Log("[Bin] Hit a bin object but no BinController found.");
            }
        }
    }

    private void CheckGrapple()
    {
        RaycastHit hit;

        // Sends a ray to what the player is looking at. If it's a piece of garbage, delete it
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pokerRange, grapplePoint))
        {
            isPlayerGrappled = true;
            currentGrapplePoint = hit.collider.gameObject;
        }
    }
}

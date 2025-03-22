using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private float inputH;
    private float inputV;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    //continue physics - time based
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(inputH, 0, inputV) * movementForce, ForceMode.Force);

    }
}

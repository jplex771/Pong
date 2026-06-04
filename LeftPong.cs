using UnityEngine;
using UnityEngine.InputSystem;

public class LeftPong : MonoBehaviour
{
    public float scale = 3f;
    public Vector2 movement = new Vector2(0, 0);

    private bool isMoving = false;
    private PlayerInput playerInput;
    private InputActionMap player1;
    private InputAction leftMove;
    private Vector3 position;
    private Quaternion rotation;
    private Rigidbody2D rb;

    // not changing var (technically not const)
    Vector2 stopped = new Vector2(0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();

        player1 = playerInput.actions.FindActionMap("Player1");
        leftMove = playerInput.actions.FindAction("LeftMove");
    }


    void FixedUpdate()
    {
        // Movement control
        if (leftMove.IsPressed())
        {
            isMoving = true;
            rb.linearVelocity = movement;
        }
        else
        {
            isMoving = false;
            rb.linearVelocity = stopped;
        }

        // Bounds
        transform.GetPositionAndRotation(out position, out rotation);
        if (position.y > 3.5) // upper bound
        {
            position.y = 3.5f;
            transform.SetPositionAndRotation(position, rotation);
        }
        else if (position.y < -3.5) // lower bound
        {
            position.y = -3.5f;
            transform.SetPositionAndRotation(position, rotation);
        }
    }

    void OnLeftMove(InputValue value)
    {
        // Debug.Log("Left Pong moves" + value.Get<Vector2>());
        movement = value.Get<Vector2>() * scale;

    }

    // Getter
    public bool getIsMoving()
    {
        return isMoving;
    }

}

using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class Spawner : MonoBehaviour
{

    // Direction and speed vars
    enum Direction { Left, Right };
//    private float lSpeed = -12f;
//    private float rSpeed = 12f;

    // Prefab
    [SerializeField] private GameObject ball;

    // Score and checker vars
    [SerializeField] private TMP_Text leftScore;
    [SerializeField] private TMP_Text rightScore;
    private int leftScoreCheck = 0;
    private int rightScoreCheck = 0;

    // Event info
    private static int left = -1;
    private static int right = 1;

    private Rigidbody2D rb;
    private Vector2 vel = new Vector2(0, 0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = ball.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Respawn
    private async Task respawnBall(Direction direction)
    {
        vel = rb.linearVelocity;
        Debug.Log("velocity = " + vel);

        // Slight Delay
        await Task.Delay(250);

        // Spawn
        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.identity;
        rb.transform.SetPositionAndRotation(position, rotation);
        //vel.x *= -1;
        rb.linearVelocity = vel;

/*
        if (direction == Direction.Left)
        {
            // Set Velocity
            Vector2 vel = new Vector2(lSpeed, 0);
            rb.linearVelocity = vel;
        }
        else if (direction == Direction.Right)
        {
            Vector2 vel = new Vector2(rSpeed, 0);
            rb.linearVelocity = vel;
        }
        */
    }

    // Event Subscriptions
    private void OnEnable()
    {
        ScoreKeeper.OnScored += HandleScore;
    }
    private void OnDisable()
    {
        ScoreKeeper.OnScored -= HandleScore;
    }

    // Event Handlers
    private async void HandleScore(int direction)
    {
        if (direction == left)
        {
            leftScoreCheck = int.Parse(leftScore.text);
            await respawnBall(Direction.Left);
        }
        else if (direction == right)
        {
            rightScoreCheck = int.Parse(rightScore.text);
            await respawnBall(Direction.Right);
        }
        
    }
}

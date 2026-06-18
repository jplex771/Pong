using UnityEngine;
using System;

namespace Pong.Models
{

    public class Ball : MonoBehaviour
    {

        public const float VELOCITY = 14f;
        private Rigidbody2D rb;
        private Vector2 currentVel = new Vector2(0, 0);
        private bool xIsNeg;
        private bool yIsNeg;
        private bool yIsZero;
        private const float MIN_X = .4f * VELOCITY;
        private const float MAX_X = VELOCITY;
        private const float SCALER = .4f;


        // Event
        public static event Action<int> OnBallScored;
        //    public static event Action OnBallDestroyedR; // --------------------<>
        //Event info
        private static int left = -1;
        private static int right = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            currentVel.x = VELOCITY * (UnityEngine.Random.Range(0, 2) * 2 - 1);
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = currentVel;

            rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            //if (currentVel.x != 0 && currentVel.y != 0)
            
            
        }

        // Bounce management
        void OnTriggerEnter2D(Collider2D coll)
        {
            // Score
            if (coll.gameObject.tag == "LeftBound")
            {
                OnBallScored?.Invoke(left);
                return;
            }
            else if (coll.gameObject.tag == "RightBound")
            {
                OnBallScored?.Invoke(right);
                return;
            }

            // Update currentVel
            currentVel.x = rb.linearVelocityX;
            currentVel.y = rb.linearVelocityY;
            // Save x and y sign
            xIsNeg = currentVel.x < 0 ? true : false;
            yIsNeg = currentVel.y < 0 ? true : false;
            yIsZero = currentVel.y == 0 ? true : false;

            
            Debug.Log("Before Bounce\nX Vel = " + rb.linearVelocityX + " | Y Vel = " + rb.linearVelocityY);
            //float currX = currentVel.x;
            if (coll.gameObject.tag == "Pong")
            {
                Debug.Log("Bounced off paddle");
                // Rigidbody abstraction
                Rigidbody2D paddle = coll.gameObject.GetComponent<Rigidbody2D>();
                bool paddleYIsNeg = paddle.linearVelocityY < 0 ? true : false;
                bool paddleYIsZero = paddle.linearVelocityY == 0 ? true : false;

                // Adjust angle 
                bounceOffPaddle(paddle, paddleYIsNeg, paddleYIsZero);
                rb.linearVelocity = currentVel;
            }
            if (coll.gameObject.tag == "Wall")
            {
                Debug.Log("Bounced off Wall");
                currentVel.y *= -1;
                rb.linearVelocity = currentVel;
            }
            Debug.Log("After Bounce\nX Vel = " + rb.linearVelocityX + " | Y Vel = " + rb.linearVelocityY);
         }

        // --- Helper Methods --- //
        
        /*
        * Bounces the ball off {@Code paddle}
        * Also deals with angle changes from paddle movement.
        */
        private void bounceOffPaddle(Rigidbody2D paddle, bool paddleYIsNeg, bool paddleYIsZero)
        {
            // If ball Y = 0 and paddle is moving
            if (yIsZero && !paddleYIsZero)
            {
                conditionZero(paddle);
            }
            // if the ball is moving in the same Y direction as the paddle
            else if (((yIsNeg && paddleYIsNeg) || (!yIsNeg && !paddleYIsNeg)) && !paddleYIsZero)
            {
                conditionOne(paddle);
            }
            // if the ball is moving in a different Y direction as the paddle
            else if (((yIsNeg && !paddleYIsNeg) || (!yIsNeg && paddleYIsNeg)) && !paddleYIsZero)
            { // Needs looking at
                conditionTwo(paddle);
            }
            // If the ball Y = 0 and paddle is not moving
            else if (yIsZero && paddleYIsZero)
            {
                Debug.Log("3rd Condition Called");
                currentVel.x *= -1;
                xIsNeg = !xIsNeg;
            }
            // If the ball is moving at an angle and the paddle is not moving
            else if (!yIsZero && paddleYIsZero)
            {
                Debug.Log("4th Condition Called");
                currentVel.x *= -1;
                xIsNeg = !xIsNeg;
            }
        }

        // Conditions for adjusting the angle of the ball when it bounces off the paddle
        /*
        * Used if the ball is moving straight (velocity.y = 0) and the paddle is moving.
        */
        private void conditionZero(Rigidbody2D paddle)
        {
            Debug.Log("0th Condition");
            currentVel.y = Math.Abs(currentVel.y) + Math.Abs(paddle.linearVelocityY * SCALER);
            // Adjust x
            currentVel.x = (xIsNeg ? (currentVel.x + currentVel.y) : (currentVel.x - currentVel.y)) * -1;
            // Y's sign
            currentVel.y *= yIsNeg ? -1 : 1;
        }

        /*
        * Used if the ball is is moving at an angle (velocity.y != 0) and the paddle is moving in the same Y direction.
        */
        private void conditionOne(Rigidbody2D paddle) {
            Debug.Log("1st Condition Called");
            currentVel.y = Math.Abs(currentVel.y) + Math.Abs(paddle.linearVelocityY * SCALER);
            currentVel.x = Math.Abs(currentVel.x) - Math.Abs(paddle.linearVelocityY * SCALER);
            Debug.Log("Angle Ajustment Step 1:\nX cVel " + currentVel.x + " | Y cVel = " + currentVel.y);
            if (currentVel.x < MIN_X) // Ball from going verticle
            {
                Debug.Log("Max Angle attained");
                currentVel.y -= MIN_X - currentVel.x;
                currentVel.x = MIN_X;
            }
            if (yIsNeg)
            {
                currentVel.y *= -1;
            }
            if (!xIsNeg)
            {
                currentVel.x *= -1;
                xIsNeg = true;
            }
        }

        /*
        * Used if the ball is moving at an angle and the paddle is moving in the opposite Y direction.
        */
        private void conditionTwo(Rigidbody2D paddle) {
            Debug.Log("2nd Condition Called");
            currentVel.y = Math.Abs(currentVel.y) - Math.Abs(paddle.linearVelocityY * SCALER);
            currentVel.x = Math.Abs(currentVel.x) + Math.Abs(paddle.linearVelocityY * SCALER);
            if (currentVel.y < 0)
            {
                currentVel.x = MAX_X - currentVel.y;
            }
            if (!xIsNeg)
            {
                currentVel.x *= -1;
                xIsNeg = true;
            }
        }
    }


    
}
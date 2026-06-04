using UnityEngine;
using System;

namespace Pong.Models
{

    public class Ball : MonoBehaviour
    {

        private float velocity = 12f;
        private Rigidbody2D rb;
        private Vector2 currentVel = new Vector2(0, 0);


        // Event
        public static event Action<int> OnBallScored;
        //    public static event Action OnBallDestroyedR; // --------------------<>
        //Event info
        private static int left = -1;
        private static int right = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            currentVel.x = velocity * (UnityEngine.Random.Range(0, 2) * 2 - 1);
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = currentVel;

            rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Bounce management
        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Pong")
            {
                currentVel.x *= -1;
                // Adjust angle
                currentVel.y += coll.gameObject.GetComponent<Rigidbody2D>().linearVelocityY * .8f;
                currentVel.x -= coll.gameObject.GetComponent<Rigidbody2D>().linearVelocityY * .2f;
                rb.linearVelocity = currentVel;
            }
            if (coll.gameObject.tag == "Wall")
            {
                currentVel.y *= -1;
                rb.linearVelocity = currentVel;
            }

        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            // Score
            if (coll.gameObject.tag == "LeftBound")
            {
                OnBallScored?.Invoke(left);
            }
            else if (coll.gameObject.tag == "RightBound")
            {
                OnBallScored?.Invoke(right);
            }
         }
    }
}
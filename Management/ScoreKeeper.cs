using UnityEngine;
using TMPro;
using System;
using Pong.Models;

namespace Pong.Management
{

    public class ScoreKeeper : MonoBehaviour
    {
        // UI elements
        [SerializeField] private TMP_Text leftScore;
        [SerializeField] private TMP_Text rightScore;

        // Score Update Events
        public static event Action<int> OnScored;
        //public static event Action OnRightScored; // ---------<>

        // Event info
        private static int left = -1;
        private static int right = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // Subscriptions
        private void OnEnable()
        {
            Ball.OnBallScored += HandleDestroyed;
        }

        // Clean up Subscriptions
        private void OnDisable()
        {
            Ball.OnBallScored -= HandleDestroyed;
        }

        // Event Handlers
        private void HandleDestroyed(int direction) // 
        {
            if (direction == right)
            {
                leftScore.text = (int.Parse(leftScore.text) + 1).ToString();
                OnScored?.Invoke(left);
                //Debug.Log("Left Score Bumped");
            }
            else if (direction == left)
            {
                rightScore.text = (int.Parse(rightScore.text) + 1).ToString();
                OnScored?.Invoke(right);
                //Debug.Log("Right Score Bumped");
            }
        }
    }
}
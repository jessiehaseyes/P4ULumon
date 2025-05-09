using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    /// <summary>
    /// Character automatically moves towards target transform
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AutoMoveToTarget : MonoBehaviour
    {
        NavMeshAgent m_Agent;
        private Animator m_Animator;
        public Transform targetTrans;
        
        [Header("Navigation Settings")]
        [Tooltip("The movement speed of the agent")]
        public float movementSpeed = 3.5f;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
            
            // Set initial speed from the public variable
            if (m_Agent != null)
            {
                m_Agent.speed = movementSpeed;
            }
            
            // Validate that target transform is assigned
            if (targetTrans == null)
            {
                Debug.LogError("Target Transform not assigned! Please assign a target in the Inspector.");
            }
        }

        void Update()
        {
            // Continuously set the agent's destination to the target position
            if (targetTrans != null)
            {
                m_Agent.destination = targetTrans.position;
                
                // Optional: Set animator parameter if character has walking animation
                if (m_Animator != null)
                {
                    m_Animator.SetBool("isWalking", true);
                }
            }
        }

        void OnAnimatorMove()
        {
            // Only override speed from animator if we're not using the public speed setting
            if (m_Animator != null && m_Animator.GetBool("isWalking"))
            {
                // You can comment this line out if you want to always use the public speed
                // m_Agent.speed = (m_Animator.deltaPosition / Time.deltaTime).magnitude;
            }
        }
        
        // Public method to change speed at runtime
        public void SetSpeed(float newSpeed)
        {
            movementSpeed = newSpeed;
            if (m_Agent != null)
            {
                m_Agent.speed = movementSpeed;
            }
        }
    }
}
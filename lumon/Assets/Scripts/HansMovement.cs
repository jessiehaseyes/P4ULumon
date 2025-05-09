using UnityEngine;
using System.Collections;

public class HansMovement : MonoBehaviour
{
    // Animation variables
    public Animator animator;
    private int _isWalkingHash;
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;
    private Vector3 _lastPosition;
    private Vector3 _actualMoveDirection;
    
    // Movement variables
    public float speed = 0.1f;
    public Vector3 startPos;
    public Vector3 endPos;
    public Transform targetTrans;
    
    // Obstacle avoidance variables
    [SerializeField] private float raycastDistance = 1.0f;
    [SerializeField] private float obstacleAvoidanceTime = 1.5f;
    [SerializeField] private float avoidanceSpeed = 0.15f;
    [SerializeField] private LayerMask obstacleLayer = -1; // Default to all layers
    private bool _isAvoidingObstacle = false;
    private Vector3 _avoidanceDirection;
    
    private CharacterController _characterController;

    void Start()
    {
        // Initialize components
        animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        
        // Animation setup
        _isWalkingHash = Animator.StringToHash("isWalking");
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
        
        // Movement setup
        _lastPosition = transform.position;
        startPos = transform.position;
    }

    void Update()
    {
        // Track actual movement for animation
        _actualMoveDirection = transform.position - _lastPosition;
        _actualMoveDirection.y = 0;
        
        // Handle movement based on obstacle detection
        if (!_isAvoidingObstacle)
        {
            MoveTowardsTarget();
        }
        else
        {
            AvoidObstacle();
        }
        
        // Handle character facing and animation
        HandleRotation();
        HandleAnimation();
        
        _lastPosition = transform.position;
    }
    
    private void MoveTowardsTarget()
    {
        if (targetTrans != null)
        {
            // Get direction to target
            endPos = targetTrans.position;
            Vector3 directionToTarget = endPos - transform.position;
            directionToTarget.y = 0;
            directionToTarget.Normalize();
            
            // Check for obstacles using multiple raycasts
            bool obstacleDetected = DetectObstacle(directionToTarget);
            
            if (obstacleDetected)
            {
                // Calculate avoidance direction based on obstacle position
                CalculateAvoidanceDirection(directionToTarget);
                StartCoroutine(AvoidObstacleCoroutine());
            }
            else
            {
                // No obstacle, move directly toward target
                MoveInDirection(directionToTarget, speed);
            }
        }
    }
    
    private bool DetectObstacle(Vector3 directionToTarget)
    {
        // Main forward raycast
        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, raycastDistance, obstacleLayer))
        {
            if (hit.transform != targetTrans)
            {
                return true;
            }
        }
        
        // Additional raycasts at slight angles to detect obstacles to the sides
        Vector3 slightlyRight = Quaternion.Euler(0, 30, 0) * directionToTarget;
        if (Physics.Raycast(transform.position, slightlyRight, raycastDistance, obstacleLayer))
        {
            return true;
        }
        
        Vector3 slightlyLeft = Quaternion.Euler(0, -30, 0) * directionToTarget;
        if (Physics.Raycast(transform.position, slightlyLeft, raycastDistance, obstacleLayer))
        {
            return true;
        }
        
        return false;
    }
    
    private void CalculateAvoidanceDirection(Vector3 directionToTarget)
    {
        // Check which side is more open using raycasts
        Vector3 rightDirection = Quaternion.Euler(0, 90, 0) * directionToTarget;
        Vector3 leftDirection = Quaternion.Euler(0, -90, 0) * directionToTarget;
        
        float rightDistance = 10f;
        float leftDistance = 10f;
        
        // Cast rays to both sides to see which has more space
        if (Physics.Raycast(transform.position, rightDirection, out RaycastHit rightHit, 10f, obstacleLayer))
        {
            rightDistance = rightHit.distance;
        }
        
        if (Physics.Raycast(transform.position, leftDirection, out RaycastHit leftHit, 10f, obstacleLayer))
        {
            leftDistance = leftHit.distance;
        }
        
        // Choose the direction with more space
        if (rightDistance > leftDistance)
        {
            _avoidanceDirection = rightDirection;
        }
        else
        {
            _avoidanceDirection = leftDirection;
        }
    }
    
    private void AvoidObstacle()
    {
        // While in avoidance mode, move in the calculated avoidance direction
        MoveInDirection(_avoidanceDirection, avoidanceSpeed);
        
        // Continue checking if path to target is clear
        if (targetTrans != null)
        {
            Vector3 directionToTarget = (targetTrans.position - transform.position).normalized;
            directionToTarget.y = 0;
            
            // If no obstacle in the path to target, exit avoidance early
            if (!DetectObstacle(directionToTarget))
            {
                _isAvoidingObstacle = false;
                StopAllCoroutines();
            }
        }
    }
    
    private void MoveInDirection(Vector3 direction, float moveSpeed)
    {
        if (_characterController != null)
        {
            _characterController.Move(direction * (moveSpeed * Time.deltaTime));
        }
        else
        {
            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator AvoidObstacleCoroutine()
    {
        _isAvoidingObstacle = true;
        
        // Update rotation for avoidance direction
        var targetAngle = Mathf.Atan2(_avoidanceDirection.x, _avoidanceDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
        
        // Move in avoidance direction for the specified time
        yield return new WaitForSeconds(obstacleAvoidanceTime);
        
        // Return to normal movement
        _isAvoidingObstacle = false;
    }

    private void HandleRotation()
    {
        if (_isAvoidingObstacle)
        {
            // During avoidance, maintain the rotation toward the avoidance direction
            var targetAngle = Mathf.Atan2(_avoidanceDirection.x, _avoidanceDirection.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
        else if (targetTrans != null)
        {
            // During normal movement, face the target
            Vector3 direction = targetTrans.position - transform.position;
            direction.y = 0;
            
            if (direction.magnitude > 0.1f)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
                transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            }
        }
    }

    private void HandleAnimation()
    {
        bool isMoving = _actualMoveDirection.magnitude / Time.deltaTime > 0.1f;
        animator.SetBool(_isWalkingHash, isMoving);
    }
}
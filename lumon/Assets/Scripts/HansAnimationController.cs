using UnityEngine;

public class HansAnimationController : MonoBehaviour
{
    public Animator animator;
    
    private int _isWalkingHash;  
    
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;
    private CharacterController _characterController;
    private ChaseMovement _chaseMovement;
    private Vector3 _lastPosition;
    private Vector3 _actualMoveDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _chaseMovement = GetComponent<ChaseMovement>();
        
        _isWalkingHash = Animator.StringToHash("isWalking");
        
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
        
    
        _lastPosition = transform.position;
    }

    void Update()
    {
        _actualMoveDirection = transform.position - _lastPosition;
        _actualMoveDirection.y = 0; 
        HandleRotation();
        HandleAnimation();
        
        _lastPosition = transform.position;
    }

    private void HandleRotation()
    {
        if (_chaseMovement != null && _chaseMovement.targetTrans != null)
        {
           
            Vector3 direction = _chaseMovement.targetTrans.position - transform.position;
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
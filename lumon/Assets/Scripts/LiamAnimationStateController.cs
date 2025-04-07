using UnityEngine;

public class LiamAnimationStateController : MonoBehaviour
{
    public Animator animator;
    private int _isWalkingHash;  
    private int _isRunningHash;

    private Vector2 _input;
    private Vector3 _moveDirection;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 5f;
    private float _currentVelocity;
    private CharacterController _characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleAnimation();
        HandleMovement();
        HandleRotation();
    }

    private void HandleInput()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _input = new Vector2(horizontal, vertical).normalized;
        
        _moveDirection = new Vector3(_input.x, 0f, _input.y);
    }

    private void HandleAnimation()
    {
        bool isRunning = animator.GetBool(_isRunningHash);
        bool isWalking = animator.GetBool(_isWalkingHash);
        bool forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(_isWalkingHash, true);
        }

        if (isWalking && !forwardPressed)
        {
            animator.SetBool(_isWalkingHash, false);
        }

        if (!isRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(_isRunningHash, true);
        }

        if (isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(_isRunningHash, false);
        }
    }

    private void HandleMovement()
    {
        if (_input.sqrMagnitude == 0) return;
        
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * 2 : speed;
        Vector3 movement = _moveDirection * currentSpeed * Time.deltaTime;
        
      
        if (_characterController != null)
        {
            _characterController.Move(movement);
        }
        else
        {
            transform.position += movement;
        }
    }

    private void HandleRotation()
    {
        if (_input.sqrMagnitude == 0) return;
        
        var targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}
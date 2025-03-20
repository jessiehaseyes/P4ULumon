using UnityEngine;

public class LiamAnimationStateController : MonoBehaviour
{
    public Animator animator;
    private int isWalkingHash;  
    private int isRunningHash;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey("left shift");
        
        
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }

        if (isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}
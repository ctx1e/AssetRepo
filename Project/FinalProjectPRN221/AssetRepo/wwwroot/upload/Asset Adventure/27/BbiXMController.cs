using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class BbiXMController : MonoBehaviour, IGamePadController
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpInput = 8f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirections touchingDirections;
    private BbiXMAttack bbiXMAttack;
    private BbiXMSkill bbiXMSkill;
    private Health myHealth;

    public static bool isAttacking = false;
    public static bool isUsingSkill = false;
    private bool _isRunning = false;
    private bool _isFacingRight = true;
    int numOfTimeGetDamage = 0;

    public int NumberOfTimeGetDamage
    {
        get { return numOfTimeGetDamage; }
        set { numOfTimeGetDamage = value; }
    }

    public Vector2 MoveInput
    {
        get => moveInput;
        set => moveInput = value;
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
            {
                FlipCharacter();
            }
            _isFacingRight = value;
        }
    }

    public bool IsMoving
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            animator.SetBool("isRunning", value);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        bbiXMAttack = GetComponent<BbiXMAttack>();
        bbiXMSkill = GetComponent<BbiXMSkill>();
        myHealth = GetComponent<Health>();
    }
    void FixedUpdate()
    {
        //if(myHealth.Hp <= 0)
        //{
        //    //Die();
        //}
        UpdateMovement();
    }
    public void OnMove(Vector2 input)
    {

        if (isUsingSkill) return;

        moveInput = input;
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }



    private void UpdateMovement()
    {

        if (!isUsingSkill && !isAttacking)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
            animator.SetBool("isRunning", moveInput.x != 0); // Đặt trạng thái chạy dựa trên moveInput
        }
        else
        {
            rb.velocity = Vector2.zero; // Dừng mọi di chuyển nếu đang sử dụng skill hoặc tấn công
            animator.SetBool("isRunning", false);
        }
    }

    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (isUsingSkill) return;

        if (moveInput.x > 0 && !_isFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && _isFacingRight)
        {
            IsFacingRight = false;
        }
    }

    private void FlipCharacter()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    public void OnJump()
    { 
        bbiXMAttack.OnJump();
    }

    public void OnDash()
    {
        if (CanExecuteAction())
        {
            animator.SetTrigger(AnimationString.dash);
            animator.SetBool(AnimationString.isDashing, true);
            PerformDash();
        }
    }

    private bool CanExecuteAction()
    {
        return touchingDirections.IsGrounded && !isUsingSkill;
    }

    private void PerformDash()
    {
        float dashDistance = 5f;
        rb.position += new Vector2(_isFacingRight ? dashDistance : -dashDistance, 0);
    }

   

    public void OnDefend()
    {
        throw new System.NotImplementedException();
    }

    public void OnAttack()
    {
        bbiXMAttack.OnAttack();
    }


    public void OnSkill1()
    {
        bbiXMSkill.OnSkillKachousen();
    }

    public void OnSkill2()
    {
        bbiXMSkill.OnSkillSayoChidori();

    }

    public void OnCombo()
    {
        bbiXMSkill.OnSkillRyuuenbu();

    }
}




using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class BbiXMAttack : MonoBehaviour
{
    [SerializeField] GameObject attackCollider1; 
    [SerializeField] GameObject attackCollider2;
    [SerializeField] GameObject attackCollider3; 
    [SerializeField] GameObject attackCollider4;
    // Start is called before the first frame update
    [SerializeField] private float jumpInput = 5f;
	private int jumpCount;
	private int maxJumps = 2;

	private Rigidbody2D rb;
	private Animator animator;
	Mana mana;


	TouchingDirections touchingDirections;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		touchingDirections = GetComponent<TouchingDirections>();
		mana = GetComponent<Mana>();
	}
	void Start()
	{

	}


	// Update is called once per frame
	void Update()
	{
		
	}

	//private bool IsJumping
	//{
	//	get
	//	{
	//		return !touchingDirections.IsGrounded && rb.velocity.y != 0;
	//	}
	//}


	// Double Jump

	void FixedUpdate()
	{
		// Kiểm tra trạng thái chạm đất trong FixedUpdate để đảm bảo cập nhật vật lý chính xác
		if (touchingDirections != null && touchingDirections.IsGrounded)
		{
			// Đặt lại số lần nhảy khi chạm đất
			jumpCount = 1;
		}
	}
	public void OnJump()
	{
        if (!BbiXMController.isUsingSkill && !BbiXMController.isAttacking)
        {
            if (touchingDirections != null && (touchingDirections.IsGrounded || jumpCount < maxJumps))
            {
                Jump();
            }
        }
    }

	private void Jump()
	{
		animator.SetTrigger(AnimationString.jump);
		rb.velocity = new Vector2(rb.velocity.x, jumpInput);
		jumpCount++;
		animator.SetInteger("countJump", jumpCount);
	}

    public void OnAttack()
    {
        if (!BbiXMController.isAttacking & touchingDirections.IsGrounded) // Chỉ tấn công nếu chưa ở trạng thái tấn công
        {
            BbiXMController.isAttacking = true; 

            int randomAttack = Random.Range(1, 5);
            animator.SetInteger("AttackIndex", randomAttack);
            animator.SetTrigger("isAttackXM");

            EnableAttackCollider(randomAttack);

            // Khởi chạy coroutine để đợi hoạt ảnh tấn công hoàn thành và mở khóa di chuyển
            StartCoroutine(EndAttack(randomAttack));
        }
    }

    private void EnableAttackCollider(int attackIndex)
    {
        // Bật collider tương ứng với đòn tấn công hiện tại
        switch (attackIndex)
        {
            case 1:
                attackCollider1.SetActive(true);
                break;
            case 2:
                attackCollider2.SetActive(true);
                break;
            case 3:
                attackCollider3.SetActive(true);
                break;
            case 4:
                attackCollider4.SetActive(true);
                break;
        }
    }
    private IEnumerator EndAttack(int attackIndex)
    {
        
        yield return new WaitForSeconds(0.6f);
        switch (attackIndex)
        {
            case 1:
                attackCollider1.SetActive(false);
                break;
            case 2:
                attackCollider2.SetActive(false);
                break;
            case 3:
                attackCollider3.SetActive(false);
                break;
            case 4:
                attackCollider4.SetActive(false);
                break;
        }
        BbiXMController.isAttacking = false; 
    }
}

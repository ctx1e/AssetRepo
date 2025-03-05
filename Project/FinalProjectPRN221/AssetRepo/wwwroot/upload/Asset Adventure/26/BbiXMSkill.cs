using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BbiXMSkill : MonoBehaviour
{
    [SerializeField] private GameObject kachousenPrefab;
    [SerializeField] private Transform kachousenPoint;
    [SerializeField] private float kachousenSpeed = 9f;

    [SerializeField] private GameObject ryuuenbuPrefab;
    [SerializeField] private Transform ryuuenbuPoint;

    [SerializeField] private GameObject sayoChidoriPrefab;
    [SerializeField] private Transform sayoChidoriPoint;

    [SerializeField] private float ultimateSpeed = 1.7f;

    internal static string skillKcs = "skillKachousen";
    internal static string throwKcs = "throwKachousen";
    internal static string skillHsb = "skillChouHissatsuShinobiBachi";
    internal static string hsb = "chouHissatsuShinobiBachi";
    internal static string skillReb = "skillRyuuenbu";
    internal static string rebFire = "ryuuenbu";
    internal static string skillSc = "skillSayoChidori";
    internal static string scSlash = "sayoChidori";

    private Rigidbody2D rb;
    private Animator animator;
    private BbiXMController bbiXMController;
    private TouchingDirections touchingDirections;
    Mana mana;

    private bool canCastSkill = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        bbiXMController = GetComponent<BbiXMController>();
        mana = GetComponent<Mana>();
    }

    // ===============================================================================================
    // Helper Methods
    private void LockMovement()
    {

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        bbiXMController.MoveInput = Vector2.zero;
        rb.velocity = Vector2.zero;
        BbiXMController.isUsingSkill = true;
        canCastSkill = false;
    }

    private void UnlockMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        BbiXMController.isUsingSkill = false;
        canCastSkill = true;
        bbiXMController.MoveInput = Vector2.zero;
        rb.velocity = Vector2.zero;
    }


    private void FlipObjectIfNeeded(GameObject skillObject)
    {
        if (!bbiXMController.IsFacingRight)
        {
            Vector3 localScale = skillObject.transform.localScale;
            localScale.x *= -1;
            skillObject.transform.localScale = localScale;
        }
    }

    private void SetSkillVelocity(Rigidbody2D skillRb, float speed)
    {
        skillRb.velocity = new Vector2((bbiXMController.IsFacingRight ? 1 : -1) * speed, 0f);
    }

    // ===============================================================================================
    // Handle Skill Kachousen
    public void OnSkillKachousen()
    {
        if (canCastSkill && touchingDirections.IsGrounded && mana.GetMana() >= 20)
        {
            LockMovement();
            mana.UseMana(20);
            BbiXMController.isUsingSkill = true;
            animator.SetTrigger(throwKcs);
            animator.SetBool(skillKcs, true);
        }
    }

    private IEnumerator KachousenObjectSpawn()
    {

        GameObject kachousen = Instantiate(kachousenPrefab, kachousenPoint.position, Quaternion.identity);
        FlipObjectIfNeeded(kachousen);
        SetSkillVelocity(kachousen.GetComponent<Rigidbody2D>(), kachousenSpeed);
        Collider2D collider = kachousen.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
        SkillObject kachousenObject = kachousen.GetComponent<SkillObject>();
        if (kachousenObject != null)
        {
            kachousenObject.damageAmount = 50;     
            kachousenObject.isKachousen = true; 
        }
        kachousenObject?.DestroySkillObject(kachousen, kachousenObject.GetKachousenLifteTime());

        yield return new WaitForSeconds(0.5f);
        if (collider != null)
        {
            collider.enabled = false;
        }
        UnlockMovement();
    }

    // ===============================================================================================
    // Handle Skill Ryuuenbu
    public void OnSkillRyuuenbu()
    {
        if (canCastSkill && touchingDirections.IsGrounded && mana.GetMana() >= 100)
        {
            LockMovement();
            mana.UseMana(20);
            BbiXMController.isUsingSkill = true;
            animator.SetTrigger(skillReb);
            animator.SetBool(rebFire, true);
        }
    }

    private IEnumerator RyuuenbuObjectSpawn()
    {

        GameObject ryuuenbu = Instantiate(ryuuenbuPrefab, ryuuenbuPoint.position, Quaternion.identity);
        FlipObjectIfNeeded(ryuuenbu);
        Collider2D collider = ryuuenbu.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        SkillObject ryuuenbuObject = ryuuenbu.GetComponent<SkillObject>();
        if (ryuuenbuObject != null)
        {
            ryuuenbuObject.damageAmount = 100; 

        }
        ryuuenbuObject?.DestroySkillObject(ryuuenbu, ryuuenbuObject.GetRyuuenbuLifteTime());

        yield return new WaitForSeconds(ryuuenbuObject.GetRyuuenbuLifteTime());
        if (collider != null)
        {
            collider.enabled = false;
        }
        UnlockMovement();
    }

    // ===============================================================================================



    // Handle Skill Sayo Chidori
    public void OnSkillSayoChidori()
    {
        // Kích hoạt skill nếu có thể cast
        if (canCastSkill && touchingDirections.IsGrounded && mana.GetMana() >= 20)
        {
            LockMovement();
            mana.UseMana(100);

            BbiXMController.isUsingSkill = true;
            animator.SetTrigger(skillSc);
            animator.SetBool(scSlash, true);
        }
    }

    private IEnumerator SayoChidoriObjectSpawn()
    {
        // Tạo ra đối tượng skill tại vị trí được chỉ định
        GameObject sayoChidori = Instantiate(sayoChidoriPrefab, sayoChidoriPoint.position, Quaternion.identity);
        FlipObjectIfNeeded(sayoChidori);
        Collider2D collider = sayoChidori.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
        SkillObject sayoChidoriObject = sayoChidori.GetComponent<SkillObject>();
        if (sayoChidoriObject != null)
        {
            sayoChidoriObject.damageAmount = 50; 
        }
        sayoChidoriObject?.DestroySkillObject(sayoChidori, sayoChidoriObject.GetRyuuenbuLifteTime());
        // Đợi thời gian tồn tại của skill và sau đó mở khóa
        yield return new WaitForSeconds(0.6f);
        if (collider != null)
        {
            collider.enabled = false;
        }
        UnlockMovement(); 
    }
    // ===============================================================================================




    // Handle Skill Chou Hissatsu Shinobi Bachi
    //public void OnSkillChouHissatsuShinobiBachi(InputAction.CallbackContext value)
    //{
    //    if (value.started && canCastSkill && touchingDirections.IsGrounded)
    //    {
    //        LockMovement();

    //        BbiXMController.isUsingSkill = true;
    //        animator.SetTrigger(skillHsb);
    //        animator.SetBool(hsb, true);
    //    }
    //}

    //public void EndAnimationOfUltimate()
    //{
    //    bbiXMController.MoveInput = Vector2.zero;
    //    UnlockMovement();
    //}

    //public void AddForwardForce()
    //{
    //    bbiXMController.MoveInput = new Vector2((bbiXMController.IsFacingRight ? 1 : -1) * ultimateSpeed, rb.velocity.y);
    //    rb.velocity = new Vector2(rb.velocity.x, 5f);
    //}
    // ===============================================================================================

}

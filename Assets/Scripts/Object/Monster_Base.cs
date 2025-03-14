using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Base : Object_Base {
    protected bool isMoving = true;
    protected Vector2 previousPos;
    protected Rigidbody2D _rb;

    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public int spawnWay = 1; // 1~3 ±æ

    public GameObject frontMonster;
    public GameObject upMonster;

    protected Animator _animator;
    private float attackAnimationDuration;

    

    protected void Start() {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        AnimatorClipInfo[] clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0 && clipInfos[0].clip.name.Contains("Attack")) {
            attackAnimationDuration = clipInfos[0].clip.length;
        }

    }

    private void FixedUpdate() {
        if (isMoving) {
            previousPos = transform.position;
            MoveToPlayer();

            _animator?.SetBool("IsIdle", true);
            _animator?.SetBool("IsAttacking", false);
        }
        if (isAttacking) {
            _animator?.SetBool("IsIdle", false);
            _animator?.SetBool("IsAttacking", true);
        }
    }

    private void MoveToPlayer() {
        if (Managers.Player == null) {
            Debug.LogError("Player not found.");
            return;
        }

        _rb.velocity = new Vector2(Vector2.left.x * moveSpeed, _rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Hero")) {
            //transform.position = previousPos;
            isMoving = false;
            isAttacking = true;
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            attackTarget = collision.GetComponent<Object_Base>();
            Attack();
        }

        if (collision.gameObject.CompareTag("Monster")) {
            if (canJump(collision.transform)) {
                frontMonster = collision.gameObject;
                collision.GetComponent<Monster_Base>().upMonster = this.gameObject;
                Jump();
            }
        }
    }

    protected bool canJump(Transform collision) {
        return collision.GetComponent<Monster_Base>().upMonster == null && IsBehind(collision);
    }

    protected bool IsBehind(Transform collision) {
        Vector2 direction = (collision.transform.position - transform.position).normalized;
        if (direction.x < 0) return true;
        return false;
    }

    protected void Jump() {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), true);
        _rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        StartCoroutine(ResetIgnoreCollision());

    }

    protected IEnumerator ResetIgnoreCollision() {
        yield return new WaitForSeconds(0.4f);
        if (frontMonster != null) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), false);
        }
    }
}

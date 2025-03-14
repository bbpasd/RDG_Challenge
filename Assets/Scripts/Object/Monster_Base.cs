using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Base : Object_Base {
    public bool isPushingToBack = false;
    public bool isMoving = true;
    protected Vector2 previousPos;
    protected Rigidbody2D _rb;

    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public int spawnWay = 1; // 1~3 길

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
            Move();

            _animator?.SetBool("IsIdle", true);
            _animator?.SetBool("IsAttacking", false);
        }
        if (isAttacking) {
            _animator?.SetBool("IsIdle", false);
            _animator?.SetBool("IsAttacking", true);
        }
    }

    protected virtual void Move() {
        if (Managers.Player == null) {
            Debug.LogError("Player not found.");
            return;
        }

        // 뒤로 밀려날 경우
        if (isPushingToBack) {
            _rb.velocity = new Vector2(Vector2.left.x * -moveSpeed, _rb.velocity.y);
        }
        else {
            _rb.velocity = new Vector2(Vector2.left.x * moveSpeed, _rb.velocity.y);
        }
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
            Debug.Log("점프 조건체크!");
            if (CanJump(collision.transform)) {
                frontMonster = collision.gameObject;
                collision.GetComponent<Monster_Base>().upMonster = this.gameObject;
                Jump();
            }
        }
    }

    protected bool CanJump(Transform collision) {
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
        StartCoroutine(PushFrontMonsterToBack());
        //StartCoroutine(ResetIgnoreCollision());

    }

    // 점프하면서 지웠던 충돌해제 복구
    protected IEnumerator ResetIgnoreCollision() {
        yield return new WaitForSeconds(0.4f);
        if (frontMonster != null) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), false);
        }
    }

    // 점프하면서 앞에 있던 몬스터를 뒤로 밀어내면서 순환
    protected IEnumerator PushFrontMonsterToBack() {
        yield return new WaitForSeconds(0.2f); // 충돌 무시 해제 대기
        if (frontMonster != null) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), false);
            yield return new WaitForSeconds(0.1f);
        }

        if (frontMonster != null) {
            Monster_Base frontMB = frontMonster.GetComponent<Monster_Base>();
            if (frontMonster != null) {
                frontMB.isPushingToBack = true;
                frontMB.isMoving = true;
                frontMB.upMonster = null;
            }

            yield return new WaitForSeconds(0.3f);
            frontMonster = null;
            yield return new WaitForSeconds(0.4f);
            if (frontMB != null) {
                frontMB.isPushingToBack = false;
            }
        }
    }
}

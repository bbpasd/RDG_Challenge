using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Base : Object_Base {
    public bool isPushingToBack = false;
    public bool isMoving = true;
    public bool isJumping = false;
    protected Vector2 previousPos;
    protected Rigidbody2D _rb;

    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public int spawnWay = 1; // 1~3 ��

    public GameObject frontMonster;
    public GameObject upMonster;

    protected Animator _animator;
    private float attackAnimationDuration;

    public BoxCollider2D _frontCollider; // Trigger

    public float stateCheckInterval = 0.5f;
    

    protected void Start() {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _frontCollider = GetComponent<BoxCollider2D>();

        AnimatorClipInfo[] clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0 && clipInfos[0].clip.name.Contains("Attack")) {
            attackAnimationDuration = clipInfos[0].clip.length;
        }

        StartCoroutine(CheckMonsterState());

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

        // �ڷ� �з��� ���
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
            Debug.Log("���� ����üũ!");
            if (IsBehind(collision.transform) && !isJumping) {
                frontMonster = collision.gameObject;
                collision.GetComponent<Monster_Base>().upMonster = this.gameObject;
                Jump();
            }
        }
    }

    protected bool CanJump(Transform collision) {
        //return collision.GetComponent<Monster_Base>().upMonster == null && IsBehind(collision); // �� ���� üũ ����
        return IsBehind(collision);
    }

    protected bool IsBehind(Transform collision) {
        Vector2 direction = (collision.transform.position - transform.position).normalized;
        if (direction.x < 0) return true;
        return false;
    }

    protected void Jump() {
        isJumping = true;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), true);
        _rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        StartCoroutine(PushFrontMonsterToBack());
        StartCoroutine(ResetJumpState());
    }

    protected IEnumerator ResetJumpState() {
        yield return new WaitForSeconds(0.3f);
        isJumping = false;
    }

    // �����ϸ鼭 �տ� �ִ� ���͸� �ڷ� �о�鼭 ��ȯ
    protected IEnumerator PushFrontMonsterToBack() {
        yield return new WaitForSeconds(0.2f); // �浹 ���� ���� ���
        if (frontMonster != null) {
            //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), false);
            yield return new WaitForSeconds(0.1f);
        }

        if (frontMonster != null) {
            Monster_Base frontMB = frontMonster.GetComponent<Monster_Base>();
            if (frontMonster != null) {
                frontMB.isPushingToBack = true;
                frontMB.isMoving = true;
                frontMB.upMonster = null;
            }

            
            /*
            // ������ �ڷ� �̵�
            float duration = 0.5f; // �о�� �ð�
            Vector2 startPosition = frontMonster.transform.position;
            float elapsedTime = 0f;
            while (elapsedTime < duration) {
                float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
                frontMonster.transform.position = Vector2.Lerp(startPosition, startPosition + new Vector2(moveSpeed * duration, 0), t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            */

            yield return new WaitForSeconds(0.3f);
            frontMonster = null;
            yield return new WaitForSeconds(0.4f);

            // ��� �浹 ���� �Ʒ��� ����
            if (frontMonster != null) {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), false);
            }
            
            if (frontMB != null) {
                frontMB.isPushingToBack = false;
            }
        }
    }

    // �������� �տ� ���Ͱ� �ִµ� ��� �� �����̴��� üũ�ϰ� ������ ���� ��Ŵ
    protected IEnumerator CheckMonsterState() {
        while (true) {
            yield return new WaitForSeconds(stateCheckInterval);
            //if (frontMonster != null || upMonster != null) continue;    
            if (isJumping) continue; // ���� ���� ��� 

            if (CheckBelow()) continue;

            // �տ� ���Ͱ� ���� ��� ������ ���� ������
            FindFrontMonster();
            if (frontMonster != null) {
                Jump();
            }
        }
    }

    protected void FindFrontMonster() {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 1f, LayerMask.GetMask("Monster"));

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.left, 1f);
        GameObject raycastMonster = null;
        foreach (RaycastHit2D hit in hits) {
            // �±� ��
            if (hit.collider.CompareTag("Monster") && hit.collider.gameObject != gameObject) {  // ���Ͱ� ��� �������� �����̰� �����Ƿ� �ڱ� �ڽ� üũ����
                if (hit.collider.GetComponent<Monster_Base>().spawnWay == this.spawnWay) {  // ���� ������ ���ͳ����� ����
                    raycastMonster = hit.collider.gameObject;
                    Debug.Log("�� ���� �߰� üũ! " + raycastMonster.gameObject.name);
                    break;
                }
                
            }
        }

        frontMonster = raycastMonster;
    }

    // ���߿� ���ִ� ���´� �ƴ��� üũ
    protected bool CheckBelow() {
        if (isJumping) return false;

        
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.3f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1.3f);
        foreach (RaycastHit2D hit in hits) {
            // �±� ��
            if (hit.collider.CompareTag("Floor")) return false;
            if (hit.collider.CompareTag("Monster") && hit.collider.gameObject != gameObject) {  // ���Ͱ� ��� �������� �����̰� �����Ƿ� �ڱ� �ڽ� üũ����
                if (hit.collider.GetComponent<Monster_Base>().spawnWay == this.spawnWay) {  // ���� ������ ���ͳ����� ����
                    return false;
                }
            }
        }
        //if (hit.collider.CompareTag("Floor")) {
        //    return false;
        //}
        //if (hit.collider == null) {
        //    isMoving = false;
        //    StartCoroutine(ResetFalling());
        //    Debug.Log("���� �߶�����");
        //}
        isMoving = false;
        StartCoroutine(ResetFalling());
        Debug.Log("���� �߶�����");
        return true;
    }

    protected IEnumerator ResetFalling() {
        yield return new WaitForSeconds(0.4f);
        isMoving = true;
    }
}

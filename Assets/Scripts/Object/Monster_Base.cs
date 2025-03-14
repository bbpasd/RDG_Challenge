using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Base : Object_Base {
    protected bool isMoving = true;
    protected Vector2 previousPos;
    protected Rigidbody2D rb;

    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public int spawnWay = 1; // 1~3 ±æ

    public GameObject frontMonster;
    public GameObject upMonster;

    public override void OnAttack(float amount) {
        base.OnAttack(amount);
    }

    protected void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (isMoving) {
            previousPos = transform.position;
            MoveToPlayer();
        }
    }

    private void MoveToPlayer() {
        if (Managers.Player == null) {
            Debug.LogError("Player not found.");
            return;
        }

        rb.velocity = new Vector2(Vector2.left.x * moveSpeed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Hero")) {
            //transform.position = previousPos;
            isMoving = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
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
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        StartCoroutine(ResetIgnoreCollision());

    }

    protected IEnumerator ResetIgnoreCollision() {
        yield return new WaitForSeconds(0.4f);
        if (frontMonster != null) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), false);
        }
    }
}

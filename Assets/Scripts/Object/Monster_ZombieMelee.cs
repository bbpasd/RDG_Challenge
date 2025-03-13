using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_ZombieMelee : Monster_Base
{
    private bool isMoving = true;
    private Vector2 previousPos;

    private Rigidbody2D rb;

    GameObject frontMonster;

    private void Start() {
        //rb = GetComponent<Rigidbody2D>();
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

        //rb.velocity = new Vector2(Vector2.left.x * moveSpeed, rb.velocity.y);

        Vector3 playerPos = Managers.Player.transform.position;
        Vector3 targetPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Hero")) {
            transform.position = previousPos;   // 움직임 복귀 (박스를 조금 밀어버리는 문제 해결)
            isMoving = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Monster")) {
            if (IsBehind(collision.transform)) {
                Debug.Log("몹끼리 닿았음 점프함" + gameObject.name);
                frontMonster = collision.gameObject;
                Jump();
            }
        }
    }

    private bool IsBehind(Transform collision) {
        Vector2 direction = (collision.transform.position - transform.position).normalized;
        if (direction.x < 0) return true;
        return false;
    }

    private void Jump() {
        //Physics.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), true);
        //rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), true);
        //rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        Debug.Log("점프");

        //Invoke(nameof(ResetIgnoreCollision), 0.5f);
    }

    private void ResetIgnoreCollision() {
        if (frontMonster == null) return;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), frontMonster.GetComponent<Collider2D>(), false);
    }
}

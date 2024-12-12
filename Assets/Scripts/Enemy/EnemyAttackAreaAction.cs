using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAreaAction : MonoBehaviour
{
    private EnemyUnit enemyUnit;
    public float enemyAttackDamage;
    public float throwForce;
    public PlayerMovement playerMov;

    // Start is called before the first frame update
    void Start()
    {
        enemyUnit = GetComponentInParent<EnemyUnit>();
        if (enemyUnit != null)
        {
            enemyAttackDamage = enemyUnit.baseDamage;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            var hittable = collision.GetComponentInParent<IHittable>();

            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();


            if (hittable != null)
            {
                Debug.Log("tidak null");
                hittable.GetHit(enemyAttackDamage, gameObject);
            }

            if (playerRB == null) return;


            playerRB.velocity = Vector2.zero;
            Vector2 thrownDirec = Vector2.zero;
            playerMov = collision.gameObject.GetComponent<PlayerMovement>();
            playerRB.simulated = true;
            if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
            {
                thrownDirec = new Vector2(-1, 1);
            }
            else if (collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                thrownDirec = new Vector2(1, 1);
            }
            else if (collision.gameObject.transform.position.y > this.gameObject.transform.position.y)
            {
                thrownDirec = new Vector2(1, 1);
            }
            EnemyHit();
            StartCoroutine(ApplyKnockBack(playerRB, thrownDirec, playerMov));

        }
    }


    private void EnemyHit()
    {
        Debug.Log("Hitting player");
    }

    public IEnumerator ApplyKnockBack(Rigidbody2D rb, Vector2 direction, PlayerMovement playerMov)
    {
        playerMov.isKnocked = true;
        rb.velocity = Vector2.zero; // Atur kecepatan ke nol
                                    //rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        rb.velocity = direction * throwForce;
        yield return new WaitForSeconds(0.3f);
        playerMov.isKnocked = false;
        rb.velocity = Vector2.zero;
    }

}

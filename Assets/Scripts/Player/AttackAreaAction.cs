using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaAction : MonoBehaviour
{
    private float playerAttackDamage;
    private PlayerUnit playerUnit;
    public float throwForce;


    private void Start()
    {
        playerUnit = GetComponentInParent<PlayerUnit>();
        if(playerUnit != null )
        {
            playerAttackDamage = playerUnit.baseDamage;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var hittable = collision.GetComponent<IHittable>();

        Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();


        if (hittable != null)
        {
            Debug.Log("tidak null");
            hittable.GetHit(playerAttackDamage, gameObject);
        }
        if(enemyRB == null) return;
   
        if(collision.gameObject.CompareTag("Enemy"))
        {
            enemyRB.velocity = Vector2.zero;
            Vector2 thrownDirec = Vector2.zero;
            EnemyUnit enemyUnit = collision.gameObject.GetComponent<EnemyUnit>();

           

            enemyRB.simulated = true;
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
            if (enemyUnit.isDyingSouls == true)
            {
                SoulsEnemyMovement soulMov = collision.gameObject.GetComponent<SoulsEnemyMovement>();
                EnemyHit();
                StartCoroutine(ApplyKnockBack(enemyRB, thrownDirec, null, soulMov, enemyUnit.isDyingSouls));
            }
            else
            {
                EnemyMovement enemyMov = collision.gameObject.GetComponent<EnemyMovement>();
                EnemyHit();
                StartCoroutine(ApplyKnockBack(enemyRB, thrownDirec, enemyMov, null, enemyUnit.isDyingSouls));
            }

            
        }
    }


    private void EnemyHit()
    {
        Debug.Log("Hitting Enemy");
    }

    public IEnumerator ApplyKnockBack(Rigidbody2D rb, Vector2 direction, EnemyMovement enemyMov, SoulsEnemyMovement soulMov, bool isDyingSouls)
    {
        if(isDyingSouls == true) soulMov.isknocked = true;
        else enemyMov.isknocked = true;

        rb.velocity = Vector2.zero; // Atur kecepatan ke nol
      //rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        rb.velocity = direction * throwForce;
        yield return new WaitForSeconds(0.1f);
        if (isDyingSouls == true) soulMov.isknocked = false;
        else enemyMov.isknocked = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularBullet : Bullet
{
    protected Rigidbody2D rb;

    public override BulletDataSO BulletData 
    { 
        get => base.BulletData;
        set
        { 
            base.BulletData = value;
            rb = GetComponent<Rigidbody2D>();
            rb.drag = BulletData.Friction;
        } 
    }

    private void FixedUpdate()
    {
        if(rb != null && BulletData != null)
        {
            rb.MovePosition(transform.position + BulletData.BulletSpeed * transform.right * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hittable = collision.GetComponent<IHittable>();

        Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();

        Debug.Log("kenak sesuatu " + collision.name);

        if (hittable != null)
        {
            Debug.Log("tidak null");
            hittable.GetHit(bulletData.Damage, gameObject);
        }
        

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (enemyRB == null) return;
            EnemyUnit enemyUnit = collision.gameObject.GetComponent<EnemyUnit>();

            enemyRB.simulated = true;

            EnemyHit(collision);

            if (enemyUnit.isDyingSouls == true)
            {
                SoulsEnemyMovement soulMov = collision.gameObject.GetComponent<SoulsEnemyMovement>();
            }
            else
            {
                EnemyMovement enemyMov = collision.gameObject.GetComponent<EnemyMovement>();
            }

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {

        }

        Destroy(this.gameObject);

    }


    private void EnemyHit(Collider2D collision)
    {
        Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
        Instantiate(BulletData.ImpactEnemyPrefab, collision.transform.position + (Vector3) randomOffset, Quaternion.identity);

    }

    private void ObstacleHit(Collider2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        if(hit.collider != null)
        {
            Instantiate(BulletData.ImpactObstaclePrefab, hit.point, Quaternion.identity);
        }
    }

}

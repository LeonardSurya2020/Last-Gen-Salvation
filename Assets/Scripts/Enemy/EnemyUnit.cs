using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour, IHittable
{

    public GameObject coinPrefab;
    public GameObject bluePrintShardPrefab;
    [SerializeField] private Image healthBarBG;
    [SerializeField] private Image healthBarImageFill;


    [SerializeField]
    private PlayerMovement playerMov;

    [field: SerializeField]
    public EnemyDataSO EnemyData { get; set; }
    [field: SerializeField]
    public float currentHealth { get; private set;}
    [field: SerializeField]
    public float baseDamage { get; private set; }
    public float baseSpeed;

    [field: SerializeField]
    public float maxHealth { get; private set; }

    [field : SerializeField]
    public UnityEvent OnGetHit { get; set; }


    //private void Awake()
    //{
    //    baseSpeed = EnemyData.speed;
    //    currentHealth = EnemyData.maxHealth;
    //    baseDamage = EnemyData.damage;
    //}
    void Start()
    {
        healthBarImageFill.gameObject.SetActive(false);
        healthBarBG.gameObject.SetActive(false);
        baseDamage = EnemyData.damage;
        baseSpeed = EnemyData.speed;
        currentHealth = EnemyData.maxHealth;
    }



    public void GetHit(float Damage, GameObject damageDealer)
    {
        Debug.Log("demeg musuh");
        currentHealth -= Damage;
        healthBarImageFill.gameObject.SetActive(true);
        healthBarBG.gameObject.SetActive(true);
        UpdateHealthBar();
        OnGetHit.Invoke();
        Debug.Log("health = " + currentHealth);
        if(currentHealth <= 0 )
        {
            Debug.Log("mokad mas");
            Die();
        }
        StartCoroutine(HealthBarVisibleDuration());
    }

    public void UpdateHealthBar()
    {
        float targetFillAmount = currentHealth / EnemyData.maxHealth;
        healthBarImageFill.fillAmount = targetFillAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DropCoin()
    {
        if (coinPrefab == null) return;
        float dropAmount = Random.Range(1, 5);
        for (int i = 0; i < dropAmount; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0);

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void DropBluePrintShard()
    {
        if (bluePrintShardPrefab == null) return;
        float dropAmount = Random.Range(1, 3);
        for (int i = 0; i < dropAmount; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0);

            Instantiate(bluePrintShardPrefab, spawnPosition, Quaternion.identity);
        }
    }


    public void Die()
    {
        playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMov.isKnocked = false;

        DropCoin();
        DropBluePrintShard();
        Destroy(gameObject);
    }

    public IEnumerator HealthBarVisibleDuration()
    {
        
        yield return new WaitForSeconds(5f);
        healthBarBG.gameObject.SetActive(false);
        healthBarImageFill.gameObject.SetActive(false);
    }
}

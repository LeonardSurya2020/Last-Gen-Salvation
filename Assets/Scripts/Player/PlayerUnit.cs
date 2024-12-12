using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUnit : MonoBehaviour, IHittable
{
    [field: SerializeField]
    public float currentHealth { get; private set; }
    public float baseDamage;
    public float baseSpeed;
    public float currencyAmount;
    public float bluePrintCurrencyAmount;

    public bool wait;

    [SerializeField] private Image healthBarImageFill;
    [SerializeField] private Image delayedHealthBarImageFill;
    [SerializeField] private float fillSpeed;

    [field: SerializeField]
    public MovementDataSO playerDataSO { get; set; }

    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }


    // Start is called before the first frame update


    void Start()
    {
        currentHealth = playerDataSO.maxHealth;
    }

    public void AddSpeedStatus()
    {

    }

    public void AddDamageStatus()
    {

    }

    public void GetHit(float Damage, GameObject damageDealer)
    {
        Debug.Log("got hit = " + Damage);
        if(wait == true) return;
        currentHealth -= Damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, playerDataSO.maxHealth);
        wait = true;
        UpdateHealthBar();
        OnGetHit.Invoke();
        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(DamageCoolDown());

    }

    public void UpdateHealthBar()
    {
        float targetFillAmount = currentHealth / playerDataSO.maxHealth;
        healthBarImageFill.fillAmount = targetFillAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(UpdateDelayHealthBar(targetFillAmount));
    }

    private IEnumerator UpdateDelayHealthBar(float targetFillAmount)
    {
        yield return new WaitForSeconds(0.5f);
        delayedHealthBarImageFill.DOFillAmount(targetFillAmount, fillSpeed);

    }

    private IEnumerator DamageCoolDown()
    {
        yield return new WaitForSeconds(0.2f);
        wait = false;
    }
    private void Die()
    {
        Debug.Log("DEAD");
        Destroy(this.gameObject);
    }
}

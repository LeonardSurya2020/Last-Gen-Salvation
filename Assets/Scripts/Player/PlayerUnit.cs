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

    [Header("Health Bar")]
    [SerializeField] private Image healthBarImageFill;
    [SerializeField] private Image delayedHealthBarImageFill;
    [SerializeField] private float fillSpeed;

    [Header("Equipment")]
    [SerializeField] public GameObject weaponObject;
    [SerializeField] public GameObject weapon;
    [SerializeField] private PlayerWeapon playerWeapon;

    [field: SerializeField]
    public MovementDataSO playerDataSO { get; set; }

    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }


    // Start is called before the first frame update


    void Start()
    {
        currentHealth = playerDataSO.maxHealth;
        string savedweaponID = PlayerPrefs.GetString("SelectedWeapon", "");
        if(!string.IsNullOrEmpty(savedweaponID))
        {
            EquipSavedWeapon(savedweaponID);
        }
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

    public void SetRangeWeapon()
    {
        RangedWeapon rangeWeapon = GetComponentInChildren<RangedWeapon>();

        if (rangeWeapon != null)
        {
            rangeWeapon.AssignWeapon();
        }
    }

    public void DontDestroy(GameObject weaponPrefab)
    {
        DontDestroyOnLoad(weaponPrefab);
    }

    private IEnumerator UpdateDelayHealthBar(float targetFillAmount)
    {
        yield return new WaitForSeconds(0.5f);
        delayedHealthBarImageFill.DOFillAmount(targetFillAmount, fillSpeed);

    }
    //Assets/Prefabs/Weapons/OldGun.prefab
    void EquipSavedWeapon(string weaponID)
    {
        // **Load prefab secara otomatis dari folder**
        GameObject weaponPrefab = Resources.Load<GameObject>($"Prefabs/Weapons/{weaponID}");


        if (weaponPrefab != null)
        {
            Transform weaponParentTransform = GameObject.FindGameObjectWithTag("WeaponParent")?.transform;

            // Hapus weapon lama jika ada
            if (weaponObject != null)
            {
                Destroy(weaponObject);
            }

            // Spawn weapon baru
            Vector2 spawnPosition = new Vector2(0.198f, 0);
            Quaternion rotationPoint = Quaternion.Euler(0, 0, 0);
            GameObject weapon = Instantiate(weaponPrefab, weaponPrefab.transform.position, rotationPoint);
            Transform weaponTransform = weapon.transform;
            weapon.transform.SetParent(weaponParentTransform);
            weaponTransform.localRotation = rotationPoint;
            weapon.transform.localPosition = spawnPosition;

            bool isRangeWeapon = PlayerPrefs.GetInt("isRangeWeapon", 0) == 1;
            PlayerMovement playermov = gameObject.GetComponentInChildren<PlayerMovement>();
            if (playermov != null)
            {
                playermov.isRangeWeapon = isRangeWeapon;
            }

            // Simpan weapon di player
            weaponObject = weapon;
            SetRangeWeapon();
        }
        else
        {
            Debug.LogError($"Weapon {weaponID} tidak ditemukan");
        }
    }

    public void RemoveRangeWeapon()
    {
       
        GameObject weapon = GameObject.FindGameObjectWithTag("RangeWeapon");
        if (weapon != null && playerWeapon.weaponFunction != null && playerWeapon.weaponRenderer != null)
        {
            playerWeapon.RemoveWeapon();
            weaponObject = null;
            Destroy(weapon);
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class WeaponFunction : MonoBehaviour
{
    [SerializeField] protected GameObject muzzle;

    [SerializeField]protected int ammo = 10;
    [SerializeField] protected WeaponDataSO weaponData;

    public int Ammo
    {
        get { return ammo; }
        set { 
            ammo = Mathf.Clamp(value, 0 , weaponData.AmmoCapacity); 
        }
    
    }

    public bool AmmoFull { get => ammo >= weaponData.AmmoCapacity; }

    protected bool isShooting = false;
    [SerializeField] protected bool reloadCoroutine = false;
    [SerializeField] protected bool reloadInProgress = false;

    [field: SerializeField] public UnityEvent OnShoot { get; set; }
    [field: SerializeField] public UnityEvent OnShootNoAmmo { get; set; }

    [field: SerializeField] public UnityEvent ReloadAmmo { get; set; }



    private void Start()
    {
        Ammo = weaponData.AmmoCapacity;
    }
    public void TryShooting()
    {
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    public void Reload()
    {
        StartCoroutine(ReloadDelay());
        
    }

    private void Update()
    {
        UseRangeWeapon();
    }

    private void UseRangeWeapon()
    {
        if(isShooting && reloadCoroutine == false)
        {
            if(Ammo > 0)
            {
                Ammo--;
                OnShoot?.Invoke();
                for(int i = 0; i < weaponData.GetBulletCountToSpawn(); i++)
                {
                    ShootBullet();
                }
            }
            else
            {
                isShooting = false;
                OnShootNoAmmo?.Invoke();

                if(reloadInProgress == false)
                {
                    ReloadAmmo?.Invoke();
                }
               
                return;
            }
            FinishShooting();
        }
    }

    private void FinishShooting()
    {
        StartCoroutine(DelayNextShooting());
        if(weaponData.AutomaticFire == false)
        {
            isShooting = false;
        }
    }

    protected IEnumerator DelayNextShooting()
    {
        reloadCoroutine = true;
        yield return new WaitForSeconds(weaponData.WeaponDelay);
        reloadCoroutine = false;
    }

    protected IEnumerator ReloadDelay()
    {
        reloadInProgress = true;
        yield return new WaitForSeconds(weaponData.ReloadDelay);
        Ammo = weaponData.AmmoCapacity;
        reloadInProgress = false;
    }

    private void ShootBullet()
    {
        SpawnBullet(muzzle.transform.position, CalculateAngle(muzzle));
        Debug.Log("Shooting a Bullet");
    }

    private void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        Debug.Log("posisi muzzle = " + position);
        var bulletPrefab = Instantiate(weaponData.BulletData.bulletPrefab, position, rotation);
        bulletPrefab.GetComponent<Bullet>().BulletData = weaponData.BulletData;
    }

    private Quaternion CalculateAngle(GameObject muzzle)
    {
        float spread = Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle);
        Quaternion bulletSpreadRotation = Quaternion.Euler(new Vector3(0, 0, spread));
        return muzzle.transform.rotation * bulletSpreadRotation;
    }
}

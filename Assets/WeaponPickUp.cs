using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{

    [SerializeField] private string weaponID;
    [SerializeField] private GameObject weaponPrefab;
    //[SerializeField] private GameObject weaponPickUpPrefab;
    [SerializeField] private bool isRangeWeapon;
    [SerializeField] private Transform weaponParentTransform;
    [SerializeField] private WeaponDataSO weaponData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMovement playerMov = collision.gameObject.GetComponentInChildren<PlayerMovement>();
            PlayerUnit playerUnit = collision.gameObject.GetComponentInParent<PlayerUnit>();
            weaponParentTransform = GameObject.FindGameObjectWithTag("WeaponParent").transform;

            if(playerMov != null && playerUnit != null && weaponParentTransform != null)
            {
                if(isRangeWeapon)
                {
                    Vector2 spawnPosition = new Vector2(0.198f , 0);
                    Quaternion rotationPoint = Quaternion.Euler(0, 0, 0);
                    GameObject weapon = Instantiate(weaponPrefab, weaponPrefab.transform.position, rotationPoint);
                    Transform weaponTransform = weapon.transform;
                    weapon.transform.SetParent(weaponParentTransform);
                    weaponTransform.localRotation = rotationPoint;
                    weapon.transform.localPosition = spawnPosition;

                    // set weapon in playerUnit
                    playerUnit.SetRangeWeapon();
                    playerUnit.weaponObject = weaponData.PickUpWeapon;
                    playerUnit.DontDestroy(weapon);
                    playerMov.isRangeWeapon = true;

                    // save selected weapon ID and etc
                    PlayerPrefs.SetString("SelectedWeapon", weaponID);
                    PlayerPrefs.SetInt("isRangeWeapon", isRangeWeapon ? 1:0);
                    PlayerPrefs.Save();

                    // Destroy this gameobject
                    Destroy(gameObject);
                }
            }

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    protected float desiredAngle;

    [SerializeField] public WeaponRenderer weaponRenderer;

    [SerializeField] public WeaponFunction weaponFunction;
    private void Awake()
    {
        AssignWeapon();
    }

    public void AssignWeapon()
    {
        weaponRenderer = GetComponentInChildren<WeaponRenderer>();
        weaponFunction = GetComponentInChildren<WeaponFunction>();
    }

    public void RemoveWeapon()
    {
        weaponRenderer = null;
        weaponFunction = null;
    }

    public virtual void AimWeapon(Vector2 pointerPositioin)
    {
        Debug.Log("pointer = " + pointerPositioin);
        var aimDirection = (Vector3)pointerPositioin - transform.position;
        desiredAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        AdjustWeaponRendering();
        transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
    }

    private void AdjustWeaponRendering()
    {
        if(weaponRenderer != null)
        {
            weaponRenderer.FlipSprite(desiredAngle > 90 || desiredAngle < -90);
            weaponRenderer.RenderBehindHead(desiredAngle < 180 || desiredAngle > 0);
        }

    }

    public void Shooting()
    {
        if(weaponFunction != null)
            weaponFunction.TryShooting();
    }

    public void StopShooting()
    {
        if (weaponFunction != null)
            weaponFunction.StopShooting();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponRenderer : MonoBehaviour
{
    [SerializeField]
    protected int PlayerSortingOrder = 0;

    protected SpriteRenderer weaponRenderer;

    private void Awake()
    {
        weaponRenderer = GetComponent<SpriteRenderer>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void FlipSprite(bool val)
    {
        int flipModifier = val ? -1 : 1;
        transform.localScale = new Vector3(transform.localScale.x, flipModifier * Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    public void RenderBehindHead(bool val)
    {
        if (val)
        {
            weaponRenderer.sortingOrder = PlayerSortingOrder - 3;
        }
        else
        {
            weaponRenderer.sortingOrder = PlayerSortingOrder + 3;
        }
    }
}

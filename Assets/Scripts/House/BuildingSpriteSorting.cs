using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSpriteSorting : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject positionObject;
    public GameObject playerObject;


    private void Update()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(playerObject.transform.position.y > positionObject.transform.position.y)
            {
                spriteRenderer.sortingOrder = 15;
            } 
           
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerObject.transform.position.y > positionObject.transform.position.y)
            {
                spriteRenderer.sortingOrder = 15;
            }
            else if (playerObject.transform.position.y < positionObject.transform.position.y)
            {
                spriteRenderer.sortingOrder = 10;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteRenderer.sortingOrder = 10;
        }
    }
    //void LateUpdate()
    //{
    //    // Sorting order diatur berdasarkan posisi Y, semakin rendah Y, semakin depan.
    //    spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDoorController : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("HIHIHA");
            animator.SetBool("PlayerClose", true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("PlayerClose", false);
        }
    }
}

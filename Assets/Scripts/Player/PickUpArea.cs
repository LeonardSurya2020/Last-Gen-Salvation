using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUpArea : MonoBehaviour
{
    private PlayerUnit playerunit;
    [SerializeField] private TextMeshProUGUI blueShardAmount;
    [SerializeField] private TextMeshProUGUI greenShardAmount;
    [SerializeField] private Image blueShardUI;
    [SerializeField] private Image greenShardUI;

    private void Start()
    {
        playerunit = GetComponentInParent<PlayerUnit>();
        blueShardAmount.text = "0";
        greenShardAmount.text = "0";

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Currency"))
        {
            currencyUnit currencyUnit = collision.gameObject.GetComponent<currencyUnit>();
            if(currencyUnit == null) return;

            float currencyAmount = currencyUnit.amount;
            AddFund(currencyAmount);
            Destroy(collision.gameObject);
            
        }
        else if(collision.gameObject.CompareTag("BluePrintCurrency"))
        {
            currencyUnit currencyUnit = collision.gameObject.GetComponent<currencyUnit>();
            if (currencyUnit == null) return;

            float currencyAmount = currencyUnit.blueprintShard;
            AddBluePrintFund(currencyAmount);
            Destroy(collision.gameObject);
        }
    }



    private void AddFund(float Amount)
    {
        if(playerunit == null) return;
        Animator animator = blueShardUI.GetComponent<Animator>();
        playerunit.currencyAmount += Amount;
        animator.SetTrigger("Get");
        blueShardAmount.text = playerunit.currencyAmount.ToString();
    }


    private void AddBluePrintFund(float Amount)
    {
        if (playerunit == null) return;
        Animator animator = greenShardUI.GetComponent <Animator>();
        playerunit.bluePrintCurrencyAmount += Amount;
        animator.SetTrigger("Get");
        greenShardAmount.text = playerunit.bluePrintCurrencyAmount.ToString();
    }
}

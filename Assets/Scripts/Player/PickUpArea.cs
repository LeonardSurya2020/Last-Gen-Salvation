using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpArea : MonoBehaviour
{
    private PlayerUnit playerunit;
    [SerializeField] private TextMeshProUGUI blueShardAmount;
    [SerializeField] private TextMeshProUGUI greenShardAmount;

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
        playerunit.currencyAmount += Amount;
        blueShardAmount.text = playerunit.currencyAmount.ToString();
    }


    private void AddBluePrintFund(float Amount)
    {
        if (playerunit == null) return;
        playerunit.bluePrintCurrencyAmount += Amount;
        greenShardAmount.text = playerunit.bluePrintCurrencyAmount.ToString();
    }
}

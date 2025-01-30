using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private bool canInteract = false;
    [SerializeField] private static float biomeCounter = 4;
    [SerializeField] private static float bossCounter = 1;

    // Start is called before the first frame update

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("This is Player");
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void Update()
    {
        Debug.Log("counter = " + biomeCounter);
        Debug.Log("counter boss = " + bossCounter);
        if (canInteract)
        {
            if(biomeCounter < 5)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    biomeCounter++;
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    biomeCounter = 0;
                    SceneManager.LoadScene("Boss_Room_" + bossCounter);
                    bossCounter++;
                }
            }

        } else
        {
            return;
        }

    }


}

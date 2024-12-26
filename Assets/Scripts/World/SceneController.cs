using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    private bool enterAllowed;
    public string sceneToLoad;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            enterAllowed = true;
            EnterScene();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enterAllowed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enterAllowed)
        {
            return;
        }

        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    SceneManager.LoadScene(sceneToLoad);
        //}
    }

    public void EnterScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

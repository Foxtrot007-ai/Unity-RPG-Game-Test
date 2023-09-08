using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Somethings hit");
        if (other.tag == "Player")
        {
            Debug.Log("Ispalyer");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player.GetComponent<InputManager>().teleportUsed && player.GetComponent<PlayerManager>().level >= 5)
            {
                Debug.Log("Loading");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}

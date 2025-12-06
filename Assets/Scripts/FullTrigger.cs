using System;
using UnityEngine;

public class FullTrigger : MonoBehaviour
{
    String playerString = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerString))
        {
            other.GetComponentInParent<Player>().RespawnPlayer();
        }
        
    }
}

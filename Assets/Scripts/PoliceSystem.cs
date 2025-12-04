using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceSystem : MonoBehaviour
{
    String playerString = "Player";
    [SerializeField] float watchTime = 5f;
    [SerializeField] bool isTriggered = false;
    [SerializeField] float speedLimit = 25f;
    [SerializeField] float policeCatchTime = 8f;
    [SerializeField] float triggerToCatchTransitionTime = 2f;
    [SerializeField] float spawnDistance = 5f;
    [SerializeField] bool policeAlreadySpawned = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerString) && !isTriggered)
        {
            isTriggered = true;
            //Debug.Log(other.gameObject.name);
            Player player = other.gameObject.GetComponentInParent<Player>();
            player.WatchText.gameObject.SetActive(true);
            StartCoroutine(watchPlayerCoroutine( player));
            
        }
    }

    IEnumerator watchPlayerCoroutine(Player player)
    {
        
        PrometeoCarController carController = player.transform.parent.GetComponentInChildren<PrometeoCarController>();
        //Transition time to give player to react to the warning
        yield return new WaitForSeconds(triggerToCatchTransitionTime);
        while(watchTime > 0f)
        {
            if(carController.carSpeed > speedLimit)
            {
                //Debug.Log("Speed limit exceeded!");
               
                PoliceCaughtSequence(carController);
            }
            watchTime -= Time.deltaTime;
            yield return null;
        }
        player.WatchText.gameObject.SetActive(false);
        
    }

    void PoliceCaughtSequence(PrometeoCarController carController)
    {
        if (policeAlreadySpawned) return;

        policeAlreadySpawned = true;    
        
        carController.StopCar();
        carController.enabled = false;
        StartCoroutine(PoliceCaughtSeqCoroutine(carController));
    }

   IEnumerator PoliceCaughtSeqCoroutine(PrometeoCarController carController)
{
    
    Player player = carController.transform.parent.GetComponentInChildren<Player>();
    Transform playerCar = carController.transform;               
    GameObject policePrefab = player.PoliceCarPrefab;           
                                        

    Vector3 spawnPos = playerCar.position + playerCar.forward * spawnDistance;
    Quaternion spawnRot = playerCar.rotation;

    GameObject spawnedPolice = Instantiate(policePrefab, spawnPos, spawnRot);
    player.policeCarAudio.Play();
    yield return new WaitForSeconds(policeCatchTime);

    carController.enabled = true;

    Destroy(spawnedPolice);
}

}

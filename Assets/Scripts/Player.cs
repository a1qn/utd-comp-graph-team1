using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] GameManager gameManager;
    [SerializeField] AudioSource crashAudio;
    [SerializeField] public AudioSource policeCarAudio;
    [SerializeField] TimeManager timeManager;
    [SerializeField] float carHitTimeReduction = 5f;
    [SerializeField] float studentHitTimeReduction = 10f;
    [SerializeField] float obstacleHitTimeReduction = 5f;
    [SerializeField] TMP_Text watchText;
    [SerializeField] GameObject policeCarPrefab;
    [SerializeField] String parkingPass = "all";
    [SerializeField] Transform spawnPoint;
    String carString = "Car";
    String studentString = "Student";
    String obstacleString = "Obstacle";
    int prevLayer = 0;
    int parkingPassNum;
    public Dictionary<string, int> parkingIndex = new Dictionary<string, int>
        {
            
            { "Green", 1 },
            { "Orange", 2 },
            { "Golden", 3 },
            {"all",4},
            {"Purple",5},
            {"",4}
        };
    ParkingSpot currentSpot;
    bool parkOptionAvailable = false;
    public bool ParkOptionAvailable{ get { return parkOptionAvailable; } set { parkOptionAvailable = value; }}
    public TMP_Text WatchText{ get { return watchText; } set { watchText = value; }}
    public ParkingSpot CurrentSpot{ get { return currentSpot; } set { currentSpot = value; }}
    public int ParkingPassNum{ get { return parkingPassNum; } set { parkingPassNum = value; }}
    public GameObject PoliceCarPrefab {get {return policeCarPrefab;}}
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit!");
        //Debug.Log(collision.gameObject.name);
        if (collision.transform.parent != null && collision.transform.parent.gameObject.CompareTag(carString))
        {
            crashSequence(carHitTimeReduction);
        }
        if (collision.gameObject.CompareTag(studentString))
        {
            Destroy(collision.gameObject);
            crashSequence(studentHitTimeReduction);
        }

        if (collision.gameObject.CompareTag(obstacleString))
        {
            Destroy(collision.transform.parent.gameObject);
            crashSequence(obstacleHitTimeReduction);
        }

        
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            int currLayer = other.gameObject.layer;
            if(currLayer == prevLayer)
            {
                RespawnPlayer();
            }
            prevLayer = currLayer;
        }
        
    }
    private void crashSequence(float timetoReduce)
    {
        crashAudio.Play();
        //DebugAudio(crashAudio);
        timeManager.TimeReduction(timetoReduce);
    }

    void DebugAudio(AudioSource src)
{
    Debug.Log("=== AUDIO DEBUG ===");
    Debug.Log("Has Clip: " + (src.clip != null));
    Debug.Log("Volume: " + src.volume);
    Debug.Log("Is Playing: " + src.isPlaying);
    Debug.Log("Spatial Blend: " + src.spatialBlend);
   
}

    void Start()
    {
      parkingPassNum = parkingIndex[parkingPass];
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && parkOptionAvailable )
        {
            
            if(parkingIndex[currentSpot.spotType.spotName] <= parkingPassNum || parkingPass == "")
            {
                LevelCompletedTrigger(currentSpot);
            }
                  
        }
    }

    public void SetGameManagerOver(bool val)
    {
        this.gameManager.IsGameOver = val;
    }

    public bool GetGameManagerOver()
    {
        return this.gameManager.IsGameOver;
    }
    public GameManager GetGameManager()
    {
        return this.gameManager;
    }

    public void LevelCompletedTrigger(ParkingSpot goalSpot)
    {
        gameManager.levelCompletedSequence(goalSpot);
    }

    public void RespawnPlayer()
    {
        PrometeoCarController carController = this.transform.parent.GetComponentInChildren<PrometeoCarController>();
        carController.StopCar();
        this.transform.position = spawnPoint.position;
        
        
    }

}

using System;
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
    String carString = "Car";
    String studentString = "Student";
    String obstacleString = "Obstacle";
    ParkingSpot currentSpot;
    bool parkOptionAvailable = false;
    public bool ParkOptionAvailable{ get { return parkOptionAvailable; } set { parkOptionAvailable = value; }}
    public TMP_Text WatchText{ get { return watchText; } set { watchText = value; }}
    public ParkingSpot CurrentSpot{ get { return currentSpot; } set { currentSpot = value; }}
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
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && parkOptionAvailable)
        {
          LevelCompletedTrigger(currentSpot);      
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

}

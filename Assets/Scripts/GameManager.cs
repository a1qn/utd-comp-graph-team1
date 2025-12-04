using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.AI;
//using System;
public class GameManager : MonoBehaviour
{
    List<ParkingSpot> parkingSpots;
    [SerializeField] public GameObject[] carPrefabs;
    [SerializeField] Player player;
    [SerializeField] float timeCounterToAllotSpace = 0f;
    [SerializeField] float allotSpacesInTime = 3f;
    [SerializeField] int allotSpacesPerTime = 1;
    [SerializeField] TimeManager timeManager;
    [SerializeField] AudioSource gameBGM;
    [SerializeField] AudioSource checkPointReachedSound;
    [SerializeField] GameObject playerCarPrefab;
    [SerializeField] TMP_Text gameOverText;
    bool isGameOver = false;
    bool isPlayerSpawned = false;
    int idGenerator = 0;
    public bool IsGameOver{ get { return isGameOver; } set { isGameOver = value; }}
    public List<ParkingSpot> ParkingSpots{ get { return parkingSpots; } set { parkingSpots = value; }}
   
    private void Awake() {
        parkingSpots = new List<ParkingSpot>(FindObjectsOfType<ParkingSpot>());
    }
    private void Update() {
        timeCounterToAllotSpace += Time.deltaTime;
        if(timeCounterToAllotSpace >= allotSpacesInTime)
        {
            AllotSpace();
            timeCounterToAllotSpace = 0f; 
        }

        // To check if timer is over

        if(timeManager.TimeLeft <= 0 && !isGameOver)
        {
            gameOverSequence();
        }
 
    }

    public void AllotSpace()
    {
        //Debug.Log(isGameOver);
        //Debug.Log(parkingSpots.Count);
        if (parkingSpots.Count == 0 || isGameOver){return;}
        
        int randomTakeSpot = Random.Range(0,parkingSpots.Count);
        ParkingSpot selectedSpot = parkingSpots[randomTakeSpot];
        AllotCar(selectedSpot);
        //Debug.Log("Space Alotted");
        selectedSpot.IsSpotAssigned = true;
        parkingSpots.RemoveAt(randomTakeSpot);

    }

    public void AllotCar(ParkingSpot selectedSpot)
{
    int randomCar = Random.Range(0, carPrefabs.Length);
    GameObject selectedCar = carPrefabs[randomCar];

    GameObject car = Instantiate(selectedCar,this.transform.position,Quaternion.identity); 
    SnapToGround(car);
    Transform carEndPos = selectedSpot.GetComponentInChildren<AgentTrigger>().transform;

    NavMeshAgent agent = car.GetComponent<NavMeshAgent>();
    CarAgent carAgent = car.GetComponent<CarAgent>();

    //carAgent.CarType = selectedCar;

    carAgent.CarID = idGenerator;
    selectedSpot.IdToAccept = idGenerator;
    idGenerator++;

    agent.SetDestination(carEndPos.position);
}

void SnapToGround(GameObject car)
{
    RaycastHit hit;

    // Cast ray down from above the car
    Vector3 start = car.transform.position + Vector3.up * 2f;

    if (Physics.Raycast(start, Vector3.down, out hit, 10f))
    {
        car.transform.position = hit.point;
    }
    else
    {
        Debug.LogWarning("No ground detected while placing car!");
    }
}

    void gameOverSequence()
    {
        gameBGM.Stop();
        player.GetComponent<PrometeoCarController>().gameObject.SetActive(false);
        isGameOver = true;
        Debug.Log("Game Over!");
        gameOverText.gameObject.SetActive(true);
    }

    public void levelCompletedSequence(ParkingSpot goalSpot)
    {
        checkPointReachedSound.Play();
        timeManager.enabled = false;
        player.gameObject.SetActive(false);

        SpawnPlayerCarAtSpot(goalSpot);
        Debug.Log("Level completed!");

    }
    void SpawnPlayerCarAtSpot(ParkingSpot goalSpot)
    {
        if (isPlayerSpawned) {return;}

        isPlayerSpawned = true;
       
        GameObject currPlay = Instantiate(playerCarPrefab.gameObject,goalSpot.transform.position,Quaternion.identity);
        //Debug.Log("Car spawned at: " + obj.transform.position);
        currPlay.GetComponent<PrometeoCarController>().enabled = false;
    }


}

using System;
using TMPro;
using UnityEngine;

public class ParkingSpot : MonoBehaviour
{
    [SerializeField] bool isTaken = false;
    [SerializeField] public ParkingType spotType;
    

    String playerString = "Player";
    String carString = "Car";
    TMP_Text parkText;

    bool isPlayerOnSpot = false;
    bool isTriggerAgent = false;
    bool isTriggerPark = false;
    bool isSpotAssigned = false;
    [SerializeField] int idToAccept = -1;
    public bool IsTaken{ get { return isTaken; } set { isTaken = value; }}
    public bool IsTriggerAgent{ get { return isTriggerAgent; } set { isTriggerAgent = value; }}
    public bool IsTriggerPark{ get { return isTriggerPark; } set { isTriggerPark = value; }}
    public bool IsSpotAssigned{ get { return isSpotAssigned; } set { isSpotAssigned = value; }}
    public int IdToAccept{ get { return idToAccept; } set { idToAccept = value; }}

    void Awake()
    {
        
    }
    void Start()
    {
       parkText =  this.GetComponentInChildren<TMP_Text>(true);
    }

    
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Here");
        if (isTriggerPark)
        {
            if (other.gameObject.CompareTag(playerString) && !isTaken && !isPlayerOnSpot)
            {
            
                Player player = other.gameObject.GetComponentInParent<Player>();
                //Debug.Log( player.parkingIndex[spotType.spotName]);
                if(player.ParkOptionAvailable || player.parkingIndex[spotType.spotName] > player.ParkingPassNum){return;}
                
                player.CurrentSpot = this;
                player.ParkOptionAvailable = true;
                //Debug.Log("PARK!");
                player.GetGameManager().ParkingSpots.Remove(this);
                //isTaken = true;
                isPlayerOnSpot = true;
                parkText.gameObject.transform.rotation =   player.transform.rotation;
                parkText.gameObject.SetActive(true);
            }
        }

        if (isTriggerAgent && other.gameObject.CompareTag(carString) && !isPlayerOnSpot && !isTaken)
        {
            
            CarAgent carAgent = other.gameObject.GetComponentInParent<CarAgent>();
            //Debug.Log(carAgent.CarID);
            if(carAgent.CarID == idToAccept)
            {
                isTaken = true;
                //Debug.Log("Car is here:" + carAgent.CarType.name);
                GameObject carPrefab = carAgent.CarType;
                //Debug.Log(carAgent.gameObject.name);
                Destroy(carAgent.gameObject);
                Transform carPointTran = transform.Find("Car Point");
                GameObject car = Instantiate(carPrefab,carPointTran.position,Quaternion.identity);


                
                //car.transform.rotation = this.transform.rotation;
            }
        }
        
    }

 

    void OnTriggerExit(Collider other)
    {
         if (!other.gameObject.CompareTag(playerString)){return;}
        
        Player player = other.gameObject.GetComponentInParent<Player>();
        if(!isTaken && !isSpotAssigned && !player.GetGameManager().ParkingSpots.Contains(this)){
            player.GetGameManager().ParkingSpots.Add(this);
            }
        player.ParkOptionAvailable = false;
        isPlayerOnSpot = false;
        parkText.gameObject.SetActive(false);
        //Debug.Log("Exit park" + spotType.name);
    }



}

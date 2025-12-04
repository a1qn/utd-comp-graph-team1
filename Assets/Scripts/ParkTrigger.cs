using UnityEngine;

public class ParkTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        this.GetComponentInParent<ParkingSpot>().IsTriggerPark = true;
    }

    void OnTriggerExit(Collider other)
    {
        this.GetComponentInParent<ParkingSpot>().IsTriggerPark = false;
    }
}

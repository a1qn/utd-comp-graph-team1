using UnityEngine;

public class AgentTrigger : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        this.GetComponentInParent<ParkingSpot>().IsTriggerAgent = true;
    }

    void OnTriggerExit(Collider other)
    {
        this.GetComponentInParent<ParkingSpot>().IsTriggerAgent = false;
    }
}

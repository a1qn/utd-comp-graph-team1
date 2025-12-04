using UnityEngine;

public class CarAgent : MonoBehaviour
{
    [SerializeField] GameObject carType;
    [SerializeField] int carID;
    public GameObject CarType{ get { return carType; } set { carType = value; }}
    public int CarID{ get { return carID; } set { carID = value; }}

}

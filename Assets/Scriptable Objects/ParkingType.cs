using UnityEngine;

[CreateAssetMenu(fileName = "ParkingType", menuName = "Scriptable Objects/ParkingType")]
public class ParkingType : ScriptableObject
{
    public string spotName;
    public Material highlightMaterial;
}

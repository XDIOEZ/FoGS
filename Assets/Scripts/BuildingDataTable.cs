using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDataTable : MonoBehaviour
{
    public static Dictionary<string,string> buildingData = new Dictionary<string, string>();
    void Start()
    {
        buildingData.Add("house", "10000");
    }
}

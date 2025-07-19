using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineManager : MonoBehaviour
{
    public List<m_Outline> outlines = new List<m_Outline>();
    // Start is called before the first frame update
    void Start()
    {
        outlines.AddRange(GetComponentsInChildren<m_Outline>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

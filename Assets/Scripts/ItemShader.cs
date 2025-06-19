using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShader : MonoBehaviour
{
public m_Outline outline;

    void Start()
    {
        outline = GetComponent<m_Outline>();
        outline.OutlineWidth = 10;
        outline.enabled = false;
    }

    void OnMouseEnter()
    {
        ApplyOutline();
    }

    void OnMouseExit()
    {
        RemoveOutline();
    }

    private void ApplyOutline()
    {
        outline.enabled = true;
    }

    private void RemoveOutline()
    {
        outline.enabled = false;
    }
}

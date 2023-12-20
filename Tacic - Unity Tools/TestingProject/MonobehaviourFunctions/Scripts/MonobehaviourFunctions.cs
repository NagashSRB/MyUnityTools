using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonobehaviourFunctions : MonoBehaviour
{
    private int x = -4;
    private void OnMouseUp()
    {
        Debug.Log("Up" + " " + Time.frameCount + " " + x);
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("Up As Button" + " " + Time.frameCount + " " + x);
    }

    private void OnMouseDown()
    {
        Debug.Log("Down" + " " + Time.frameCount + " " + x);
        x = 2;
    }

    private void OnMouseDrag()
    {
        x++;
        Debug.Log("Drag" + " " + Time.frameCount + " " + x);
    }

    private void OnMouseOver()
    {
        Debug.Log("Over" + " " + Time.frameCount + " " + x);
    }

    private void OnMouseEnter()
    {
        Debug.Log("Enter" + " " + Time.frameCount + " " + x);
    }

    private void OnMouseExit()
    {
        Debug.Log("Exit" + " " + Time.frameCount + " " + x);
    }
}

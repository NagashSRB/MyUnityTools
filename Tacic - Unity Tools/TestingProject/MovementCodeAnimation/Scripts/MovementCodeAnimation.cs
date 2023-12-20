using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MovementCodeAnimation : MonoBehaviour
{
    private Vector3 startingPosition;
    private Vector3 currentVelocity;
    private void OnMouseDown()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startingPosition = new Vector3(newPosition.x, newPosition.y, 0);
        currentVelocity = Vector3.zero;
        StartCoroutine(SmoothLerp(transform, transform.position, startingPosition + new Vector3(5, 5, 0), 2));
    }

    private void OnMouseDrag()
    {
        //transform.localPosition = Vector3.Lerp(transform.localPosition );
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition = new Vector3(newPosition.x, newPosition.y, 0);
        //transform.position = Vector3.Lerp(transform.position, newPosition, 5f * Time.deltaTime);
        
        // Parametar je daljina koja se prelazi, brzina * vreme od proslog frejma = kolko daljinu da predje.
        // Dal moze da se menja brzina? za to bih mozda koristio lerp. ili rucno podesavao 
        // var speed = 5f;
        //var step =  speed * Time.deltaTime; // calculate distance to move
        //transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
        
        //float t = Time.deltaTime / 2;
        //t = t * t * (3f - 2f * t);
        //transform.position = Vector3.Lerp(startingPosition, newPosition, t);
        
        //Da se isproba jos malo ali nije lose, bolje vrv nego infinite lerp
        //newPosition = Vector3.SmoothDamp(transform.position, startingPosition + new Vector3(5, 5, 0), ref currentVelocity, 1f);
            
        transform.position = newPosition;
        Debug.Log(newPosition);
    }
    
    IEnumerator SmoothLerp(Transform objectToMove, Vector3 startValue, Vector3 endValue, float lerpDuration)
    {
        float percentage = 0;
        float customizedPercentage = 0;
        while (percentage < 1)
        {
            objectToMove.position = Vector3.Lerp(startValue, endValue, customizedPercentage);
            percentage += Time.deltaTime / lerpDuration;
            customizedPercentage = percentage * percentage * (3f - 2f * percentage);
            yield return null;
        }
        objectToMove.position = endValue;
        Debug.Log("Gotov lerp");
    }
}

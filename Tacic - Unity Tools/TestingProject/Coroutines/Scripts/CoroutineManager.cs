using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private Coroutine a;
    private int number;
    void Start()
    {
        //StartCoroutine(MainCoroutine());
        //StartCoroutine(SecondCoroutine());
        
        //StartCoroutine(IntegerFlagCoroutine1());
        //Debug.Log(number);
        //StartCoroutine(IntegerFlagCoroutine2());

        Debug.Log($"Start C1");
        a = StartCoroutine(C1());
    }

    private IEnumerator C1()
    {
        Debug.Log($"Start C3");
        StartCoroutine(C3());
        for (int i = 0; i < 2000000000; i++)
        {
            number = i;
        }
        Debug.Log($"Start C2");
        yield return StartCoroutine(C2());
        Debug.Log($"End C2");
        yield return new WaitForSeconds(1);
        Debug.Log($"End C1");
    }
    private IEnumerator C2()
    {
        yield return new WaitForSeconds(2);
    }
    private IEnumerator C3()
    {
        yield return a;
        Debug.Log($"End C3");
    }

    private IEnumerator IntegerFlagCoroutine1()
    {
        number++;
        yield return null;
    }

    // private IEnumerator IntegerFlagCoroutine2()
    // {
    // }

    private IEnumerator MainCoroutine()
    {
        //yield return new WaitForSeconds(1);
        a = StartCoroutine(Coroutine1());
        // yield return a;
        // Debug.Log($"Main korutina gotova");
        // yield return a;
        // Debug.Log($"Main korutina gotova2");
        // yield return a;
        // Debug.Log($"Main korutina gotova3");
         yield return null;
    }
    
    private IEnumerator SecondCoroutine()
    {
        //a = StartCoroutine(Coroutine1());
        yield return a;
        Debug.Log($"Second korutina gotova");
        yield return a;
        Debug.Log($"Second korutina gotova2");
        yield return a;
        Debug.Log($"Second korutina gotova3 {a == null}");
        yield return null;
    }
    
    private IEnumerator Coroutine1()
    {
        Debug.Log($"Korutina1 startovana");
        yield return StartCoroutine(Coroutine2());
        Debug.Log($"Korutina1 zavrsena");
    }
    private IEnumerator Coroutine2()
    {
        Debug.Log($"Korutina2 startovana");
        yield return StartCoroutine(Coroutine3());
        Debug.Log($"Korutina2 zavrsena");
    }
    private IEnumerator Coroutine3()
    {
        Debug.Log($"Korutina3 startovana");
        yield return new WaitForSeconds(2);
        Debug.Log($"Korutina3 zavrsena");
        yield return null;
    }
}

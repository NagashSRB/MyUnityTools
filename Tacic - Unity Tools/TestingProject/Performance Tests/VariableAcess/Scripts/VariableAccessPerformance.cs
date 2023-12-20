using System;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;


public class VariableAccessPerformance : MonoBehaviour
{
    [SerializeField] private int numberOfCallsPerTest;
    [SerializeField] private int numberOfIterations;
    [SerializeField] private VariableAccessTestClass testClass;
    private void Awake()
    {
        for (int i = 0; i < numberOfIterations; i++)
        {
            TestPerformanceLocalVariable();
            TestPerformanceClassField();
        }
    }

    private void TestPerformanceLocalVariable()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < numberOfCallsPerTest; i++)
        {
            testClass.testNumber = i;
        }

        Debug.Log($"Time Local: {stopwatch.Elapsed}");
        stopwatch.Stop();
    }
    private void TestPerformanceClassField()
    {
        int testNumber = 2;
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < numberOfCallsPerTest; i++)
        {
            testNumber = i;
        }
        Debug.Log($"Time Class: {stopwatch.Elapsed}");
        stopwatch.Stop();
    }
}

// Script written by Louis M. Green [001103565]
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDAManager : MonoBehaviour
{
    float diffScore;
    [SerializeField] Text DDAUI;

    [SerializeField] float[] benchmark;

    public float varToChange;
    float orgVarToChange;

    float previoiusBenchmark;
    float currentBenchmark;

    float positiveTimer;


    void Start()
    {
        orgVarToChange = varToChange;
    }

    void Update()
    {

        ChangeDiff();


        //print(diffScore);
        //print("Current benchmark: " + currentBenchmark);
        //print(varToChange);

        if (diffScore < benchmark[0])
        {
            diffScore = benchmark[0];
        }
        else if (diffScore > benchmark.Length)
        {
            diffScore = benchmark[benchmark.Length];
        }

        BenchmarkCheck();

        if (DDAUI != null)
        {
            DDAUI.text = "DDA Score: " + diffScore;

            if (diffScore < 0)
            {
                DDAUI.color = Color.red;
            }
            else if (diffScore >= 0)
            {
                DDAUI.color = Color.green;
            }
        }

        positiveTimer += Time.deltaTime;

        if(positiveTimer >= 30)
        {
            DDAUp(2);
            positiveTimer = 0;
        }

    }

    public void DDAUp(float val)
    {
        diffScore += val;

        positiveTimer = 0;
    }

    public void DDADown(float val)
    {
        diffScore -= val;
    }

    //void BenchmarkCheck()
    //{
    //    for (int i = 1; i > benchmark.Length; i++)
    //    {
    //        if (diffScore > benchmark[i - 1] && diffScore <= benchmark[i])
    //        {
    //            print("test");
    //            currentBenchmark = benchmark[i];

    //            ChangeDiff();
    //        }
    //    }
    //}

    void BenchmarkCheck()
    {
        if (previoiusBenchmark != currentBenchmark)
        {
            ChangeDiff();
        }

        if (diffScore == -100)
        {
            currentBenchmark = -100;
            varToChange = 1;

        }
        else if (diffScore > -100 && diffScore <= -75)
        {
            currentBenchmark = -75;
            varToChange = 1.25f;
        }
        else if (diffScore > -75 && diffScore <= -50)
        {
            currentBenchmark = -50;
            varToChange = 1.5f;
        }
        else if (diffScore > -50 && diffScore <= -25)
        {
            currentBenchmark = -25;
            varToChange = 1.75f;
        }
        else if (diffScore > -25 && diffScore <= 0)
        {
            currentBenchmark = 0;
            varToChange = 2f;
        }
        else if (diffScore > 0 && diffScore <= 25)
        {
            currentBenchmark = 0;
            varToChange = 2f;
        }
        else if (diffScore > 25 && diffScore <= 50)
        {
            currentBenchmark = 25;
            varToChange = 2.25f;
        }
        else if (diffScore > 50 && diffScore <= 75)
        {
            currentBenchmark = 50;
            varToChange = 2.5f;
        }
        else if (diffScore > 75 && diffScore <= 100)
        {
            currentBenchmark = 75;
            varToChange = 2.75f;
        }
        else if (diffScore == 100)
        {
            currentBenchmark = 100;
            varToChange = 3f;
        }
    }


    void ChangeDiff()
    {
        //if(currentBenchmark < 0)
        //{
        //    varToChange -= (currentBenchmark * 0.05f);
        //}
        //else if(currentBenchmark > 0)
        //{
        //    varToChange += (currentBenchmark * 0.05f);

        //    //BM 25 = Health 10 + (25 * 0.05) = 11.25
        //    //BM 50 = Health 10 + (50 * 0.05) = 12.5
        //    //BM 75 = Health 10 + (75 * 0.05) = 13.75
        //    //BM 100 = Health 10 + (100 * 0.05) = 15
        //}
        //else
        //{
        //    varToChange = orgVarToChange;
        //}


    }

}

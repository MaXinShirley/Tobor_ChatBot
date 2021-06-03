using System;
using UnityEngine;

/// <summary>
/// This script to check on current date and decide on which phrase to load
/// </summary>
public class LevelManager : MonoBehaviour
{
    int phaseInitiated;
    string year;
    string date;


    //Once we get the broadcast date from Triangletales, please adjust the date here to load the scene properly
    //There are Phase 1, 2, 3, 4, 5, 6
    void Start()
    {
        year = DateTime.Now.Year.ToString();
        date = DateTime.Now.ToString("MM/dd");

        Debug.Log(year);
        Debug.Log(date);

        if (year == "2021")
        {
            if (date == "04/15")
            {
                Debug.Log("04/15 phrase 1");
                phaseInitiated = 1;
                PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
            }
            else if (date == "04/16")
            {
                Debug.Log("04/16 phrase 2");
                phaseInitiated = 3;
                PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
            }
            else
            {
                Debug.Log("Phase Unknown");
            }
        }
        else
        {
            Debug.Log("Try for 2021");
        }

    }
}

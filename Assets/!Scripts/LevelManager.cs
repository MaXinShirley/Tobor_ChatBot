using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int phaseInitiated;
    string year;
    string date;


    //Once we get the broadcast date from Triangletales, please adjust the date here to load the scene properly
    //There are Phase 1, 2, 3, 4, 5 and 5 + AR mode
    void Start()
    {
        year = DateTime.Now.Year.ToString();
        date = DateTime.Now.ToString("MM/dd");


        


        Debug.Log(year);
        Debug.Log(date);

    }

    //For testing purposes only
    //May be deleted once we have the actual date of broadcast and finished testing the app
    void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Process user message on enter press.
            phaseInitiated = 1;
            StartCoroutine(LoadLevel(5f, phaseInitiated));
            PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Process user message on enter press.
            phaseInitiated = 2;
            StartCoroutine(LoadLevel(5f, phaseInitiated));
            PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Process user message on enter press.
            phaseInitiated = 3;
            StartCoroutine(LoadLevel(5f, phaseInitiated));
            PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //Process user message on enter press.
            phaseInitiated = 4;
            StartCoroutine(LoadLevel(5f, phaseInitiated));
            PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //Process user message on enter press.
            phaseInitiated = 5;
            StartCoroutine(LoadLevel(5f, phaseInitiated));
            PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
        }*/
    }

    //This is used for the button test
    //May be deleted once we have the actual date of broadcast and finished testing the app
    public void GoToLevel(int level)
    {
        StartCoroutine(LoadLevel(5f, level));
        PlayerPrefs.SetInt("Phase Initiated", level);
    }

    //This is used for go back to intro scene
    //May be deleted once we have the actual date of broadcast and finished testing the app

    public void GoBackIntro()
    {
        SceneManager.LoadScene(0);
    }

    //Extra Scene control loading scenes dependes on different date
    //Can put 5 buttons for 5 scenes to test loading
    //Suspend execution for waitTime seconds
    IEnumerator LoadLevel(float waitTime, int sceneIndex)
    {
        yield return new WaitForSeconds(waitTime);
        print("WaitAndPrint " + Time.time);
        SceneManager.LoadScene(sceneIndex);
    }


    public void OnClickLogin()
    {

        if (year == "2021")
        {
            if (date == "04/15")
            {
                Debug.Log("04/15 phrase 1");
                phaseInitiated = 1;
                StartCoroutine(LoadLevel(3f, phaseInitiated));
                PlayerPrefs.SetInt("Phase Initiated", phaseInitiated);
            }
            else if (date == "04/16")
            {
                Debug.Log("04/16 phrase 2");
                phaseInitiated = 3;
                StartCoroutine(LoadLevel(3f, phaseInitiated));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    public Image BGimage;
    public Camera mainCam;
    public GameObject arSession;
    public GameObject tobor;
    public Canvas canvas;

    public bool objectPlaced;

    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private SwitchToggle switchToggle;

    public ARRaycastManager raycastManager { get; private set; }

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        switchToggle = FindObjectOfType<SwitchToggle>();
    }

    void Update()
    {
        if (!switchToggle.toggle.isOn)
        {
            //Turn On BG Image
            BGimage.GetComponent<Image>().enabled = false;
            //Turn off AR Cam
            arSession.SetActive(true);
            //Turn on Main Cam
            mainCam.gameObject.SetActive(false);
            //Turn On Model
            tobor.SetActive(false);
            //Turn Off AR Model
            objectToPlace.SetActive(true);
            //Set canvas render camera to MainCam
            canvas.worldCamera = null;




            if (objectPlaced == false)
            {
                UpdatePlacementPose();
                UpdatePlacementIndicator();

                if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    PlaceObject();
                }
            }

        }
        if(switchToggle.toggle.isOn)
        {
            //Turn On BG Image
            BGimage.GetComponent<Image>().enabled = true;
            //Turn off AR Cam
            arSession.SetActive(false);
            //Turn on Main Cam
            mainCam.gameObject.SetActive(true);
            //Turn On Model
            tobor.SetActive(true);
            //Turn Off AR Model
            objectToPlace.SetActive(false);
            //Set canvas render camera to MainCam
            canvas.worldCamera = mainCam;

        }

        Debug.Log(switchToggle.toggle.isOn);

    }

    private void PlaceObject()
    {
        Quaternion objRotation = Quaternion.Euler(placementPose.rotation.x, placementPose.rotation.y + 180, placementPose.rotation.z);
        objectToPlace.SetActive(true);
        objectToPlace.transform.SetPositionAndRotation(placementPose.position, objRotation);
        objectPlaced = true;
        placementIndicator.SetActive(false);

    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager = FindObjectOfType<ARRaycastManager>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
       //  arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

}

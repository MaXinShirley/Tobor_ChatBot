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

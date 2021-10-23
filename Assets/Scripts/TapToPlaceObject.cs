using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
// using UnityEngine.Experimental.XR;
using System;

public class TapToPlaceObject : MonoBehaviour
{
   
    public GameObject placementIndicator;
	public GameObject objectToPlace;

    // private ARSessionOrigin arOrigin;
	private ARRaycastManager arOrigin;
    private Pose placementPose; //data structure describes position/rotation of a point.
    private bool placementPoseIsValid = false;

    void Start()
    {
        // arOrigin = FindObjectOfType<ARSessionOrigin>();
		arOrigin = FindObjectOfType<ARRaycastManager>();

		// raycastManager = arOrigin.GetComponent<ARRaycastManager>();

    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void UpdatePlacementPose() {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

        var hits = new List<ARRaycastHit>();
		// arOrigin.AddRaycast(screenCenter)
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

	private void UpdatePlacementIndicator() {
        if (placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject() {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }
}
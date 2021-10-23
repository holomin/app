using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCursor : MonoBehaviour
{
	public GameObject cursorChildObject;
	public GameObject objectToPlace;
	public ARRaycastManager raycastManager;

	public bool useCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        cursorChildObject.SetActive(useCursor);
    }

    // Update is called once per frame
    void Update()
    {
        if (useCursor) {
			UpdateCursor();
		}
    }

	void UpdateCursor() {
		Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
		List<ARRaycastHit> hits = new List<ARRaycastHit>();
		raycastManager.Raycast(screenPosition, hits, TrackableType.Planes);

		if(hits.Count > 0) {
			transform.position = hits[0].pose.position;
			transform.rotation = hits[0].pose.rotation;
		}
	}
}

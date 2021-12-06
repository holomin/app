using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.Networking;
// using UnityEngine.JsonUtility;
// using UnityEngine.GenerateSwitch;

public class Holomin : MonoBehaviour
{
	//variables
	//debug
	public Text Logger;
	public GameObject DebugObject = null;
	public Material MatRJ45;
	private string DetectedQRCode;
	private string RegisteredQRCode;
	private string SpawnedQRCode;

	private GameObject NetworkSwitch = null; //network switch to ge generated.
	private bool QRCodeVisible = false;
	private bool IsSpawned = false;
	private Vector3 Offset = new Vector3(0f,0f,0f);

	public void OnCodeDetected(string content){
		//TODO
		// Show UI change that QR code it there.
		// Show/Update stability level in Text (debug) color (final)
		DetectedQRCode = content;
		QRCodeVisible = true;
		Log("DETECTED: " + content);

		//TODO
		// jsonQR jsondata = JsonUtility.FromJson<jsonQR>(content);
		//Parse ID from QR Code (Convert to Json)
		//if localID is not set, set.
		//Get & Parse JSON Switch Data by using ID
		//if localID is set, do nothing.
		if(NetworkSwitch == null){
			NetworkSwitch = GenerateOverlay(); //generate overlay from JSON data
		}

		// Pose = new Pose. //-0.022225f
		// Offset = new Vector3(0, 0, 0);	
	}

	public void OnCodeRegistered(string content, GameObject reference) {
		//TODO
		Log("LocalScale: "+reference.transform.localScale.ToString());
		Log("LossyScale: "+reference.transform.lossyScale.ToString());
		Log("Position: "+reference.transform.position.ToString());
		Log("Rotation: "+reference.transform.rotation.ToString());

		foreach (Transform child in reference.transform)
		{
			Log(child.ToString());
		}

		RegisteredQRCode = content;
		Log("Registered");
		if(IsSpawned == false){
			if(DebugObject) {
				//using debug object
				NetworkSwitch = Instantiate(DebugObject, reference.transform.position, reference.transform.rotation) as GameObject;
			} else {
				//using generate from json
				NetworkSwitch.transform.SetPositionAndRotation(reference.transform.position, reference.transform.rotation);
				// NetworkSwitch.transform.position += reference.transform.position; //position includes offset
				// NetworkSwitch.transform.rotation = reference.transform.rotation; //rotation should be fixed to QR code
				IsSpawned = true;
				
			}
			Log("Spawned");
		} 
		// else {
		// 	NetworkSwitch.transform.SetPositionAndRotation( gameObject.transform.position, gameObject.transform.rotation);
		// 	Log("UpdatedPositionInRegistered");
		// }
		
		// Log("POSITION: " + gameObject.transform.position.ToString());
		// Log("ROTATION: " + gameObject.transform.rotation.ToString());

		// if(RegisteredQRCode == SpawnedQRCode){
		// 	o.transform.position = gameObject.transform.position;
		// 	o.transform.rotation = gameObject.transform.rotation;
		// 	Log("LOCATION UPDATED");

		// } else {
		// 	GameObject o = Instantiate(DebugObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		// 	SpawnedQRCode = content;
		// 	o.name = "208c";
		// 	o.tag = "208c";
		// 	Log("SPHERE SPAWNED");
		// }

		// if(object with same id exists) {	
			
		// else
			//if(object with different id exists)		
				//Delete SwitchObjects
			// 	else
				//Create SwitchGameObject dependant on String
				//Spawn switch to the GameObject location

		
	}

	public void OnCodeLost() {
		QRCodeVisible = false;
		Log("LOST QR CODE");
		// Logger.Log("REGISTERED: " + content);
	}

	//debug thing
	public void Log(string message) {
		int numLines = Logger.text.Split('\n').Length;
		if(numLines > 40){
			string temp = Logger.text;
			for (int i = 40; i < numLines; i++)
			{
				temp = System.Text.RegularExpressions.Regex.Replace(Logger.text,"^(.*\n){1}","");
			}
			temp += message;
			Logger.text = temp;
			//remove lines until 40. 
		} else {
			Logger.text += message + "\r\n";
		}
	}

	// Start is called before the first frame update
    void Start()
    {
        Log("Holomin script has started");
    }

    // Update is called once per frame
    void Update()
    {
		if(QRCodeVisible == true){
			GameObject reference = GameObject.FindGameObjectWithTag("Reference");
			if(reference != null){
				NetworkSwitch.transform.SetPositionAndRotation(reference.transform.position, reference.transform.rotation);
			}
		}
    }

	public GameObject GenerateOverlay(string content = "temp") {
		Vector3 scale1 = new Vector3(1,1,1);
		Vector3 scale2 = new Vector3(0.033f, 0.033f, 0.033f);
		Vector3 scale3 = new Vector3(0.4826f, 0.04445f, 0.01f); //19" wide, 1U high, 1cm depth
		Vector3 scale4 = new Vector3(3.0303030303f, 3.0303030303f, 3.0303030303f);
		Vector3 scale5 = new Vector3(0.4826f, 0.01f, 0.04445f);

		GameObject localNetworkSwitch = new GameObject(); //empty parent gameobject.
		GameObject PRS = new GameObject(); // Position, Rotation, Scale
		SetParentChild(localNetworkSwitch, PRS);

		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.localScale = scale5;
		SetParentChild(PRS, cube);

		ZeroPositionAndRotationParams(localNetworkSwitch, PRS, cube);
		
		PRS.transform.localScale = scale1;
		PRS.transform.position = new Vector3(0.2283f,0.005f,0); //-0.1813f

		// localNetworkSwitch.transform.position = new Vector3(-0.1813f,0,0.0165f);
		// localNetworkSwitch.transform.rotation = Quaternion.Euler(rotation90);
		
		// int sections = 2;
		// int columns = 6;
		// int rows = 2;
		float x = 0;
		float y = 0;
		float offset = 0.20f;

		for (int i = 1; i < 12; i++)
		{	
			GameObject rj45 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			rj45.name = $"port{i}";
			rj45.GetComponent<Renderer>().material = MatRJ45; //set mat.
			rj45.transform.localScale = new Vector3(0.01f, 0f, 0.01f);

			Vector3 rotation0 = new Vector3(0,0,0);
			Vector3 rotation180 = new Vector3(180,0,0);			

			// Vector3 offset = new Vector3(0.01f, 0.01f, 0.1f);
			SetParentChild(PRS, rj45);
			rj45.transform.position = new Vector3(x+offset, 0.015f, y);

			if(i%2 == 0){
				y += 0.0115f;
				rj45.transform.rotation = Quaternion.Euler(rotation180);

			} else {
				y = 0;
				x += 0.0115f;
				rj45.transform.rotation = Quaternion.Euler(rotation0);
			}
		}

		// Vector3 zeroP = new Vector3(0f,0f,0f);
		// Quaternion zeroQ = new Quaternion(0f,0f,0f,0f);
		// GameObject outline = GameObject.CreatePrimitive(PrimitiveType.Plane);
		// outline.transform.localScale = scale; //plane size to height: 1U,  width: 19", depth: 1cm
		// outline.transform.SetPositionAndRotation(zeroP, zeroQ);
		// outline.transform.parent = localNetworkSwitch.transform; //add outline to parent
		// Vector3 rotation90 = new Vector3(90,0,0);
		// localNetworkSwitch.transform.position = new Vector3(-0.1813f,0,0.0165f);
		// localNetworkSwitch.transform.rotation = Quaternion.Euler(rotation90);
		// localNetworkSwitch.transform.localScale = scale;
		return localNetworkSwitch;
	}

	public void ZeroPositionAndRotation(GameObject obj){
		obj.transform.SetPositionAndRotation(new Vector3(0f,0f,0f), new Quaternion(0f,0f,0f,0f));
	}

	 public static void ZeroPositionAndRotationParams(params GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].transform.SetPositionAndRotation(new Vector3(0f,0f,0f), new Quaternion(0f,0f,0f,0f));
        }
    }

	public static void SetParentChild(GameObject parent, GameObject child) {
		child.transform.parent = parent.transform;
	}
	// public JSONObject GenerateJSON(string content) {
	// 	// JSONObject json = new JSONObject();
	// 	// json.AddField("id", content);
	// 	// json.AddField("position", new JSONObject(new Vector3(0, 0, 0)));
	// }
	IEnumerator getJsonFromApi(string key) {
		// https://api.holomin.app/208c1816-1eb3-43c5-beb7-ef936f41b838.json
		string uri = "https://api.holomin.app/" + key + ".json";
		
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
	}
	// 
	// 	// console.Log
	// 	
	// 	// plane.transform.
	// 	// PrimitiveType SwitchPlane = PrimitiveType.Plane; //cube
	// 	// GameObject Outline = new GameObject.CreatePrimitive(PrimitiveType.Plane);
	// 	// switchObj.name = "Switch";
	// 	// switchObj.transform.position = new Vector3(0, 0, 0);
	// 	// switchObj.AddComponent<NetworkIdentity>();
	// 	// switchObj.AddComponent<NetworkTransform>();
	// 	// switchObj.AddComponent<NetworkSwitch>();
	// 	// return switchObj;
	// 	// return LocalNetworkSwitch;
}


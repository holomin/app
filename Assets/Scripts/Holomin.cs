using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
// using UnityEngine.JsonUtility;


public class Holomin : MonoBehaviour
{
	//variables
	//debug
	public Text Logger;
	public GameObject DebugObject;
	// public GameObject Sphere;
	//actually neccesary
	// public GameObject NetworkSwitch;
	private string DetectedQRCode;
	private string RegisteredQRCode;
	private string SpawnedQRCode;
	// private GameObject Reference;
	private GameObject NetworkSwitch = null;
	
	private bool QRCodeVisible = false;
	private bool IsSpawned = false;
	private Vector3 Offset;

	public void OnCodeDetected(string content){
		//TODO
		// Show UI change that QR code it there.
		// Show/Update stability level in Text (debug) color (final)
		DetectedQRCode = content;
		QRCodeVisible = true;
		Log("DETECTED: " + content);

		// Pose = new Pose. //-0.022225f
		Offset = new Vector3(0, 0, 0);	
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
			NetworkSwitch = Instantiate(DebugObject, reference.transform.position + Offset, reference.transform.rotation) as GameObject;
			IsSpawned = true;
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
	
	// public static QRDataObject CreateFromJSON(string jsonString)
	// {
	// 	return JsonUtility.FromJson<PlayerInfo>(jsonString);
	// }

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
			GameObject Reference = GameObject.FindGameObjectWithTag("Reference");
			if(Reference != null){
				NetworkSwitch.transform.SetPositionAndRotation(Reference.transform.position + Offset, Reference.transform.rotation);
			}
		}
    }
}

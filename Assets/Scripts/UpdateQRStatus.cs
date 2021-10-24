using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpdateQRStatus : MonoBehaviour
{
	public Text QRStatus;
	public Text QRContent;
	public GameObject NetworkSwitch;

	public void OnCodeDetected(string content){
		Debug.Log("DETECTED: " + content);
		QRContent.text = content;
		QRStatus.text = "DETECTED";
	}

	// public void QRRegistered(string content) { 
	// 	Debug.Log("REGISTERED: " + content);
	// 	QRStatus.text = "REGISTERED";
	// }

	public void OnCodeRegistered(string content, GameObject gameObject) {
		Debug.Log("REGISTERED: " + content);
		QRStatus.text = "REGISTERED";
	}

	// public void QRRegistered(string content, GameObject qrcodelocation) { 
	// 	QRStatus.text = "REGISTERED";
	// 	Instantiate(NetworkSwitch, qrcodelocation.transform.position, qrcodelocation.transform.rotation);
	// }

	public void OnCodeLost() {
		QRContent.text = "reset";
		QRStatus.text = "LOST";
	}
}

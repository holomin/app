using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
// using Newtonsoft.Json;

public partial class Holomin : MonoBehaviour
{
	private JsonAPI _switchData = new JsonAPI();
	private RootObject _snmpData = new RootObject();

	IEnumerator GetSwitch(string id, System.Action callback)
	{
		string uri = "https://api.holomin.app/" + id;

		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			yield return webRequest.SendWebRequest();

			switch (webRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					Log("Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Log("HTTP Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					// Log("Received: \n" + webRequest.downloadHandler.text);
					yield return new WaitForSeconds(2f);
					_switchData = JsonUtility.FromJson<JsonAPI>(webRequest.downloadHandler.text);
					// Log(localDATA.data.brand);
					callback();
					break;
			}
		}
	}

	IEnumerator GetSNMP() //System.Action callback
	{
		// string uri = "http://localhost:5019/";
		string uri = "http://192.168.1.245:5019/";

		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			yield return webRequest.SendWebRequest();

			switch (webRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					Debug.Log("Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.Log("HTTP Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					// Debug.Log(webRequest.downloadHandler.text);
					_snmpData = JsonUtility.FromJson<RootObject>(webRequest.downloadHandler.text);
					// Debug.Log(_snmpData.time);
					break;
			}
		}
	}
}


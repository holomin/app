using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class Holomin : MonoBehaviour
{
	IEnumerator GetData(string id, System.Action callback)
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
					_switchData = JsonUtility.FromJson<JsonAPI>(webRequest.downloadHandler.text);
					// Log(localDATA.data.brand);
					callback();
					break;
			}
		}
	}
}

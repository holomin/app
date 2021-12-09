using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.Networking;
// using UnityEngine.JsonUtility;
// using UnityEngine.GenerateSwitch;


public partial class Holomin : MonoBehaviour
{
	private GameObject _switchObj = null; //network switch to ge generated.
	private GameObject _referenceObj = null;
	private JsonAPI _switchData = new JsonAPI();
	private string _newID = null;
	private string _currentID = null;

	private bool _isVisible = false;
	private bool _isRegistered = false;
	private bool _isSpawned = false;

	public async void OnCodeDetected(string content)
	{
		_isVisible = true;

		try
		{
			_newID = JsonUtility.FromJson<JsonQR>(content).id;
		}
		catch (System.Exception)
		{
			Log("This QR Code is not part of the Holomin ecosystem.");
		}

		//TODO
		// Show UI change that QR code it there.
		// Show/Update stability level in Text (debug) color (final)
		//TODO
		//Parse ID from QR Code (Convert to Json)
		//if localID is not set, set.
		//Get & Parse JSON Switch Data by using ID
		//if localID is set, do nothing.
	}

	public void OnCodeRegistered(string content, GameObject reference)
	{
		_isRegistered = true;
		_referenceObj = reference;
		//Show in UI that everything is good2go
		// if (NetworkSwitch)
		// {
		// 	if (!IsSpawned)
		// 	{
		// 		NetworkSwitch.transform.SetPositionAndRotation(reference.transform.position, reference.transform.rotation);
		// 		IsSpawned = true;
		// 		Log("Spawned");
		// 	}
		// }
	}

	public void OnCodeLost()
	{
		_isVisible = false;
		_isRegistered = false;
	}

	void Start()
	{
		// Start is called before the first frame update
	}

	void Update()
	{
		// Update is called once per frame
		UpdateSwitchPosition();

		if (_newID != null)
		{
			//if new QR code is scanned, create new switch
			if (_currentID != _newID)
			{
				_currentID = _newID;
				_switchObj = null; //destroy old switchObj
				StartCoroutine(GetData(_currentID, SpawnSwitch));
				// _newID = null;
			}
		}

		// if (_localID != null)
		// {
		// 	StartCoroutine(GetData(_localID, SpawnSwitch));
		// 	_localID = null;
		// }
	}
	public void UpdateSwitchPosition()
	{
		if (_isSpawned == true && _referenceObj != null)
		{
			// reference = GameObject.FindGameObjectWithTag("Reference");
			_switchObj.transform.SetPositionAndRotation(_referenceObj.transform.position, _referenceObj.transform.rotation);
			// if (reference != null && _isSpawned == true)
			// {
			// 	_networkSwitch.transform.SetPositionAndRotation(reference.transform.position, reference.transform.rotation);
			// }
		}
	}
}


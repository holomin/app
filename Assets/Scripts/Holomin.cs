using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading;
// using UnityEngine.JsonUtility;
// using UnityEngine.GenerateSwitch;
// using System.Diagnostics;
using Debug = UnityEngine.Debug;
// Debug.WriteLine


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

	public void OnCodeDetected(string content)
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

	void Start() // Start is called before the first frame update
	{
		// thread = new Thread(this.GetMacAddresses);
		// thread1 = new Thread(() => GetMacAddressesAsync(2f));
		// thread1.Start();
		// thread1.Abort();
		// thread2 = new Thread(() => UpdateSwitchPortsAsync(2f));
		// thread2.Start();
		// UnityEngine.XR.ARSubsystems.CameraFocusMode
		// GetMacAddresses();
		// InvokeRepeating("GetMacAddresses", 0.0f, 5.0f);
		// InvokeRepeating("UpdateSwitchPorts", 1.0f, 1.0f);

		StartCoroutine(GetMacAddressesAsync(2f));
		StartCoroutine(UpdateSwitchPortsAsync(2f));
	}

	void Update() // Update is called once per frame
	{
		SpawnSwitch();
		UpdateSwitchPosition();
		// UpdateSwitchPorts(2f);
	}

	private void SpawnSwitch()
	{
		if (_newID != null)
		{
			//if new QR code is scanned, create new switch
			if (_currentID != _newID)
			{
				_currentID = _newID;
				_switchObj = null; //destroy old switchObj
				StartCoroutine(GetData(_currentID, GenerateOverlay));
				// _newID = null;
			}
		}
	}
	private void UpdateSwitchPosition()
	{
		if (_isSpawned == true && _referenceObj != null)
		{
			// reference = GameObject.FindGameObjectWithTag("Reference");
			// _switchObj.transform.SetPositionAndRotation(_referenceObj.transform.position, _referenceObj.transform.rotation);
			if (_referenceObj != null && _isSpawned == true)
			{
				_switchObj.transform.SetPositionAndRotation(_referenceObj.transform.position, _referenceObj.transform.rotation);
			}
		}
	}

	private void UpdateSwitchPorts()
	{
		Log("UpdateSwitchPortsAsync");
		if (_isSpawned == true)
		{
			foreach (Dictionary<string, string> item in _SNMP)
			{
				string portname = "port" + item["port"];
				GameObject port = GameObject.Find(portname);
				port.GetComponent<Renderer>().material = _materialLAN_ON;
			}
		}

	}

	private IEnumerator UpdateSwitchPortsAsync(float seconds)
	{
		while (true)
		{
			if (_isSpawned == true && _SNMP != null)
			{
				Log("UpdateSwitchPortsAsync");
				for (int i = 1; i <= portnumber; i++)
				{


				}


				foreach (Dictionary<string, string> item in _SNMP)
				{
					Debug.Log(item["mac"] + "\n" + "on Port:" + item["port"]); // + " ip: " + ip
					string portname = "port" + item["port"];
					GameObject port = GameObject.Find(portname);
					port.GetComponent<Renderer>().material = _materialLAN_ON;
					// yield return new WaitForSeconds(0.032f);
				}

				// yield return new WaitForSeconds(10);
			}
			yield return new WaitForSeconds(seconds);
		}
	}

	private IEnumerator GetMacAddressesAsync(float seconds)
	{
		while (true)
		{
			// Thread thread_mac = new Thread(GetMacAddresses);

			// if (!thread_mac.IsAlive)
			// {
			// 	Log("Thread started");
			// 	thread_mac.Start();
			// }

			GetMacAddresses();
			yield return new WaitForSeconds(seconds);
		}
	}
}


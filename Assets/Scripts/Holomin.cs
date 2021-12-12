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
	[SerializeField] private Animator _focusBrackets;
	// [SerializeField] private string

	private GameObject _switchObj = null; //network switch to ge generated.
	private GameObject _referenceObj = null;

	private List<string> _activePorts = new List<string>();
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
			// _focusBrackets.Play("CorrectQRCode", 0, 0.0f);
			_focusBrackets.SetTrigger("triggerCorrect");
			_focusBrackets.SetTrigger("triggerIdle");
		}
		catch (System.Exception)
		{
			// _focusBrackets.Play("WrongQRCode", 0, 0.0f);
			_focusBrackets.SetTrigger("triggerWrong");
			_focusBrackets.SetTrigger("triggerIdle");
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
		_referenceObj = reference;
		_isRegistered = true;
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
		_referenceObj = null;
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

		// StartCoroutine(GetMacAddressesAsync(2f));
		StartCoroutine(GetSnmpDataAsync(2f));
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
		if (_newID != null) //if there is a _newID
		{
			if (_currentID != _newID) //if the currentID is different from newID
			{                         //works both for app start when no currentID yet, and when scanning new QR code)
				_currentID = _newID;
				Destroy(_switchObj);
				StartCoroutine(GetSwitch(_currentID, GenerateOverlay));
			}
		}
	}

	private void UpdateSwitchPosition()
	{
		if (_referenceObj != null && _isSpawned == true)
		{
			_switchObj.transform.SetPositionAndRotation(_referenceObj.transform.position, _referenceObj.transform.rotation);
		}
	}
	private IEnumerator UpdateSwitchPortsAsync(float seconds)
	{
		while (true)
		{
			if (_isSpawned == true && _snmpData != null)
			{
				for (int i = 1; i <= portnumber; i++)
				{
					string key = "port" + i;
					GameObject port = GameObject.Find(key);
					if (port)
					{
						if (_snmpData.data.FindIndex(item => item.port == i) != -1)
						{
							port.GetComponent<Renderer>().material = _materialLAN_ON;
						}
						else
						{
							port.GetComponent<Renderer>().material = _materialLAN_OFF;
						}
					}


				}
			}

			yield return new WaitForSeconds(seconds);
		}
	}

	private IEnumerator GetSnmpDataAsync(float seconds)
	{
		while (true)
		{
			StartCoroutine(GetSNMP());
			// foreach (var item in _snmpData.data)
			// {
			// 	Debug.Log(item.port + " | " + item.mac);
			// }
			yield return new WaitForSeconds(seconds);
		}
	}
}


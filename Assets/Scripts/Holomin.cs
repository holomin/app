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
using TMPro;


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
			if (_newID != null)
			{
				_focusBrackets.SetTrigger("triggerCorrect");
			}
		}
		catch (System.Exception)
		{
			_focusBrackets.SetTrigger("triggerWrong");
		}
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
		Destroy(_referenceObj);
		// _referenceObj = null;
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
		StartCoroutine(UpdateSwitchPortsAsync(3f));
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
			// yield return new WaitForSeconds(2f);

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
				// ClearLog();

				int listCounter = 0;

				for (int i = 1; i <= portnumber; i++)
				{
					string key = "port" + i;
					GameObject port = GameObject.Find(key);

					if (port)
					{
						int ts = port.transform.childCount;

						if (_snmpData.data.FindIndex(item => item.port == i) != -1)
						{
							port.GetComponent<Renderer>().material = _materialLAN_ON;

							// Log("Port:" + _snmpData.data[listCounter].port);
							// Log("Mac: " + _snmpData.data[listCounter].mac);
							// Log("IP: " + _snmpData.data[listCounter].ip);
							// Log("Host: " + _snmpData.data[listCounter].hostname);
							// Log("");

							if (ts == 0)
							{
								GameObject description = new GameObject("description" + i);
								ZeroPRSParams(description);
								SetParentChild(port, description);
								// Debug.Log(description.transform);
								description.AddComponent<TextMesh>();


								var text = description.GetComponent<TextMesh>();
								text.text = _snmpData.data[listCounter].hostname + "\n" + _snmpData.data[listCounter].ip;
								text.characterSize = 0.3f;

								Vector3 rotationVector = new Vector3(90, 0, 270);
								Quaternion rotation = Quaternion.Euler(rotationVector);
								Vector3 position = new Vector3(0.5f, 0f, -1f);
								description.transform.localPosition = position;
								description.transform.localRotation = rotation;
								description.transform.localScale = new Vector3(1f, 1f, 1f);
							}
							listCounter++;

						}
						else
						{
							port.GetComponent<Renderer>().material = _materialLAN_OFF;
							if (ts != 0)
							{
								foreach (Transform child in port.transform)
								{
									Destroy(child.gameObject);
								}
							}
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Holomin : MonoBehaviour
{
	public Material _materialRJ45;
	public Material _materialSFP;
	private GameObject CreateSwitchOld(JsonAPI json)
	{
		// Log(json.data.brand);
		Vector3 scale1 = new Vector3(1, 1, 1);
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
		PRS.transform.position = new Vector3(0.2283f, 0.005f, 0); //-0.1813f

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
			rj45.GetComponent<Renderer>().material = _materialRJ45; //set mat.
			rj45.transform.localScale = new Vector3(0.01f, 0f, 0.01f);

			Vector3 rotation0 = new Vector3(0, 0, 0);
			Vector3 rotation180 = new Vector3(180, 0, 0);

			// Vector3 offset = new Vector3(0.01f, 0.01f, 0.1f);
			SetParentChild(PRS, rj45);
			rj45.transform.position = new Vector3(x + offset, 0.015f, y);

			if (i % 2 == 0)
			{
				y += 0.0115f;
				rj45.transform.rotation = Quaternion.Euler(rotation180);

			}
			else
			{
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

	public void ZeroPositionAndRotation(GameObject obj)
	{
		obj.transform.SetPositionAndRotation(new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f));
	}

	public static void ZeroPositionAndRotationParams(params GameObject[] list)
	{
		for (int i = 0; i < list.Length; i++)
		{
			list[i].transform.SetPositionAndRotation(new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f));
		}
	}

	public static void SetParentChild(GameObject parent, GameObject child)
	{
		child.transform.parent = parent.transform;
	}

	private void SpawnSwitch()
	{
		Vector3 scale2 = new Vector3(0.033f, 0.033f, 0.033f);
		Vector3 scale3 = new Vector3(0.4826f, 0.04445f, 0.01f); //19" wide, 1U high, 1cm depth
		Vector3 scale4 = new Vector3(3.0303030303f, 3.0303030303f, 3.0303030303f);
		Vector3 scale5 = new Vector3(0.4826f, 0.01f, 0.04445f);

		// CREATE MAIN OBJECT & PRS
		GameObject localNetworkSwitch = new GameObject(_switchData.data.brand + " " + _switchData.data.model); //empty parent gameobject.
		GameObject PRS = new GameObject("PosRotScale"); // Position, Rotation, Scale
		PRS.transform.localScale = new Vector3(1, 1, 1);
		SetParentChild(localNetworkSwitch, PRS);

		// CREATE OUTLINE
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		SetParentChild(PRS, cube);
		cube.name = "SwitchBody";
		cube.transform.localScale = new Vector3(_switchData.data.layout.width, 0.005f, _switchData.data.layout.height);

		//Reset
		ZeroPositionAndRotationParams(localNetworkSwitch, PRS, cube);

		int portnumber = 1;
		foreach (Section s in _switchData.data.sections)
		{
			//CREATE SECTION
			GameObject newSection = new GameObject("section" + s.id.ToString());
			SetParentChild(PRS, newSection);

			Vector3 rotation0 = new Vector3(0, 0, 0);
			Vector3 rotation180 = new Vector3(180, 0, 0);


			// float offsetY = s.po;

			//CREATE PORTS
			float posX = 0.000f;
			float posY = 0.000f;
			float rotate = 180f;
			for (int i = 0; i < s.ports; i++)
			{
				GameObject port = GameObject.CreatePrimitive(PrimitiveType.Cube);
				SetParentChild(newSection, port);
				port.name = $"port{portnumber}";
				portnumber++;

				Vector3 size = new Vector3(0.012f, 0f, 0.01f);
				switch (s.type)
				{
					case "RJ45":
						size = new Vector3(0.012f, 0f, 0.01f);
						port.GetComponent<Renderer>().material = _materialRJ45;
						break;
					case "SFP":
						size = new Vector3(0.0135f, 0f, 0.0118f);
						port.GetComponent<Renderer>().material = _materialSFP;
						rotate = 180;
						break;
				}
				port.transform.localScale = size; //size of an RJ45 port
				port.transform.position = new Vector3(posX, 0, posY);
				port.transform.RotateAround(port.transform.position, port.transform.up, rotate);
				switch (s.order)
				{
					case "TopToBottom":
						if (i % 2 != 0)
						{
							posX += s.offsetX + size.x;
							posY = 0;
							rotate = 180f;
						}
						else
						{
							//posX doesn't change. 
							posY -= s.offsetY + size.z;
							rotate = 0;
						}
						break;
					case "LeftToRight":
						if (i % 2 != 0)
						{
							posY = 0;
						}
						else
						{
							posX += s.offsetX + size.x;
						}
						break;
				}
			}
			Log(s.posY.ToString());
			newSection.transform.position = new Vector3(s.posX, 0.003f, s.posY);
		}

		PRS.transform.position = new Vector3(_switchData.data.layout.qrCodeOffset, 0.001f, 0); //-0.1813f

		_switchObj = localNetworkSwitch;
		_isSpawned = true;
	}
	// public class Section
	// {
	// 	public int id = 0;
	// 	public float offsetX = 0.100f;
	// 	public float offsetY = 0;
	// 	public int ports = 8;
	// 	public string order = "TopToBottom";

	// 	public float portOffsetX = 0.002f;
	// 	public float portOffsetY = 0.001f;
	// }
}


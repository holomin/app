using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class Holomin : MonoBehaviour
{
	public Material _materialSwitch;
	public Material _materialLAN_OFF;
	public Material _materialLAN_ON;
	public Material _materialSFP;


	private int portnumber = 1;

	public static void ZeroPRSParams(params GameObject[] list)
	{
		for (int i = 0; i < list.Length; i++)
		{
			list[i].transform.localPosition = new Vector3(0f, 0f, 0f);
			list[i].transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
			list[i].transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public static void SetParentChild(GameObject parent, GameObject child)
	{
		child.transform.parent = parent.transform;
	}

	private void GenerateOverlay()
	{
		// Vector3 scale2 = new Vector3(0.033f, 0.033f, 0.033f);
		// Vector3 scale3 = new Vector3(0.4826f, 0.04445f, 0.01f); //19" wide, 1U high, 1cm depth
		// Vector3 scale4 = new Vector3(3.0303030303f, 3.0303030303f, 3.0303030303f);
		// Vector3 scale5 = new Vector3(0.4826f, 0.01f, 0.04445f);

		// CREATE MAIN OBJECT & PRS
		GameObject localNetworkSwitch = new GameObject(_switchData.data.brand + " " + _switchData.data.model); //empty parent gameobject.
		ZeroPRSParams(localNetworkSwitch);                                                                                         // ZeroPRSParams(localNetworkSwitch);

		GameObject PRS = new GameObject("PosRotScale"); // Position, Rotation, Scale
		ZeroPRSParams(PRS);
		PRS.transform.localScale = new Vector3(1, 1, 1);
		SetParentChild(localNetworkSwitch, PRS);

		// CREATE SWITCH BODY
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ZeroPRSParams(cube);
		SetParentChild(PRS, cube);
		cube.name = "SwitchBody";
		cube.transform.localScale = new Vector3(_switchData.data.layout.width, 0.01f, _switchData.data.layout.height);
		cube.GetComponent<Renderer>().material = _materialSwitch;

		//OUTLINE
		var outline = cube.AddComponent<Outline>();
		outline.OutlineMode = Outline.Mode.OutlineAll;
		outline.OutlineColor = new Color(0, 175, 336, 1);
		outline.OutlineWidth = 10f;

		//Reset



		foreach (Section s in _switchData.data.sections)
		{
			//CREATE SECTION
			GameObject newSection = new GameObject("section" + s.id.ToString());
			ZeroPRSParams(newSection);
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
				ZeroPRSParams(port);
				SetParentChild(newSection, port);



				port.name = $"port{portnumber}";
				portnumber++;

				Vector3 size = new Vector3(0.012f, 0f, 0.01f);
				switch (s.type)
				{
					case "RJ45":
						size = new Vector3(0.012f, 0f, 0.01f);
						port.GetComponent<Renderer>().material = _materialLAN_OFF;
						break;
					case "SFP":
						size = new Vector3(0.0135f, 0f, 0.0118f);
						port.GetComponent<Renderer>().material = _materialSFP;
						rotate = 180;
						break;
				}

				port.transform.localScale = size; //size of an RJ45 port
				port.transform.localPosition = new Vector3(posX, 0f, posY);
				port.transform.RotateAround(port.transform.localPosition, port.transform.up, rotate);

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




			newSection.transform.localPosition = new Vector3(s.posX, 0.011f, s.posY);
		}

		PRS.transform.localPosition = new Vector3(_switchData.data.layout.qrCodeOffset, 0f, 0); //-0.1813f

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


using System;
using System.Collections.Generic;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// private const double width = 0.48260;
// private const double height = 0.04445;

[Serializable] //why is this neccesary?
public class JsonAPI
{
	public Response response;
	public Data data;

}

[Serializable]
public class Response
{
	public int code;
	public string status;
	public string message;
	public int ms;
}

[Serializable]
public class Data
{
	public string id;
	public string brand;
	public string model;
	public bool poe;
	public bool poeplus;
	public Layout layout;
	public List<Section> sections;
}

[Serializable]
public class Layout
{
	public float qrCodeOffset;
	public float width;
	public float height;
	// public Ports ports;

}

[Serializable]
public class Ports
{
	public RJ45 rj45;
	public SFP sfp;
}

[Serializable]
public class RJ45
{
	public float leftOffset;
	public float topOffset;
	public string speed; //will be converted to array when jsonified
	public int amount;
	public int sections;
	public float sectionSpacing;
	public int rows;
	public string order;
	public string poe; //will be converted to array when jsonified
	public string poeplus; //will be converted to array when jsonified
}

[Serializable]
public class SFP
{
	public float leftOffset;
	public float topOffset;
	public string speed; //will be converted to array when jsonified
	public int amount;
	public int sections;
	public float sectionSpacing;
	public int rows;
	public string order;
}
[Serializable]
public class Section
{
	public int id;
	public string type;
	public float posX;
	// public float posY;
	public float posY
	{
		get
		{
			switch (type)
			{
				case "RJ45":
					return 0.005f + (offsetY / 2f);
				case "SFP":
					return 0.0058f + (offsetY / 2f);
				default:
					return 0;
			}
		}
	}
	public int ports;
	public string order;
	public float offsetX;
	public float offsetY;
}
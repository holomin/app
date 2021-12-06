using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// private const double width = 0.48260;
// private const double height = 0.04445;

[Serializable] //why is this neccesary?
public class jsonData {
	public string id;
	public string brand;
	public string model;
	public bool poe;
	public bool poeplus;
	public Layout layout;
}

[Serializable]
public class Layout {
	public double qrCodeOffset;
	public double width;
	public double height;
}

[Serializable]
public class Ports {
	public RJ45 rj45;
	public SFP sfp;
}

[Serializable]
public class RJ45 {
	public double leftOffset;
	public double topOffset;
	public string speed; //will be converted to array when jsonified
	public int amount;
	public int section;
	public int rows;
	public string order;
	public string poe; //will be converted to array when jsonified
	public string poeplus; //will be converted to array when jsonified

}

[Serializable]
public class SFP {
	public double leftOffset;
	public double topOffset;
	public string speed; //will be converted to array when jsonified
	public int amount;
	public int section;
	public int rows;
}
using System;
using System.Collections.Generic;

[Serializable]
public class JsonPort
{
	public int port;
	public string mac;
	public string ip;
	public string hostname;
}

[Serializable]
public class RootObject
{
	public string time;
	public List<JsonPort> data;
}
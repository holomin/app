using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.IO;
using System;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;

public partial class Holomin : MonoBehaviour
{
	List<Dictionary<string, string>> _SNMP = null;
	// List<Dictionary<string, string>> SNMP = null;

	private void GetMacAddresses() // string SwitchIp = "192.168.1.222"
	{
		string SwitchIp = "192.168.1.222";
		// List<Dictionary<string, string>> Devices = new List<Dictionary<string, string>>();
		_SNMP = new List<Dictionary<string, string>>();

		var result = new List<Variable>();

		Messenger.Walk(VersionCode.V2,
					   new IPEndPoint(IPAddress.Parse(SwitchIp), 161),
					   new OctetString("holomin"),
					   new ObjectIdentifier(".1.3.6.1.2.1.17.4.3.1.2"),
					   result,
					   60000,
					   WalkMode.WithinSubtree);

		foreach (var item in result)
		{
			Dictionary<string, string> Device = new Dictionary<string, string>();
			string mac = "";
			var substring = "1.3.6.1.2.1.17.4.3.1.2.";
			var splititem = System.Convert.ToString(item).Split(':');
			var indexofsubstring = splititem[2].IndexOf(substring);
			var withoutsubstring = splititem[2].Remove(indexofsubstring, substring.Length);
			var total = withoutsubstring.Split(';');
			var convertion = total[0].Split('.');
			foreach (var number in convertion)
			{
				int myint = int.Parse(number);
				string macPart = myint.ToString("X") + '-';
				if (macPart.Length == 2)
				{
					macPart = "0" + macPart;
				}
				mac += macPart;
			}
			mac = mac.Remove(mac.Length - 1, 1);
			var Port = splititem[3].Trim();

			Device.Add("port", Port);
			Device.Add("mac", mac);
			// if (Port != " 1")
			// {
			// 	Device.Add("port", Port);
			// 	Device.Add("mac", mac);
			// 	// string ip = IPMacMapper.FindIPFromMacAddress(mac.ToLower());
			// 	Debug.Log(Device["mac"] + "\n" + "on Port:" + Device["port"]); // + " ip: " + ip
			// }
			_SNMP.Add(Device);
		}

		// thread_mac.Abort();
	}
}


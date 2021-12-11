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

public class IPMacMapper
{
	private static List<IPAndMac> list;

	private static StreamReader ExecuteCommandLine(String file, String arguments = "")
	{
		ProcessStartInfo startInfo = new ProcessStartInfo();
		startInfo.CreateNoWindow = true;
		startInfo.WindowStyle = ProcessWindowStyle.Hidden;
		startInfo.UseShellExecute = false;
		startInfo.RedirectStandardOutput = true;
		startInfo.FileName = file;
		startInfo.Arguments = arguments;

		Process process = Process.Start(startInfo);

		return process.StandardOutput;
	}

	private static void InitializeGetIPsAndMac()
	{
		if (list != null)
			return;

		var arpStream = ExecuteCommandLine("arp", "-a");
		List<string> result = new List<string>();
		while (!arpStream.EndOfStream)
		{
			var line = arpStream.ReadLine().Trim();
			result.Add(line);
		}

		list = result.Where(x => !string.IsNullOrEmpty(x) && (x.Contains("dynamic") || x.Contains("static")))
			.Select(x =>
			{
				string[] parts = x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				return new IPAndMac { IP = parts[0].Trim(), MAC = parts[1].Trim() };
			}).ToList();
	}

	public static string FindIPFromMacAddress(string macAddress)
	{
		//Debug.Log(macAddress);
		InitializeGetIPsAndMac();
		//Debug.Log(IPMacMapper.list[0].IP);
		IPAndMac item = list.SingleOrDefault(x =>
		{
			//Debug.Log(x.MAC + " " + x.IP);

			return x.MAC == macAddress;

		});
		if (item == null)
			return "No ip found";
		return item.IP;
	}

	public static string FindMacFromIPAddress(string ip)
	{
		InitializeGetIPsAndMac();
		IPAndMac item = list.SingleOrDefault(x => x.IP == ip);
		if (item == null)
			return null;
		return item.MAC;
	}

	private class IPAndMac
	{
		public string IP { get; set; }
		public string MAC { get; set; }
	}
}

// public class Port
// {
// 	public string mac { get; set; }
// 	public string ip { get; set; }
// 	public string hostname { get; set; }
// }
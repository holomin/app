using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Holomin : MonoBehaviour
{
	public Text Logger;
	public void Log(string message)
	{
		int numLines = Logger.text.Split('\n').Length;
		if (numLines > 40)
		{
			string temp = Logger.text;
			for (int i = 40; i < numLines; i++)
			{
				temp = System.Text.RegularExpressions.Regex.Replace(Logger.text, "^(.*\n){1}", "");
			}
			temp += message;
			Logger.text = temp;
			//remove lines until 40. 
		}
		else
		{
			Logger.text += message + "\r\n";
		}
	}
}

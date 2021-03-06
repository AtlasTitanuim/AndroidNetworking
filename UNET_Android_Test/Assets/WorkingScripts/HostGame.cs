﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
public class HostGame : MonoBehaviour {
	private uint roomSize = 16;
	private string roomName;
	private NetworkManager networkManager;
	public GUIStyle customButton;
	public GUIStyle customInputField;
	public Texture2D Background;
	public Texture2D Button;
	public Texture2D inpField;
	public Font currentFont;

	void Start(){
		networkManager = NetworkManager.singleton;
		if(networkManager.matchMaker == null){
			networkManager.StartMatchMaker();
		}

		roomName = "/room name/";
	}

	public void SetRoomName(string _name){
		roomName = _name;
	}

	public void CreateRoom(){
		if(roomName != "" && roomName != null && roomName != "/room name/"){
			networkManager.matchMaker.ListMatches(0, 10, roomName, true, 0, 0, OnMatchList);
		}
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background, ScaleMode.StretchToFill, true, 10.0F);
		customButton = new GUIStyle("button");
		customButton.normal.background = Button;
		customButton.hover.background = Button;
		customButton.fontSize = Screen.height/14;
		customButton.font = currentFont;

		//Host game Button
		customButton.fontSize = Screen.height/12;
		if (GUI.Button(new Rect(0, Screen.height-(Screen.height/6), Screen.width, Screen.height/6), "Host Game", customButton)){
			CreateRoom();
		}

		//InputField
		customInputField = new GUIStyle("");
		customInputField.normal.background = inpField;
		customInputField.hover.background = inpField;
		customInputField.fontSize = Screen.height/24;
		customInputField.alignment = TextAnchor.MiddleCenter;
		customInputField.font = currentFont;
		customInputField.normal.textColor = Color.black;
		roomName = GUI.TextField(new Rect(0, Screen.height-((Screen.height/6)*1.5f), Screen.width, Screen.height/12), roomName, customInputField);
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
		if(matches.Count >= 1){
			Debug.Log("Already a Match");
		} else{
			Debug.Log("No matches found!");
			networkManager.matchMaker.CreateMatch(roomName,roomSize,true,"","","",0,0,networkManager.OnMatchCreate);
		}
    }
}

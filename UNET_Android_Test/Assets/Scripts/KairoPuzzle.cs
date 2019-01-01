﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class KairoPuzzle : NetworkBehaviour {

	private PlayerController pController;
	public GameObject brokenPuzzle;
	public Button startPuzzleButton;
	public GameObject[] puzzlePieces;
	private int countToWin;
	private int finalScore;
	private GUIStyle end;
	public Font Dyslectic_font;
	public Texture2D winImage;
	private bool won = false;
	void Start(){
		startPuzzleButton.onClick.AddListener(TaskOnClick);	
		foreach(GameObject piece in puzzlePieces){
			piece.GetComponent<Draggable>().enabled = false;
		}
	}

	void Update(){
		if(isServer){
            this.enabled = false;
            return;
        }
		if(pController != null){
			Debug.Log("found player");
		} else{
			pController = GameObject.Find("Client").GetComponent<PlayerController>();
			Debug.Log("player not found");
			return;
		}

		brokenPuzzle.SetActive(!pController.puzzle3);

		if(!startPuzzleButton.gameObject.active){
			Debug.Log("puzzle running");
			foreach(GameObject piece in puzzlePieces){
				if(piece.GetComponent<Draggable>().isRight){
					countToWin++;
				} else {
					countToWin = 0;
					break;
				}

				Debug.Log(countToWin);
				if(countToWin >= puzzlePieces.Length){
					StopAllCoroutines();
					pController.puzzle3 = true;
					won = true;
					countToWin = 0;
				}
			}
		}
	}

	void OnGUI(){
		end = new GUIStyle("button");
		end.normal.background = winImage;
		end.alignment = TextAnchor.MiddleCenter;
		end.font = Dyslectic_font;

		if(won){
			end.fontSize = Screen.height/17;
			string score = "Goed gedaan\nJe wint:\n" + finalScore + " Punten";
			if(GUI.Button(new Rect(Screen.width/8,Screen.height/4,Screen.width/1.25f,Screen.height/2), score, end)){
				pController.CmdGainScore(finalScore,3);
				this.enabled = false;
			}
		} 
	}
	public void TaskOnClick(){
		foreach(GameObject piece in puzzlePieces){
			piece.GetComponent<Draggable>().enabled = true;
		}
		while(startPuzzleButton.gameObject.active){
			Debug.Log("still active");
			startPuzzleButton.gameObject.SetActive(false);
		}
		Debug.Log("start puzzle");
		StartCoroutine(CountScore());
	}

	private IEnumerator CountScore(){
		finalScore = 200;
		yield return new WaitForSeconds(60);
		finalScore = 100;
		yield return new WaitForSeconds(60);
		finalScore = 50;
	}
}

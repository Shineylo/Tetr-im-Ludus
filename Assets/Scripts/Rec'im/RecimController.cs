using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecimController : MonoBehaviour
{
	public static RecimController instance;


	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
	private void Awake()
	{
		if(instance != null)
		{
			return;
		}
		instance = this;
	}
	/*
	public bool GameLevelStart(Callback functionCallbackToStart)
	{

	}
	*/
	public void StartStockedGame()
	{

	}

	public void GameLevelQuit()
	{

	}

	/*public void GameReviveRequested(Action<bool> onReviveRequestCompleteCallback)
	{

	}*/
	public void GameReviveRequestComplete()
	{

	}

	public void GameLevelComplete(int scorePlayer, int playerHasRevive, float timeInGame)
	{

	}
	
}

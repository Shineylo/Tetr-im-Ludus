using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptUi : MonoBehaviour
{

    //variable that store if the game is pause
    public bool gamePause = false;
    //variables that store gameobject that are on the UI for modifiction on them
    public GameObject GameOverUi, pauseUi, on, off, revive, replay, dontRevive;

    // Start is called before the first frame update
    private void Start()
    {
        //The fonction is to check if the user mute or not the song at the start of the scene
        //Useful if the user muted the app during the game
        if (PlayerPrefs.GetInt("mute") == 1)
        {
            //Get the audio and pause it
            FindObjectOfType<AudioSource>().Pause();
            //Toggle the game object that display the music is muted
            off.SetActive(true);
        }
        else
        {
            //Get the audio and play it
            FindObjectOfType<AudioSource>().Play();
            //Toggle the game object that display the music is playing
            on.SetActive(true);
        }
    }

    public void MusicManage()
    {
        if (PlayerPrefs.GetInt("mute") == 0)//Check if music was playing
        {
            //Able the gameobject showing that music is muted
            on.SetActive(false);
            //Disable the gameobject showing that music is playing
            off.SetActive(true);
            //Get the audio and pause it
            FindObjectOfType<AudioSource>().Pause();
            //Store that the musi is muted
            PlayerPrefs.SetInt("mute", 1);
        }
        else
        {
            //Able the gameobject showing that music is playing
            on.SetActive(true);
            //Disable the gameobject showing that music is muted
            off.SetActive(false);
            //Get the audio and play it
            FindObjectOfType<AudioSource>().Play();
            //Store that the musi is playing
            PlayerPrefs.SetInt("mute", 0);
        }
    }

    //fonction that resume the game
    public void Resume()
    {
        //disable the UI that display the pause menu
        pauseUi.SetActive(false);
        //Set the time scale to 1 unit so everything unfreeze
        Time.timeScale = 1f;
        //Change to false the fact that the game is pause
        gamePause = false;
        //Since when you resume you tapp on the screen that made the tetromino rotate, so i had to undo it
        FindObjectOfType<ScriptTetromino>().UnRotation();
    }

    //Fonction that pause the game
    public void Pause()
    {
        //change to true the fact the game is pause
        gamePause = true;
        //Activate the UI that display the pause menu
        pauseUi.SetActive(true);
        //Set the time scale to 0 so everything freeze
        Time.timeScale = 0f;
    }

    //Fonction that display either the revive or the replay menu depending is it was a gameover or not
    public void End(bool GO)
    {
        if (GO) //If it is a game over 
        {
            //display the revive menu
            revive.SetActive(true);
            dontRevive.SetActive(true);
        }
        else// if not
        {
            RecimController.instance.GameLevelComplete(PlayerPrefs.GetInt("score"), PlayerPrefs.GetInt("revive"), Time.time);
            //display the replay menu
            replay.SetActive(true);
        }
        //Store that game is "pause" (no more action)
        gamePause = true;
        //Able the game over menu
        GameOverUi.SetActive(true);
    }

    //Fonction if the player choose to replay
    public void Replay()
    {
        //Reboot the game
        SceneManager.LoadScene("Game");
    }

    //fonction if the player decide to revive
    public void Revive()
    {
        PlayerPrefs.SetInt("Revive", 1);
        //ReccimController.instance.GameReviveRequested();
        //Call fonction in the game script to revive
        //GameReviveRequestedComplete();
        FindObjectOfType<ScriptGame>().Revive();
        //unfreeze the game
        gamePause = false;
        //disable the Game over menu 
        GameOverUi.SetActive(false);
    }

    //Player don't want to revive
    public void DontRevive()
    {
        //Disable the revive menu
        revive.SetActive(false);
        dontRevive.SetActive(false);
        //Able the replay menu
        replay.SetActive(true);
    }

    //Fonction that is called to quit the app
    public void Quit()
    {
        RecimController.instance.GameLevelQuit();
        //Fonction that quit the app
        Application.Quit();
    }
}

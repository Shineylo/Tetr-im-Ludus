using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptMenu : MonoBehaviour
{
    //Variable that contain the Gameobject that display either the Mute or unmute button
    public GameObject on, off;

    //Fonction that is called at the start of the scene
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

    //Fonction that start the Game scene which start the game
    public void StartPlay()
    {
        //if (RecimController.instance.GameLevelStart())
        //{
        RecimController.instance.StartStockedGame();
            //Call the fonction to switch scene to Game
            SceneManager.LoadScene("Game");
        //}
    }

    //Fonction that change the disply to see if music is on or not
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

    //Fonction that is called to quit the app
    public void Quit()
    {
        //Fonction that quit the app
        Application.Quit();
    }
}

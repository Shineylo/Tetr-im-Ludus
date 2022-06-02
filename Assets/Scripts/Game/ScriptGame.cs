using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScriptGame : MonoBehaviour
{
    //variable that stock time
    private float previousTime;
    //variable that scock the speed of the fall
    private float fallTime;
    //height of the playable area
    public static int height = 20;
    //Maximum height before a gameover
    public static int max = 17;
    //width of the playable area
    public static int width = 10;
    //Grid that will stock the localisation of all the tetromino
    public Transform[,] grid = new Transform[width, height];
    //Variable that store the position of the finger on the screen
    private Vector3 startTouch;
    //Variable that store the position of the finger when it get off the screen
    private Vector3 endTouch;
    //Variable that store the number of line cleared on one move
    private int numLine;
    //Variable that store the distance the minimum distance bto be called a swipe
    private float swipeDistance;
    //Variable that store the position of the playable tetromino
    public Vector3 tetromino;
    //Variable that store if the game is over
    private bool gameOver = false;
    //variable that store the time remaining to play
    private float remainingTime = 180f;

    // Start is called before the first frame update
    void Start()
    {
        //setting the fall speed
        fallTime = 1.5f;
        //Setting the time scale
        Time.timeScale = 1f;
        //Setting the score of the player
        PlayerPrefs.SetInt("Score", 0);
        //setting the revive count 
        PlayerPrefs.SetInt("Revive", 0);
        //setting the minimum distance to be consider a swipe
        swipeDistance = Screen.width/ 15;
        //Spawn a new tetromino
        FindObjectOfType<ScriptSpawner>().NewTetromino();
        //start the coroutine for the timer
        StartCoroutine(updateTimer());
    }

    // Update is called once per frame
    void Update()
    {
        //check if the game is pause or not to able movement 
        if (!FindObjectOfType<ScriptUi>().gamePause)
        {
            //Reach the text of the score to display the current score of the user
            GameObject.Find("Score").GetComponent<Text>().text = "Score : " + PlayerPrefs.GetInt("Score").ToString();
            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch on the screen
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    //store the starting position of the touch
                    startTouch = touch.position;
                    //double store the position if the user remove the finger immediatly 
                    endTouch = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    //Store the position of the finger mooving
                    endTouch = touch.position;
                    if (Mathf.Abs(endTouch.x - startTouch.x) > swipeDistance || Mathf.Abs(endTouch.y - startTouch.y) > swipeDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(endTouch.x - startTouch.x) > Mathf.Abs(endTouch.y - startTouch.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            //Call the fonction to move the tetromino either left or right 
                            FindObjectOfType<ScriptTetromino>().Move(Mathf.RoundToInt((endTouch.x - startTouch.x) / swipeDistance));
                        }
                        else
                        {
                            if (endTouch.y < startTouch.y)  //If the movement was down
                            {   //Down swipe
                                FindObjectOfType<ScriptTetromino>().Fall();
                            }
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    if (endTouch.x == startTouch.x && endTouch.y == startTouch.y)
                    {//Check if it's just a touch
                        //Rotate the tetromino
                        FindObjectOfType<ScriptTetromino>().Rotation();
                    }
                    //Store the new position of the tetromino after the interaction of the user
                    tetromino = FindObjectOfType<ScriptTetromino>().transform.position;
                }
            }

            //If the time elapse is over the fallspeed then
            if (Time.time - previousTime > fallTime)
            {
                //The tetromino fall
                FindObjectOfType<ScriptTetromino>().Fall();
                //Store the time that it happen
                previousTime = Time.time;
            }
            //Check if there is any line after the fall
            CheckLines();
        }
    }

    //fonction that look if there is a line
    void CheckLines()
    {
        //Check every horizontale line in the grid
        for (int i = height - 1; i >= 0; i--)
        {
            if (Line(i))
            {
                //If there is a line then delete it
                DeleteLine(i);
                //Move everything above one down
                RowDown(i);
            }
        }
        //Let's see how many line have been found and add score accordingly to the number
        switch (numLine)
        {
            case 1:
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 50);
                break;
            case 2:
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 150);
                break;
            case 3:
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 400);
                break;
            case 4:
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 1000);
                break;
        }
        //reset the number of line for the newt time
        numLine = 0;
    }

    //fonction that check if there is any empty space in a horizontale line
    bool Line(int a)
    {
        for (int i = 0; i < width; i++)
        {//checking every scare
            if (grid[i, a] == null)
            {//if null then no line
                return false;
            }
        }
        //if all full then line
        return true;
    }

    //Fonction to delete the line
    void DeleteLine(int a)
    {
        for (int i = 0; i < width; i++)
        {//Get every square of the horizontal line
            //Destroy the square
            Destroy(grid[i, a].gameObject);
            //Erase the presence of a square in the grid
            grid[i, a] = null;
        }
        //Add that a line has been found and cleared
        numLine++;
    }

    //fonction to move everything above one down
    void RowDown(int a)
    {
        for (int j = a; j < height; j++)
        {//get every horizontale line
            for (int i = 0; i < width; i++)
            {//get every square
                if (grid[i, j] != null)
                {//check if the square is not empty
                    //tell the new position that it will be not empty
                    grid[i, j - 1] = grid[i, j];
                    //Old position is now empty
                    grid[i, j] = null;
                    //Move the part of the tetromino to the new location
                    grid[i, j - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    //coroutine that update the timer
    private IEnumerator updateTimer()
    {
        //check if the game is alive
        while (!gameOver)
        {
            //update the remaining time
            remainingTime -= Time.deltaTime;
            if(remainingTime <= 120f)
            {//If there is less then 120 sec remaining
                if(remainingTime <= 60f)
                {//If there is less then 60 sec remaining
                    if (remainingTime <= 0f)
                    {//If there is no time remaining
                        //Stop the game "victorious"
                        End();
                    }
                }
                else
                {
                    //speed up the fall 
                    fallTime = 0.7f;
                }
            }else
            {
                //speed up the fall 
                fallTime = 1.1f;
            }
            //Convert the remaingin time in a string with a definite structure
            string remainingTimestr = TimeSpan.FromSeconds(remainingTime).ToString("mm':'ss");
            //Change the display of the timer to the remaining time
            GameObject.Find("Timer").GetComponent<Text>().text = remainingTimestr;
            //tell the coroutine the script has been done to redo
            yield return null;
        }
    }

    //Fonction called if the player finish the game 
    public void End()
    {
        //game is finish
        gameOver = true;
        //Call fonction and telling it that it was not a loosing end
        FindObjectOfType<ScriptUi>().End(false);
        //Add bonus for victory
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") *2);
        //Display score
        GameObject.Find("TextScore").GetComponent<Text>().text = "Score : " + PlayerPrefs.GetInt("Score").ToString();
        // Display message
        GameObject.Find("TextEnd").GetComponent<Text>().text = "Fin de partie";

    }

    //Fonction called if the player loose
    public void GameOver()
    {
        //Game is finish
        gameOver = true;
        //Call fonction and telling it it was a loosing end
        FindObjectOfType<ScriptUi>().End(true);
        //Display score
        GameObject.Find("TextScore").GetComponent<Text>().text = "Score : " + PlayerPrefs.GetInt("Score").ToString();
        //display message
        GameObject.Find("TextEnd").GetComponent<Text>().text = "Perdu";
    }

    //Fonction if the player want a revive
    public void Revive()
    {
        for (int j = height-1; j>height-10; j--)
        {//get every horizontale line
            for (int i = 0; i < width; i++)
            {//get every square
                if (grid[i, j] != null)
                {//Remove any tetromino if it's not empty
                    Destroy(grid[i, j].gameObject);
                    grid[i, j] = null;
                }
            }
        }
    }
}

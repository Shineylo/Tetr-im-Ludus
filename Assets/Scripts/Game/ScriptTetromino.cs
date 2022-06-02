using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTetromino : MonoBehaviour
{
    //Variable that I use to store the rotation point for the tetromino
    public Vector3 rotationPoint;
    //Variable that store the previous position of the tetromino
    public Vector3 oldPos;

    //Fonction call at the start of the scene
    void Start()
    {
        //Store the position of the tetromino in the main script
        FindObjectOfType<ScriptGame>().tetromino = transform.position;
    }

    //Function that makes the tetromino move 
    public void Move(int dist) //Dist is an int that is retrieve when fonction is called
    {
        //Store the position of the tetromino before any modifiation
        oldPos = transform.position;
        //Modify the x position of the tetromino with the the variable dist that can be + or -
        transform.position = new Vector3(FindObjectOfType<ScriptGame>().tetromino.x + dist, transform.position.y, transform.position.z);
        //Check if the move si valid
        if (!ValidMove())
            //If not then move back to its original position
            transform.position = new Vector3(oldPos.x,oldPos.y,transform.position.z);
    }


    //Function that makes the tetromino rotate 
    public void Rotation()
    {
        //Rotate the tetromino from the rotation point i store in the variable -90°
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        //Check if the move si valid
        if (!ValidMove())
            //If not then move back to its original position
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
    }

    //Function that revert a rotation
    public void UnRotation()
    {
        //Rotate the tetromino from the rotation point i store in the variable -90°
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
    }

    //Function that make the tetromino fall
    public void Fall()
    {
        ////Move the position of the tetromino 1 unit down
        transform.position += new Vector3(0, -1, 0);
        //Check if valid
        if (!ValidMove())
        {
            //If not then move back to its original position
            transform.position -= new Vector3(0, -1, 0);
            //Check if it game over
            if (CheckGameOver()) {
                //If it is not then add the tetromino to the grid
                AddToGridd();
                //Add 10 point to the score
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 10);
                //Disable the Tetromino
                this.enabled = false;
                //Call the fonction to spawn a new tetromino
                FindObjectOfType<ScriptSpawner>().NewTetromino();
            }
            else //if It is game over
            {
                //Call the fonction for the game over
                FindObjectOfType<ScriptGame>().GameOver(); ;
            }
        }
    }

    //Function that add the tetromino on the grid to save the position where it's
    void AddToGridd()
    {
        //Loop to get all the square that make the tetromino
        foreach (Transform children in transform)
        {
            //Round the position of the square and store it
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            //Access the script of the game 
            ScriptGame game = GameObject.Find("MainCamera").GetComponent<ScriptGame>();
            //Store the position of the square in the grid
            game.grid[roundedX, roundedY] = children;
        }
    }

    //Fonction that send a bool if the tetromino is inside the playable area
    bool ValidMove()
    {
        //Access the script of the game 
        ScriptGame game = GameObject.Find("MainCamera").GetComponent<ScriptGame>();
        //Loop to get all the square that make the tetromino
        foreach (Transform children in transform)
        {
            //Round the position of the square and store it
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            //Condition that check if the square is inside the limits
            if (roundedX < 0 || roundedX >= 10 || roundedY < 0)
            {
                //if not then return that is not
                return false;
            }

            //Condition that check if there is not a square where the square is
            if (game.grid[roundedX, roundedY] != null)
            {
                //if not then return that is not
                return false;
            }
        }
        //Return that the space is free
        return true;
    }

    bool CheckGameOver()
    {
        //Access the script of the game 
        ScriptGame game = GameObject.Find("MainCamera").GetComponent<ScriptGame>();
        //Loop to get all the square that make the tetromino
        foreach (Transform children in transform)
        {
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if (roundedY > 17)
            {
                //if not then return that is not
                return false;
            }
        }
        return true;
    }
}

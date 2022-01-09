using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwapScript : MonoBehaviour
{
    public List<GameObject> ButtonList;
    private Vector2[] ButtonPositions;
    private Vector2[] TempPosition;
    private bool ChangingButtons;
    private GameObject soundScriptObj;

    //private int CurrentButton;

    // Start is called before the first frame update
    void Start()
    {
        soundScriptObj = GameObject.Find("SoundManager");
        initButtons(); 
    }

    // Update is called once per frame
    void Update()
    {
        //If not currently Updating Buttons
        if (!ChangingButtons)
        {
            //check for input
            if (Input.GetKeyDown(KeyCode.E))
            {
                //If input update buttons, set changing button to true
                UpdateButtons();
                ChangingButtons = true;
                //Disable placement in player script
                GameObject.Find("Pointer").GetComponent<StaffMovement>().ready = false;
            }
        }
        else
        {
            //Move buttons
            int tempCount = 0;
            int movedCount = 0;
            bool finishedMoving = false;

            //get current position, move towards appropriate Button position, check if finished moving
            foreach (GameObject Button in ButtonList)
            {
                Vector2 CurrentPosition = new Vector2(Button.transform.localPosition.x, Button.transform.localPosition.y);
                finishedMoving = MoveButton(CurrentPosition, ButtonPositions[tempCount], tempCount);
                tempCount++;
                if (finishedMoving == true) //tally finished moving
                {
                    movedCount++;
                }
            }

            //check tally and back to checking for input
            if (movedCount == ButtonPositions.Length)
            {
                ChangingButtons = false;
                GameObject.Find("Pointer").GetComponent<StaffMovement>().ready = true;
                soundScriptObj.GetComponent<SoundScript>().playItemSwap();
            }
        }
    }
    
    void initButtons()
    {
        //Check all child objects of the canvas/get an existing list of button objects from somewhere - handled by engine for now

        //Instantiate button pos arrays based on button list length
        ButtonPositions = new Vector2[ButtonList.Count];
        TempPosition = new Vector2[ButtonList.Count];

        //For each record the position of the button for later use
        int tempCount = 0;
        foreach(GameObject Button in ButtonList)
        {
            ButtonPositions[tempCount] = Button.gameObject.transform.localPosition;
            tempCount++;
        }
    }

    void UpdateButtons()
    {
        //CurrentButton = (CurrentButton + 1) % ButtonList.Count; //Should cycle reference position 

        //Take each of the buttons and move them into the next position
        int tempCount = 0;
        foreach (GameObject Button in ButtonList)
        {
            //Set Current position variable to compare against 2D array
            Vector2 CurrentPosition = new Vector2(Button.transform.localPosition.x, Button.transform.localPosition.y);

            //populate/reset array of positions
            TempPosition[tempCount] = CurrentPosition;

            //Debug.Log("curpos: " + TempPosition[tempCount].ToString("F3"));

            tempCount++;
        }

        tempCount = 0;
        //Shift position data by 1
        for (int i = 0; i < ButtonList.Count; i++)
        { 
            tempCount++;
            tempCount = tempCount % ButtonPositions.Length;
            ButtonPositions[i] = TempPosition[tempCount];
        }

    }

    bool MoveButton(Vector2 OrigPosition, Vector2 NewPosition, int ButtonNumber)
    {
        //init move pos
        Vector2 MovePosition = new Vector2(0.0f, 0.0f);

        //Check if same, if so return true
        if (OrigPosition.Equals(NewPosition))
        {
            return true; //button set
        }

        //Speed value
        float speed = 1.0f;

        //Create distance vector
        Vector2 DistV = new Vector2(OrigPosition.x - NewPosition.x, OrigPosition.y - NewPosition.y);

        //create unit vector
        Vector2 UnitV = DistV.normalized;

        //multiple UV by speed for velocity
        Vector2 VelcV = UnitV * speed;

        //check if velocity.magnitude>distance.magnitude, if so set position to new
        if (VelcV.magnitude >= DistV.magnitude)
        {
            MovePosition = new Vector2(ButtonList[ButtonNumber].gameObject.transform.localPosition.x, ButtonList[ButtonNumber].gameObject.transform.localPosition.y); //Possible bug here...?
            ButtonList[ButtonNumber].gameObject.transform.localPosition = MovePosition;
            return true; //button set
        }
        else
        {
            //Debug.Log("Original: " + OrigPosition.ToString("F3") + " Velocity: " + VelcV.ToString("F3"));
            //add velocity to origPos, and set to movePos
            MovePosition = OrigPosition - VelcV;
            ButtonList[ButtonNumber].gameObject.transform.localPosition = MovePosition;
        }
        

        return false; //not finished moving
    }
}

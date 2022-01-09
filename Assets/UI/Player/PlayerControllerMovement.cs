using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMovement : MonoBehaviour
{
    private GameObject soundScriptObj;
    int moveSoundTimer;
    bool moveSoundCD;

    float HInput;
    float VInput;

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        soundScriptObj = GameObject.Find("SoundManager");
        speed = 10.0f;
        speed = speed / 100.0f;

        moveSoundTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HInput = Input.GetAxis("Horizontal");
        VInput = Input.GetAxis("Vertical");

        //Cooldown to prevent constant can't move feedback
        if (moveSoundCD)
        {
            moveSoundTimer++;
            if (moveSoundTimer == 30)
            {
                moveSoundCD = false;
                moveSoundTimer = 0;
            }
        }
        

        UpdatePosition();
    }


    void UpdatePosition()
    {
        float deltaX = 0.0f;
        float deltaZ = 0.0f;

        //X 30.635, 3.5
        //z 3.04, 25.858

        float rightBound = 8.25f; //8.5f
        float leftBound = 25.95f; //25.635
        float lowerBound =  4.0f; // 3.04f
        float upperBound = 24.95f; //25.858f
        //float HCenter = 17.0675f;
        //float VCenter = 14.449f;

        

        Vector3 pos = transform.position;

        bool CanMove = true;

        //between the two boundaries
        if ((pos.x < leftBound && HInput > 0.0f) || ( pos.x > rightBound && HInput <0.0f))
        {
            deltaX = HInput * speed;
        }
        else if(HInput != 0)
        {
            Debug.Log("Stuck");
            CanMove = false;
        }
        if ((pos.z < upperBound && VInput > 0.0f) || (pos.z > lowerBound && VInput < 0.0f))
        {
            deltaZ = VInput * speed;
        }
        else if (VInput != 0)
        {
            Debug.Log("Stuck");
            CanMove = false;
        }
        if (!CanMove)
        {
            if (!moveSoundCD)
            {
                soundScriptObj.GetComponent<SoundScript>().playEdgeOfMap();
                moveSoundCD = true;
            }
        }

        //May input zoom element here....

        transform.position = transform.position + new Vector3(deltaX, 0.0f, deltaZ);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaffMovement : MonoBehaviour
{
    //obviously in an ideal scenario we would attach the triggers and position to actual controllers, but are simulating them here
    float speed = 0.125f;
    bool isSelected = false;
    Vector3 initPositionOfController, initPositionOfPlayer;
    bool trigger;
    public bool ready;
    public GameObject goodPrefab;
    public GameObject badPrefab;
    public Material selectedMaterial;
    private GameObject soundScriptObj;
    bool MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        soundScriptObj = GameObject.Find("SoundManager");

        //record initial positions of player/controller
        initPositionOfController = this.gameObject.transform.localPosition;
        initPositionOfPlayer = this.gameObject.transform.parent.position;

        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        float delx = Input.GetAxis("Mouse X");
        float delz = Input.GetAxis("Mouse Y");
        //float yRotation = Input.GetAxis("Mouse ScrollWheel");

        //R to reset controller position
        bool resPosition = Input.GetKey(KeyCode.R);

        //F to replace materials
        trigger = Input.GetKey(KeyCode.F);
        MainMenu = Input.GetKey(KeyCode.Escape);

        //Move 2D in front of player - update later?
        this.transform.position += speed * new Vector3(delx, 0, delz);
        //Vector3 screenMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //this.transform.localPosition = new Vector3(screenMousePos.x, 0.0f, screenMousePos.y);

        if (resPosition)
        {
            this.gameObject.transform.localPosition = initPositionOfController;
        }

        if (trigger && ready)
        {
            RayCasting();
        }

        if (MainMenu)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void readySwitch()
    {
        ready = !ready;
        //Debug.Log("Rcheck1 " + ready.ToString());      
    }

    IEnumerator switchDelay()
    {
        yield return new WaitForSeconds(0.1f);
        readySwitch();
        //Debug.Log("Rcheck2  " + ready.ToString());
    }

    IEnumerator particleDelay(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(obj);
    }

    //Doesnt need many changes
    void RayCasting()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, this.gameObject.transform.up, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow, 0.01f, true);
            //Debug.Log("Did Hit");

            if (isSelected == false)
            {
                //select & grab
                isSelected = true;
                //hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
                //hit.transform.gameObject.transform.SetParent(this.transform);
                GameObject obj = hit.transform.gameObject;

                //Terrain check to prevent deletion of mountains surrounding
                if (obj.transform.parent.name == "Terrain")
                {

                    //get currently assigned material
                    GameObject objChild = obj.transform.GetChild(0).gameObject; //obj.GetComponentInChildren<Renderer>().material;
                    Material childMaterial = objChild.GetComponent<Renderer>().material;
                    //Debug.Log(obj.name);
                    //check user selected material
                    if (childMaterial.name.Substring(0,4) != selectedMaterial.name.Substring(0, 4))
                    {
                        //Debug.Log("Child: " + childMaterial.name + " Selected: " + selectedMaterial.name);
                        Debug.Log("change material: childname" + childMaterial.name + " selected name: " + selectedMaterial.name);
                        //childMaterial = selectedMaterial;
                        readySwitch();
                        StartCoroutine("switchDelay");

                        //apply user selected material
                        objChild.GetComponent<Renderer>().material = selectedMaterial;
                        obj.gameObject.GetComponent<HexAI>().updateNearbyHexes();
                        Debug.Log("change material: childname" + childMaterial.name + " selected name: " + selectedMaterial.name);
                        //Call good sound from sound manager
                        soundScriptObj.GetComponent<SoundScript>().playCanPlace();
                        //show good particle effect, call coroutine to delete
                        GameObject gPart = GameObject.Instantiate(goodPrefab, GameObject.Find("Particles").transform);
                        //Debug.Log(goodPart.gameObject.name);
                        //Debug.Log("is active :"  + goodPart.gameObject.active.ToString());
                        StartCoroutine("particleDelay", gPart);
                    }
                    else
                    {
                        //Is same material

                        //Debug.Log("same mat");

                        readySwitch();
                        StartCoroutine("switchDelay");
                        //Play bad sound from sound manager
                        soundScriptObj.GetComponent<SoundScript>().playCantPlace();
                        //show good particle effect, call coroutine to delete
                        GameObject bPart = GameObject.Instantiate(badPrefab, GameObject.Find("Particles").transform);
                        //Debug.Log("bpart: " + name);
                        StartCoroutine("particleDelay", bPart); 
                    }

                }
            }
            else
            {
                //release object
                isSelected = false;
                //hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
                //hit.transform.gameObject.transform.SetParent(null);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up * 1000, Color.red, 0.01f, true);
            //Debug.Log("Did not Hit");
        }

    }
}

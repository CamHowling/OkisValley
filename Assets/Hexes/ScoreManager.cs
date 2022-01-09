using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    List<GameObject> HexList;
    HexAI.HexStats totalStats;
    GameObject scoreObj;
    // Start is called before the first frame update
    void Start()
    {
        HexList = new List<GameObject>();
        totalStats = new HexAI.HexStats();
        scoreObj = GameObject.Find("ScoreText");

        //build list of hexes
        List<GameObject> tmpList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Hex"));
        //Debug.Log(tmpList.Count.ToString("F1"));
        foreach (GameObject obj in tmpList)
        {
            
            if (obj.transform.parent.name == "Terrain")
            {
                //Debug.Log(obj.name);
                //Debug.Log(obj.transform.parent.name);
                HexList.Add(obj);
            }
        }

    }

    void clearStats()
    {
        totalStats.humans = 0;
        totalStats.livestock = 0;
        totalStats.food = 0;
        totalStats.stone = 0;
        totalStats.iron = 0;
        totalStats.gold = 0;
        totalStats.worship = 0;
        totalStats.happiness = 0;
        totalStats.productivity = 0;
    }

    void updateScoreText()
    {
        string scoreText = "";

        int humans = totalStats.humans;
        int worship = totalStats.worship;
        int happy = totalStats.happiness;
        int prod = totalStats.productivity;
        
        //if (worship > 0)
        //{
        //    Debug.Log("worship value: " + worship.ToString("F5"));
        //}

        scoreText = "Humans: " + humans.ToString() + "\nWorship: " + worship.ToString() + "\nhappiness: " + happy.ToString() + "\nproductivity: " + prod.ToString();
        scoreObj.GetComponent<Text>().text = scoreText;

    }

    // Update is called once per frame
    void Update()
    {
        clearStats();
        HexAI.HexStats tmpStats = new HexAI.HexStats();
        foreach (GameObject obj in HexList)
        {
            tmpStats = obj.GetComponent<HexAI>().localStats;
            //Debug.Log(tmpStats.humans);
            totalStats.humans += tmpStats.humans;
            totalStats.livestock += tmpStats.livestock;
            totalStats.food += tmpStats.food;
            totalStats.stone += tmpStats.stone;
            totalStats.iron += tmpStats.iron;
            totalStats.gold += tmpStats.gold;
            totalStats.worship += tmpStats.worship;
            totalStats.happiness += tmpStats.happiness;
            totalStats.productivity += tmpStats.productivity;
        }
        //Debug.Log(totalStats.humans.ToString());
        //Call score update
        updateScoreText();
    }
}

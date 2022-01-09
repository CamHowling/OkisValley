using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAI : MonoBehaviour
{
    //stats
    public HexStats localStats;
    public HexStats nearbyStats;

    private Material currentMat;
    private Material lastUpdateMat;
    private List<GameObject> nearbyHexes;

    public struct HexStats{
        public int humans;
        public int livestock;
        public int food;
        public int stone;
        public int iron;
        public int gold;
        public int worship;
        public int happiness;
        public int productivity;
    }

    // Start is called before the first frame update
    void Start()
    {

        currentMat = GetComponentInChildren<Renderer>().material;
        lastUpdateMat = GetComponentInChildren<Renderer>().material;

        //LOL rip CPU
        nearbyHexesList();

        calculateStats();
    }

    // Update is called once per frame
    void Update()
    {
        //Optimisation
        currentMat = GetComponentInChildren<Renderer>().material;
        if (currentMat != lastUpdateMat)
        {
            //Debug.Log("calc stats");
            initLocalStats();
            calculateStats();
            updateNearbyHexes();
            calculateHappiness();
            calculateProductivity();
            calculateWorship();
        }
    }

  void initLocalStats()
   {
        localStats.humans = 0;
        localStats.livestock = 0;
        localStats.food = 0;
        localStats.stone = 0;
        localStats.iron = 0;
        localStats.gold = 0;
        localStats.worship = 0;
        localStats.happiness = 0;
        localStats.productivity = 0;
    }

    void calculateStats()
    {
        //Debug.Log(currentMat.name.ToString());
        //Check mat, calculate various elements based on material type
        switch (currentMat.name.ToString().Substring(0,4))
        {
            //Base, 3-5 modifers
            
            case ("Farm"):
                //Settlement
                localStats.humans = 3;
                localStats.food = 3;
                if (nearbyStats.food > 8)
                {
                    //Debug.Log("Too much food");
                    localStats.humans += 2;
                }
                if (nearbyStats.livestock > 5)
                {
                    localStats.humans += 2;
                    localStats.food += 1;
                }
                if (nearbyStats.stone > 0)
                {
                    localStats.humans -= 1;
                }
                if (nearbyStats.gold > 0)
                {
                    localStats.humans += 1;
                }

                break;
            case ("Past"):
                //Livestock and food generation
                localStats.livestock = 3;
                localStats.food = 1;
                if (nearbyStats.food > 5)
                {
                    localStats.livestock += 2;
                }
                break;
            case ("Lake"):
                //Food
                localStats.food = 3;
                if (nearbyStats.humans > 5)
                {
                    localStats.livestock += 2;
                    localStats.food += 2;
                }
                break;
            case ("Wate"):
                //Food
                //Debug.Log("wata");
                localStats.food = 3;
                if (nearbyStats.humans > 5)
                {
                    localStats.food += 2;
                }
                break;
            case ("Dirt"):
                //Low food - removed for now, can add later
                //localStats.food = 1;
                localStats.stone = 1;
                break;
            case ("Diam"):
                //Low iron
                localStats.stone = 3;
                localStats.iron = 1;
                if (nearbyStats.stone > 0)
                {
                    localStats.iron += 1;
                }
                break;
            case ("Iron"):
                //Iron
                localStats.iron = 3;
                if (nearbyStats.stone > 0)
                {
                    localStats.iron += 2;
                }
                break;
            case ("Gold"):
                //Gold
                localStats.gold = 1;
                if (nearbyStats.iron > 0)
                {
                    localStats.gold += 1;
                }
                break;
            default:
                //do nothing
                //Debug.Log("Cases failing");
                break;
        }

        //Set up for later
        lastUpdateMat = currentMat;
    }

    void defaultHexStats()
    {
        nearbyStats.humans= 0;
        nearbyStats.livestock = 0;
        nearbyStats.food = 0;
        nearbyStats.stone = 0;
        nearbyStats.iron = 0;
        nearbyStats.gold = 0;
        nearbyStats.worship = 0;
        nearbyStats.happiness = 0;
        nearbyStats.productivity = 0;
    }

    //Builds list of nearby hexes, and then calculates a total of their resources
    void nearbyHexesList()
    {
        //Reset
        defaultHexStats();

        //Sphere cast from pos
        RaycastHit[] HitArray = Physics.SphereCastAll(new Ray(transform.position, transform.up), 1.5f);
        List<GameObject> HitList = new List<GameObject>();

        foreach(RaycastHit Hit in HitArray)
        {

            //Debug.Log(Hit.point.ToString("F2"));
            GameObject obj = Hit.transform.gameObject;

            //Exclude self
            if (obj != this.transform.gameObject)
            {
                //Prevent duplicates
                if (!HitList.Contains(obj))
                {
                    HitList.Add(obj);
                    HexStats hitStats = new HexStats();
                    hitStats = obj.GetComponent<HexAI>().localStats;
                    // Find("Pointer").GetComponent<StaffMovement>().ready = false;
                    nearbyStats.humans += hitStats.humans;
                    nearbyStats.livestock += hitStats.livestock;
                    nearbyStats.food += hitStats.food;
                    nearbyStats.stone += hitStats.stone;
                    nearbyStats.iron += hitStats.iron;
                    nearbyStats.gold += hitStats.gold;
                    nearbyStats.worship += hitStats.worship;
                    nearbyStats.happiness += hitStats.happiness;
                    nearbyStats.productivity += hitStats.productivity;
                    //if (nearbyStats.food > 8)
                    //{
                        //Debug.Log("livestock stats " + nearbyStats.livestock + "obj: " + obj.name);
                    //}
                }

            }
        }
       
    }

    //Forces nearby hexes to update their stats when a local hex is changed
    public void updateNearbyHexes()
    {
        //int i = 0;
        //Sphere cast from pos
        RaycastHit[] HitArray = Physics.SphereCastAll(new Ray(transform.position, transform.up), 1.5f); //Currently bugged
        //Debug.Log("Array: " + HitArray.Length.ToString());
        foreach (RaycastHit Hit in HitArray)
        {
            
            GameObject obj = Hit.transform.gameObject;
            if (obj != this.transform.gameObject)
            {
                //Debug.Log("obj count: " + i);
                //i++;
                obj.GetComponent<HexAI>().nearbyHexesList();
            }
        }
    }

    void calculateWorship()
    {
        int wShip = (localStats.humans / (1 + nearbyStats.gold)) * (1 + localStats.happiness);

        //Factor in productivity and happiness, maybe change calculation to include gold but not livestock and food


        localStats.worship = wShip;
        if (wShip == 0)
        {
            //Debug.Log("no worship value returned");
        }
        else
        {
            //Debug.Log(wShip.ToString("F2"));
        }
    }

    void calculateHappiness()
    {
        //remove gold?
        //only cares about how easy it is to eat
        //too many nearby humans is bad
        int happy = (1 + nearbyStats.gold) / (1+localStats.humans) +  (1 + localStats.food + nearbyStats.food / (1+localStats.humans));

        localStats.happiness = happy;
        //Debug.Log("happ " + happy.ToString());
    }

    void calculateProductivity()
    {
        int prod = localStats.humans * (1 + nearbyStats.livestock) * (1 + nearbyStats.stone + (nearbyStats.iron * 2)) * (1 + localStats.happiness);
        localStats.productivity = prod;
        //Debug.Log("prod " + prod.ToString());
    }

}

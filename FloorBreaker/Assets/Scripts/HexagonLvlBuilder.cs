using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HexagonLvlBuilder : MonoBehaviour
{
    public GameObject hexagon, UI;
    public PlayerController player;
    HexagonLvlBuilder hexagonBuild;
    public Slider difficulty, cameraSensitivty;
    public MovingAngel angel;

    public List<GameObject> hexagons = new List<GameObject>();
    public List<GameObject> usedHexagons = new List<GameObject>();
    List<GameObject> amagedons = new List<GameObject>();

    [Range(1,10)]
    public int difficultyLvl; // 0.25f defualt value
    bool lvl1, lvl2, lvl3;
    CameraController camCon;
    float cameraSensitivityLvl;
    public Material matRed, matDef;

    private void Awake()
    {
        camCon = GetComponent<CameraController>();
        hexagonBuild = this;
        lvl1 = true;
        lvl2 = lvl3 = false;
        difficulty.onValueChanged.AddListener(delegate { Hi(); });
        cameraSensitivty.onValueChanged.AddListener(delegate { Hi(); });
        cameraSensitivty.value = cameraSensitivityLvl = 0.35f;
    }

    public void Hi()
    {
       difficultyLvl = (int)difficulty.value;
       cameraSensitivityLvl = (int)cameraSensitivty.value;
       camCon.cameraSensitivity = cameraSensitivty.value * 10;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Reset")) PressStart();
        if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Menu"))
        {
            if (UI.activeInHierarchy) UI.SetActive(false);
            else UI.SetActive(true);// I don't have a way to stop the game functionality from running when returning to the menu!
        }
    }

    public void PressStart ()
    {
        CancelInvoke();
        UI.SetActive(false);
        angel.TimerReset();
        player.ResetPlayerPos();
        ResetPillarList();
        if (lvl1 && !lvl2)
        {
            StartOflvlChaos(45, 115, 3, 1);
        }
        else if (lvl2 && !lvl3) // next time buddy
        {
            StartOflvlChaos(75, 150, 2, 3);
        }
        else if (lvl3) StartOflvlChaos(120, 300, 1, 5);
	}

    public void PlayCustomLvl()
    {
        player.ResetPlayerPos();
        angel.TimerReset();
        CancelInvoke();
        StartOflvlChaos(difficultyLvl * 30, difficultyLvl * 38, 5 / difficultyLvl, difficultyLvl);
    }

    private void ResetPillarList()
    {
        foreach (GameObject item in usedHexagons)
        {
            item.GetComponent<PillarFunctionality>().StopEveryThing();
        }
        foreach (GameObject item in hexagons)
        {
            item.GetComponent<PillarFunctionality>().StopEveryThing();
        }

        for (int i = 0; i < usedHexagons.Count; i++)
        {
            hexagons.Add(usedHexagons[i]); 
            usedHexagons.Remove(usedHexagons[i]); // this can not posibly work!! It did!! I do tend to use "foreach" over "for" all the time
        }
        for (int i = 0; i < hexagons.Count; i++)
        {
            hexagons[i].GetComponent<PillarFunctionality>().fall = hexagons[i].GetComponent<PillarFunctionality>().voidSucker
                = hexagons[i].GetComponent<PillarFunctionality>().begone = false;
            hexagons[i].transform.position = new Vector3(hexagons[i].transform.position.x, 0,hexagons[i].transform.position.z);
        }
        for (int i = 0; i < amagedons.Count; i++) // I'm not seing this take effect
        {
            amagedons[i].GetComponent<MeshRenderer>().material = matDef; // You can tell I thought ahead on this one...
            amagedons.Clear();
        }

        /*foreach (GameObject item in usedHexagons)
        {
            hexagons.Add(item);
            usedHexagons.Remove(item);
        }*/
    }

    void StartOflvlChaos(int pillarAffectionMin, int pillarAffectionMax, float repeatRate, int dangerPillar)
    {
        int amount = Random.Range(pillarAffectionMin, pillarAffectionMax);
        InvokeRepeating("Chaos", 3, repeatRate);
        DangerPillar(dangerPillar);
        for (int i = 0; i < amount; i++) 
        {
            int index = Random.Range(1, hexagons.Count); // make sure you can't call the same pillar more then once
            hexagons[index].transform.position = new Vector3(hexagons[index].transform.position.x, Random.Range(-0.25f,1.6f), hexagons[index].transform.position.z);
        }
    }

    void Chaos() 
    {
        if (hexagons.Count < 13) return;
        VoidPillar(4, 12); // the void is still imensly strong at close range, godangit
        if (hexagons.Count < 11) return;
        BegonePillar(2,10);
        if (hexagons.Count < 20) return;
        FallPillar(12,19);
    }

    public void FallPillar(int minRange, int maxRange)
    {
        for (int i = 0; i < Random.Range(minRange, maxRange); i++) 
        {
            int index = Random.Range(1, hexagons.Count);
            if (!hexagons[index].activeInHierarchy)
            {
                hexagons[index].GetComponent<PillarFunctionality>().fall = true;
                usedHexagons.Add(hexagons[index]);
                hexagons.Remove(hexagons[index]);
            }
        }
    }

    void BegonePillar(int minRange, int maxRange)
    {
        for (int i = 0; i < Random.Range(minRange, maxRange); i++)
        {
            int index = Random.Range(1, hexagons.Count);
            if (!hexagons[index].activeInHierarchy)
            {
                hexagons[index].GetComponent<PillarFunctionality>().begone = true;
                usedHexagons.Add(hexagons[index]);
                hexagons.Remove(hexagons[index]);
            }
        }
    }

    void VoidPillar(int minRange, int maxRange)
    {
        for (int i = 0; i < Random.Range(minRange, maxRange); i++)
        {
            int index = Random.Range(1, hexagons.Count); // make notice you have enough pillars to call! That you don't run out off them
            if (hexagons[index].activeInHierarchy)
            {
                int indexu = Random.Range(1, 5);
                if (indexu == 3) hexagons[index].GetComponent<PillarFunctionality>().voidSucker = true;
                hexagons[index].GetComponent<PillarFunctionality>().StartCoroutine("Fall", Random.Range(0.1f, 0.2f));
                usedHexagons.Add(hexagons[index]);
                hexagons.Remove(hexagons[index]);
            }
        }
    }

    void DangerPillar(int amount) // I'm not reseting them on restart lvl, so there keeps coming more for each restart
    {
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(1, hexagons.Count);
            amagedons.Add(hexagons[i]);
            hexagons[index].GetComponent<PillarFunctionality>().Amargedon(hexagonBuild);
            hexagons[index].GetComponent<MeshRenderer>().material = matRed; // this way I'm only cache the value once! Running out off time tough
        }
    }
}
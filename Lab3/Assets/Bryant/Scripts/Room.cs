using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class Room : MonoBehaviour
{
    public bool onPath, isEntrance, isExit;
    public int gridX, gridY;

    public List<Room> neighbours;
    public List<string> openings = new List<string>();
    public GameObject[] bases;



    TMP_Text roomLabel;
    void Start()
    {
        
    }
    private void Awake()
    {
        roomLabel = GetComponentInChildren<TMP_Text>();
    }
    public void UpdateInfo() 
    {
        //roomLabel.text = ("[" + gridX + "," + gridY + "]");
        if (onPath) 
        {
            roomLabel.color = Color.yellow;
        }
        if (isEntrance) 
        {
            roomLabel.color = Color.green;
        }
        if (isExit) 
        {
            roomLabel.color = Color.red;
        }
    }
    public GameObject PickBase() 
    {
        GameObject roomBase;
        List<GameObject> possibleBases = new List<GameObject>();
        for (int i = 0; i < bases.Length - 1; i++) 
        {
            if (onPath)
            {
                RoomBaseInfo rbInfo = bases[i].GetComponent<RoomBaseInfo>();
                bool containsAllOpenings = false;
                foreach (string k in openings) 
                {
                    if (rbInfo.roomType.Contains(k) == false)
                    {
                        containsAllOpenings = false;
                        break;
                    }
                    else 
                    {
                        containsAllOpenings = true;
                    }
                }
                if (containsAllOpenings) 
                {
                    possibleBases.Add(bases[i]);
                }

            }
            else 
            {
                possibleBases.Add(bases[i]);
            }
        }
        int roomBaseIndex = Random.Range(0, possibleBases.Count);

        roomBase = possibleBases[roomBaseIndex];
        return roomBase;
    }
    public void PlaceRoom() 
    {
        GameObject roomBase = PickBase();
        roomBase = (GameObject)Instantiate(roomBase);
        roomBase.transform.parent = transform;
        roomBase.transform.localPosition = Vector3.zero;
    }
}

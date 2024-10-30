using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int levelWidth, levelHeight, roomSize;
    public Material gridMat1, gridMat2;
    GameObject levelGrid;

    [Header("Tracking Rooms")]
    public Room currentRoom;
    public GameObject player;

    [Header("From Rooms")]
    public GameObject roomPrefab, levelRooms;
    public Room[,] rooms;


    void Start()
    {
        GenerateGrid();
        CreateRooms();
        CreatePath();
    }
    void GenerateGrid()
    {
        levelGrid = new GameObject("_LevelGrid");
        levelGrid.transform.position = new Vector3((-levelWidth / 2) * roomSize, 0, (levelHeight / 2) * roomSize);
        Material activeGridMat = gridMat1;
        for (int y = 0; y < levelWidth; y++)
        {
            for (int x = 0; x < levelHeight; x++)
            {
                GameObject floorTile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                floorTile.transform.parent = levelGrid.transform;
                floorTile.transform.localScale = new Vector3(roomSize, roomSize, 1);
                floorTile.transform.localEulerAngles = new Vector3(90, 0, 0);
                floorTile.transform.localPosition = new Vector3(x * roomSize, 0, -y * roomSize);
                //alternate
                floorTile.GetComponent<MeshRenderer>().material = activeGridMat;
                if (activeGridMat == gridMat1)
                {
                    activeGridMat = gridMat2;
                }
                else
                {
                    activeGridMat = gridMat1;
                }
            }
        }
    }
    void CreateRooms()
    {
        levelRooms = new GameObject("_LevelRooms");
        levelRooms.transform.position = new Vector3((-levelWidth / 2) *
        roomSize, 0, (levelHeight / 2) * roomSize);
        rooms = new Room[levelWidth, levelHeight];
        for (int y = 0; y < levelWidth; y++)
        {
            for (int x = 0; x < levelHeight; x++)
            {
                GameObject room = (GameObject)Instantiate(roomPrefab, levelRooms.transform);
                room.name = "Room : " + x + "," + y;
                room.transform.localPosition = new Vector3(x * roomSize, 0, -y * roomSize);
                Room roomInfo = room.GetComponent<Room>();
                roomInfo.gridX = x;
                roomInfo.gridY = y;
                roomInfo.UpdateInfo();
                rooms[x, y] = roomInfo;
            }
        }
    }
    void CreatePath() 
    {
        int entranceX = Random.Range(0, levelWidth - 1);
        currentRoom = rooms[entranceX, 0];
        Room roomInfo = rooms[entranceX, 0].GetComponent<Room>();
        roomInfo.onPath = true;
        roomInfo.isEntrance = true;
        roomInfo.UpdateInfo();
        //put player in entrance
        player.transform.position = roomInfo.transform.position;
        FindNextRoom();
    }
    public void FindNextRoom() 
    {
        currentRoom.neighbours = GetNeighbours(currentRoom);
        //make sure that there are neighbours
        if (currentRoom.neighbours.Count > 0) 
        {
            //select random room that is neighbours
            int nextIndex = Random.Range(0, currentRoom.neighbours.Count);
            Room nextRoom = currentRoom.neighbours[nextIndex];
            nextRoom.onPath = true;
            //deternmine room openings
            if(nextRoom.gridX == currentRoom.gridX - 1) 
            {
                //Right -> Left
                currentRoom.openings.Add("L");
                nextRoom.openings.Add("R");
            }
            if (nextRoom.gridX == currentRoom.gridX + 1) 
            {
                //Left -> Right
                currentRoom.openings.Add("R");
                nextRoom.openings.Add("L");
            }
            if (nextRoom.gridY == currentRoom.gridY + 1)
            {
                //Up -> Down
                currentRoom.openings.Add("D");
                nextRoom.openings.Add("U");
            }
            nextRoom.UpdateInfo();
            currentRoom = nextRoom;
            FindNextRoom();
        }
        else
        {
            currentRoom.isExit = true;
            currentRoom.UpdateInfo();
            PlaceRooms();
        }
    }
    public List<Room> GetNeighbours(Room room) 
    {
        List<Room> neighbours = new List<Room>();
        //check left neighbour
        if (room.gridX - 1 >= 0) 
        {
            Room leftRoom = rooms[room.gridX - 1, room.gridY];
            if (!leftRoom.onPath)
            {
                neighbours.Add(leftRoom);
            }
        }
        //check right neighbour
        if (room.gridX + 1 < levelWidth) 
        {
            Room rightRoom = rooms[room.gridX + 1, room.gridY];
            if (!rightRoom.onPath) 
            {
                neighbours.Add(rightRoom);
            }
        }
        //check bottom Neighbour
        if (room.gridY < levelHeight - 1) 
        {
            Room downRoom = rooms[room.gridX, room.gridY + 1];
            if (!downRoom.onPath) 
            {
                neighbours.Add(downRoom);
            }
        }
        return neighbours;
    }
    void PlaceRooms() 
    {
        foreach (Room room in rooms) 
        {
            room.PlaceRoom();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //Variables
    //[SerializeField] private int numberOfRooms = 16;

    private int chosenMap;

    private GameObject[] combatRooms, puzzleRooms, shopRooms;
    private List<GameObject> combatRoomList = new List<GameObject>();
    private List<GameObject> puzzleRoomList = new List<GameObject>();
    private List<GameObject> shopRoomList = new List<GameObject>();
    private GameObject startRoom, tutorialRoom, endRoom;
    private Dictionary<Vector2Int, GameObject> roomDictionary = new Dictionary<Vector2Int, GameObject>();
    private int[] roomPositions = new int[4];

    /*private enum typeOfRoom{
        None, //0
        StartRoom, //1
        TutorialRoom, //2
        ShopRoom, //3
        BattleRoom, //4
        PuzzleRoom, //5
        EndRoom //6
    }*/

    //Map - nxn matrix of rooms
    //16 rooms, 8 combat, 3 puzzle, 2 shop, 1 boss, 1 start, 1 tutorial
    private int[,] Map1 = new int[6,6] { 
    {1,2,0,4,0,0}, 
    {0,4,5,4,4,3},
    {0,0,4,0,0,0},
    {0,3,5,4,0,0},
    {0,0,0,4,5,4},
    {0,0,0,0,0,6}
    };
    private int[,] Map2 = new int[6,6] { 
    {1,2,0,4,0,0},
    {0,4,5,4,4,0},
    {0,0,4,0,0,0},
    {0,4,5,3,0,0},
    {0,0,0,4,5,4},
    {0,0,0,0,0,6}
    };
    private int[,] Map3 = new int[6,6] { 
    {1,2,0,4,0,0},
    {0,4,5,4,4,0},
    {0,0,4,0,0,0},
    {0,4,5,3,0,0},
    {0,0,0,4,5,4},
    {0,0,0,0,0,6}
    };

    // Start is called before the first frame update
    void Start()
    {
        //will switch to FindGameObjectsWithTag later
        //look for startRoom gameobject
        startRoom = GameObject.Find("StartRoom");
        //look for tutorialRoom gameobject
        tutorialRoom = GameObject.Find("TutorialRoom");
        //look for endRoom gameobject
        endRoom = GameObject.Find("EndRoom");
        //look for combatRoom gameobjects
        combatRooms = GameObject.FindGameObjectsWithTag("CombatRoom");
        Debug.Log("Combat rooms: " + combatRooms.Length + " found");
        //look for puzzleRoom gameobjects
        puzzleRooms = GameObject.FindGameObjectsWithTag("PuzzleRoom");
        Debug.Log("PuzzleRooms: " + puzzleRooms.Length + " found");
        //look for shopRoom gameobjects
        shopRooms = GameObject.FindGameObjectsWithTag("ShopRoom");
        Debug.Log("ShopRooms: " + shopRooms.Length + " found");

        assignList();
        int[,] map = PickMap();
        assignRoom(map);
        assignPortal(map);
    }

    private void assignList(){
        foreach (GameObject room in combatRooms){
            combatRoomList.Add(room);
        }
        foreach (GameObject room in puzzleRooms){
            puzzleRoomList.Add(room);
        }
        foreach (GameObject room in shopRooms){
            shopRoomList.Add(room);
        }
    }

    private int[,] PickMap(){
        chosenMap = Random.Range(1, 4);
        switch (chosenMap)
        {
            case 1:
                return Map1;
            case 2:
                return Map2;
            case 3:
                return Map3;
            default:
                return Map1;
        }
    }

    //Pick a room according to the map
    private GameObject PickRoom(int roomType){
        switch (roomType)
        {
            case 1:
                return startRoom;
            case 2:
                return tutorialRoom;
            case 3:
                int shopRandom = Random.Range(0, shopRoomList.Count);
                var room3 = shopRoomList[shopRandom];
                shopRoomList.RemoveAt(shopRandom);
                return room3;
            case 4:
                int combatRandom = Random.Range(0, combatRoomList.Count);
                var room4 = combatRoomList[combatRandom];
                combatRoomList.RemoveAt(combatRandom);
                return room4;
            case 5:
                int puzzleRandom = Random.Range(0, puzzleRoomList.Count);
                var room5 = puzzleRoomList[puzzleRandom];
                puzzleRoomList.RemoveAt(puzzleRandom);
                return room5;
            case 6:
                return endRoom;
            default:
                return null;
        }
    }

    private void assignRoom(int[,] map){
        //assigns the room to the current position in the map
        for (int i = 0; i < map.GetLength(0); i++){
            for (int j = 0; j < map.GetLength(1); j ++){
                int currentRoomIndex = map[i,j];
                if (currentRoomIndex != 0){
                    Vector2Int currentRoom = new Vector2Int(i, j);
                    GameObject room = PickRoom(currentRoomIndex);
                    roomDictionary.Add(currentRoom, room);
                    Debug.Log("Room " + currentRoom + " assigned");
                }
            }
        }
    }

    //iterates through the map, assigning currentRoom to the room at the current position
    //and prevRoom to the room at the previous position
    //also assigns and checks if there are rooms above, below, left and right of the current room
    //if any of these rooms are the previous room, it will not make a portal to that room
    private void assignPortal(int[,] map){

        for (int i = 0; i < map.GetLength(0); i++){
            for (int j = 0; j < map.GetLength(1); j ++){
                int currentRoomIndex = map[i,j];
                if(currentRoomIndex != 0){

                    Vector2Int currentRoomKey = new Vector2Int(i, j);
                    GameObject room = roomDictionary[currentRoomKey];

                    if (i + 1 < map.GetLength(0) && map[i + 1, j] != 0)
                    {
                        Vector2Int adjacentRoomKey = new Vector2Int(i + 1, j);
                        if(roomDictionary.ContainsKey(adjacentRoomKey)){
                            GameObject adjacentRoom = roomDictionary[adjacentRoomKey];
                            //Link Teleporter A1 to Teleporter A2
                            GameObject PortalA1 = room.transform.GetChild(2).gameObject;
                            GameObject PortalA2 = adjacentRoom.transform.GetChild(1).gameObject;
                            PortalA1.GetComponent<LinkTeleporter>().targetRoom = PortalA2;
                            PortalA2.GetComponent<LinkTeleporter>().targetRoom = PortalA1;
                            Debug.Log("Portal A1: " + PortalA1.name + " Portal A2: " + PortalA2.name);
                        }
                    }

                    if (j + 1 < map.GetLength(1) && map[i, j + 1] != 0)
                    {
                        Vector2Int adjacentRoomKey = new Vector2Int(i, j + 1);
                        if(roomDictionary.ContainsKey(adjacentRoomKey)){
                            GameObject adjacentRoom = roomDictionary[adjacentRoomKey];
                            //Link Teleporter B1 to Teleporter B2
                            GameObject PortalB1 = room.transform.GetChild(3).gameObject;
                            GameObject PortalB2 = adjacentRoom.transform.GetChild(4).gameObject;
                            PortalB1.GetComponent<LinkTeleporter>().targetRoom = PortalB2;
                            PortalB2.GetComponent<LinkTeleporter>().targetRoom = PortalB1;
                            Debug.Log("Portal B1: " + PortalB1.name + " Portal B2: " + PortalB2.name);
                        }
                    }
                }
            }
        }
    }
}

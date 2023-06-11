using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //Variables
    //[SerializeField] private int numberOfRooms = 16;

    [SerializeField] private GameObject miniRoom;
    [SerializeField] private GameObject miniMap;
    [SerializeField] private Sprite endRoomSprite;

    private int chosenMap;

    private GameObject[] combatRooms, puzzleRooms, shopRooms;
    private List<GameObject> combatRoomList = new List<GameObject>();
    private List<GameObject> puzzleRoomList = new List<GameObject>();
    private List<GameObject> shopRoomList = new List<GameObject>();
    private GameObject startRoom, tutorialRoom, endRoom, currentRoom;
    private Vector2Int currentRoomCoordinates;
    private Dictionary<Vector2Int, GameObject> roomDictionary = new Dictionary<Vector2Int, GameObject>();

    private int[,] map;

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
    private int[,] map1 = new int[6,6] { 
    {1,2,0,4,0,0}, 
    {0,4,5,4,4,3},
    {0,0,4,0,0,0},
    {0,3,5,4,0,0},
    {0,0,0,4,5,4},
    {0,0,0,0,0,6}
    };

    private int[,] mapAmogus = new int[9,9] { 
    //amongus shaped map
    {0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0},
    {0,0,4,2,1,0,0,0,0}, 
    {0,3,5,0,0,6,0,0,0},
    {0,4,4,0,4,3,0,0,0},
    {0,4,5,4,4,0,0,0,0},
    {0,0,4,0,5,0,0,0,0},
    {0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0}
    };

    // Start is called before the first frame update
    void Start()
    {
        //will switch to FindGameObjectsWithTag later
        //look for startRoom gameobject
        startRoom = GameObject.FindWithTag("StartRoom");
        //look for tutorialRoom gameobject
        tutorialRoom = GameObject.FindWithTag("TutorialRoom");
        //look for endRoom gameobject
        endRoom = GameObject.FindWithTag("EndRoom");
        //look for combatRoom gameobjects
        combatRooms = GameObject.FindGameObjectsWithTag("CombatRoom");
        Debug.Log("Combat rooms: " + combatRooms.Length + " found");
        //look for puzzleRoom gameobjects
        puzzleRooms = GameObject.FindGameObjectsWithTag("PuzzleRoom");
        Debug.Log("PuzzleRooms: " + puzzleRooms.Length + " found");
        //look for shopRoom gameobjects
        shopRooms = GameObject.FindGameObjectsWithTag("ShopRoom");
        Debug.Log("ShopRooms: " + shopRooms.Length + " found");

        AssignList();
        //map = PickMap();

        int RandomMap = Random.Range(0, 10);

        // 80% chance of drunkard walk map, 20% chance of amongus map
        if ((RandomMap + 1) % 5 == 0){
            map = mapAmogus;
        }
        else{
            map = GetComponent<DrunkardWalk>().GenerateMatrix();
        }
        
        AssignRoom(map);
        AssignPortal(map);

        DrawMiniMap();
    }

    private void AssignList(){
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

    private void AssignRoom(int[,] map){
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
    private void AssignPortal(int[,] map){

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
                            PortalA1.GetComponent<LinkTeleporter>().TargetRoom = PortalA2;
                            PortalA2.GetComponent<LinkTeleporter>().TargetRoom = PortalA1;
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
                            PortalB1.GetComponent<LinkTeleporter>().TargetRoom = PortalB2;
                            PortalB2.GetComponent<LinkTeleporter>().TargetRoom = PortalB1;
                            Debug.Log("Portal B1: " + PortalB1.name + " Portal B2: " + PortalB2.name);
                        }
                    }
                }
            }
        }
    }

    //Draw mini map based on the map matrix
    // this creates a minimap gameobject and initializes miniRoom for each room as a child of minimap
    private void DrawMiniMap(){
        //create squares for each room
        for (int i = 0; i < map.GetLength(0); i++){
            for (int j = 0; j < map.GetLength(1); j ++){
                //instatiates a miniRoom prefab per room the same position as the room in the map
                GameObject miniRoomPrefab = Instantiate(miniRoom, miniMap.transform);
                //sets the position of the miniRoom according to the position in the map, relative to the minimap
                miniRoomPrefab.transform.localPosition = new Vector3(j, -i, 0);
                //make all of them in layer 3
                //if matrix value is 6, change sprite to end room sprite
                if (map[i,j] == 6){
                    miniRoomPrefab.GetComponent<SpriteRenderer>().sprite = endRoomSprite;
                }
                miniRoomPrefab.GetComponent<SpriteRenderer>().sortingOrder = 3;
                //set all rooms to transparent
                miniRoomPrefab.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
        }

        //set start room = current room to yellow by getting its keyvalue
        currentRoomCoordinates = getKeyFromValue(roomDictionary, startRoom);
        GameObject startMiniRoom = miniMap.transform.GetChild(currentRoomCoordinates.x * map.GetLength(1) + currentRoomCoordinates.y).gameObject;
        startMiniRoom.GetComponent<SpriteRenderer>().color = Color.white;

        //update adjacent rooms
        UpdateAdjacent(currentRoomCoordinates);
    }
    //Update minimap based on current room
    public void UpdateMiniMap(GameObject currentRoom){
        //set previous room to white
        GameObject prevRoom = miniMap.transform.GetChild(currentRoomCoordinates.x * map.GetLength(1) + currentRoomCoordinates.y).gameObject;
        prevRoom.GetComponent<SpriteRenderer>().color = Color.grey;
        
        //get current room position in map
        currentRoomCoordinates = getKeyFromValue(roomDictionary, currentRoom);
        //get current room position in minimap
        GameObject miniRoom = miniMap.transform.GetChild(currentRoomCoordinates.x * map.GetLength(1) + currentRoomCoordinates.y).gameObject;
        //set current room to white
        miniRoom.GetComponent<SpriteRenderer>().color = Color.white;

        //update adjacent rooms
        UpdateAdjacent(currentRoomCoordinates);
    }

    // Check Adjacent Rooms
    private void UpdateAdjacent(Vector2Int currentRoomCoordinates){
        //set adjacent rooms up, down, left, and right that are transparent to orange
        if (currentRoomCoordinates.x + 1 < map.GetLength(0)){
            Vector2Int adjacentRoomCoordinates = new Vector2Int(currentRoomCoordinates.x + 1, currentRoomCoordinates.y);
            GameObject adjacentMiniRoom = miniMap.transform.GetChild(adjacentRoomCoordinates.x * map.GetLength(1) + adjacentRoomCoordinates.y).gameObject;
            //check if the color is transparent
            if (adjacentMiniRoom.GetComponent<SpriteRenderer>().color == new Color(0, 0, 0, 0) && map[adjacentRoomCoordinates.x, adjacentRoomCoordinates.y] != 0){
                adjacentMiniRoom.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
        if (currentRoomCoordinates.x - 1 >= 0){
            Vector2Int adjacentRoomCoordinates = new Vector2Int(currentRoomCoordinates.x - 1, currentRoomCoordinates.y);
            GameObject adjacentMiniRoom = miniMap.transform.GetChild(adjacentRoomCoordinates.x * map.GetLength(1) + adjacentRoomCoordinates.y).gameObject;
            if (adjacentMiniRoom.GetComponent<SpriteRenderer>().color == new Color(0, 0, 0, 0) && map[adjacentRoomCoordinates.x, adjacentRoomCoordinates.y] != 0){
                adjacentMiniRoom.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
        if (currentRoomCoordinates.y + 1 < map.GetLength(1)){
            Vector2Int adjacentRoomCoordinates = new Vector2Int(currentRoomCoordinates.x, currentRoomCoordinates.y + 1);
            GameObject adjacentMiniRoom = miniMap.transform.GetChild(adjacentRoomCoordinates.x * map.GetLength(1) + adjacentRoomCoordinates.y).gameObject;
            if (adjacentMiniRoom.GetComponent<SpriteRenderer>().color == new Color(0, 0, 0, 0) && map[adjacentRoomCoordinates.x, adjacentRoomCoordinates.y] != 0){
                adjacentMiniRoom.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
        if (currentRoomCoordinates.y - 1 >= 0){
            Vector2Int adjacentRoomCoordinates = new Vector2Int(currentRoomCoordinates.x, currentRoomCoordinates.y - 1);
            GameObject adjacentMiniRoom = miniMap.transform.GetChild(adjacentRoomCoordinates.x * map.GetLength(1) + adjacentRoomCoordinates.y).gameObject;
            if (adjacentMiniRoom.GetComponent<SpriteRenderer>().color == new Color(0, 0, 0, 0) && map[adjacentRoomCoordinates.x, adjacentRoomCoordinates.y] != 0){
                adjacentMiniRoom.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    //get key from value in dictionary
    private Vector2Int getKeyFromValue(Dictionary<Vector2Int, GameObject> dict, GameObject val){
        foreach (KeyValuePair<Vector2Int, GameObject> entry in dict){
            if (entry.Value == val){
                return entry.Key;
            }
        }
        return new Vector2Int(-1, -1);
    }

    public void destroyMiniMap(){
        //destroy minimap child objects
        foreach (Transform child in miniMap.transform){
            Destroy(child.gameObject);
        }
    }

    //Reset all global variables
    void ResetAll(){
        combatRoomList.Clear();
        puzzleRoomList.Clear();
        shopRoomList.Clear();
        roomDictionary.Clear();
        currentRoomCoordinates = new Vector2Int(-1, -1);

        //destroy minimap child objects
        destroyMiniMap();
        
        //Instiate all again
        Start();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrunkardWalk : MonoBehaviour
{

    private static int maxCombatRooms = 8;
    private static int maxPuzzleRooms = 3;
    private static int maxShopRooms = 2;
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private int matrixLength = 6;
    private int numberOfRooms = 16;
    private int maxSteps = 100;

    private int[,] matrix;
    private Vector2Int currentRoom;
    private Vector2Int previousRoom;
    private Vector2Int nextRoom;
    private Vector2Int[] rooms;
    private int roomIndex = 0;
    private int stepIndex = 0;
    private int shopRoomCount, puzzleRoomCount, combatRoomCount;
    

    private enum RoomType
    {
        
        None, // is 0 in matrix, empty room
        StartRoom, // prints 1 in matrix, only one
        TutorialRoom, // prints 2 in matrix, only one, always adjacent to StartRoom
        CombatRoom, // prints 3 in matrix, total of 8
        PuzzleRoom, // prints 4 in matrix, total of 3, cannot be adjacent to another PuzzleRoom
        ShopRoom, // prints 5 in matrix, total of 2, cannot be adjacent to another ShopRoom
        EndRoom, // prints 6 in matrix, only one, last room
    }

    private List<RoomType> roomProbabilities = new List<RoomType>();

    void Start()
    {
        GenerateMatrix();
        PrintMatrix();
    }

    //keep track of current room, previous room, and next room
    public int[,] GenerateMatrix(){
        ResetValues();
        // Set StartRoom Start at first row, random column
        currentRoom = new Vector2Int(0, Random.Range(0, matrixLength));
        previousRoom = currentRoom;
        matrix[currentRoom.x, currentRoom.y] = (int)RoomType.StartRoom;
        rooms[roomIndex] = currentRoom;
        roomIndex++;
        // Set TutorialRoom, can only be adjacent at the left or right or down of StartRoom, also depends on the column of StartRoom (if it's at the edge)
        //if currentRoom.y == 0, TutorialRoom can only be at the right or down of StartRoom
        GenerateTrainingRoom();

        // Walk through the matrix, randomly choosing a direction to go to
        // keep walking until the number of rooms is reached
        // do not adda a room if it is out of bounds or if it is already a room
        while(roomIndex <= numberOfRooms){
            //if the number of steps is greater than the max steps, regenerate the matrix

            if (stepIndex > maxSteps){
                ResetValues();
                GenerateMatrix();
                break;
            }

            //choose a random direction
            int randomDirection = Random.Range(0, 4);
            //set nextRoom to the random direction
            nextRoom = currentRoom + directions[randomDirection];

            stepIndex++;

            //if nextRoom is adjacent to startRoom, choose another direction
            if (nextRoom == rooms[0] + Vector2Int.up || nextRoom == rooms[0] + Vector2Int.down || nextRoom == rooms[0] + Vector2Int.left || nextRoom == rooms[0] + Vector2Int.right){
                continue;
            }
            //if nextRoom is out of bounds, choose another direction
            if (nextRoom.x < 0 || nextRoom.x >= matrixLength || nextRoom.y < 0 || nextRoom.y >= matrixLength){
                continue;
            }
            //if nextRoom is already a room, choose another direction
            if (matrix[nextRoom.x, nextRoom.y] != (int)RoomType.None){
                continue;
            }

            if (roomIndex == numberOfRooms - 1){
                //if there are no more rooms to add, add the end room and break out of the loop
                matrix[nextRoom.x, nextRoom.y] = (int)RoomType.EndRoom;
                roomIndex++;
                break;
            }
            
            if(roomProbabilities.Count != 0){
                // 8 combat rooms, 3 puzzle rooms, 2 shop rooms = 13 rooms
                int randomRoom = Random.Range(0, 13);
                if (randomRoom < 8){
                    if (roomProbabilities.Contains(RoomType.CombatRoom)){
                        matrix[nextRoom.x, nextRoom.y] = (int)RoomType.CombatRoom;
                        combatRoomCount++;
                        if (combatRoomCount == maxCombatRooms){
                            roomProbabilities.Remove(RoomType.CombatRoom);
                        }
                        roomIndex++;
                    }
                    else{
                        continue;
                    }
                }
                else if (randomRoom < 11){
                    if(roomProbabilities.Contains(RoomType.PuzzleRoom)){
                        //if mext room's adjacent room is a puzzleRoom, choose another room
                        if(CheckAdjacency(nextRoom, RoomType.PuzzleRoom)){
                            continue;
                        }
                        matrix[nextRoom.x, nextRoom.y] = (int)RoomType.PuzzleRoom;
                        puzzleRoomCount++;
                        if (puzzleRoomCount == maxPuzzleRooms){
                            roomProbabilities.Remove(RoomType.PuzzleRoom);
                        }
                        roomIndex++;
                    }
                    else{
                        continue;
                    }
                }
                else if (randomRoom < 13){
                    if(roomProbabilities.Contains(RoomType.ShopRoom)){
                        //if next room's adjacent room is a shopRoom, choose another room
                        if(CheckAdjacency(nextRoom, RoomType.ShopRoom)){
                            continue;
                        }
                        matrix[nextRoom.x, nextRoom.y] = (int)RoomType.ShopRoom;
                        shopRoomCount++;
                        if (shopRoomCount == maxShopRooms){
                            roomProbabilities.Remove(RoomType.ShopRoom);
                        }
                        roomIndex++;
                    }
                    else{
                        continue;
                    }
                }
            } 

            rooms[roomIndex] = nextRoom;
            previousRoom = currentRoom;
            currentRoom = nextRoom;
        }

        // check if the end room is adjacent to the start room or tutorial room, if it is regenerate the matrix
        if (rooms[roomIndex - 1] == rooms[1] || rooms[roomIndex - 1] == rooms[2]){
            Debug.Log("End room is adjacent to start room or tutorial room");
            ResetValues();
            GenerateMatrix();
        }
        
        return matrix;
    }

    //checks all adjacent rooms to see if they are the same type
    //should be kept in matrix bounds
    private bool CheckAdjacency(Vector2Int roomCheck, RoomType roomType){
        //TODO: 
        return false;
    }

    private void GenerateTrainingRoom(){
        //if currentRoom.y == matrixLength - 1, TutorialRoom can only be at the left or down of StartRoom
        if (currentRoom.y == 0){
            //choose between right or down
            int randomDirection = Random.Range(0, 2);
            if (randomDirection == 0){
                matrix[currentRoom.x, currentRoom.y + 1] = (int)RoomType.TutorialRoom;
                rooms[roomIndex] = new Vector2Int(currentRoom.x, currentRoom.y + 1);
            } else {
                matrix[currentRoom.x + 1, currentRoom.y] = (int)RoomType.TutorialRoom;
                rooms[roomIndex] = new Vector2Int(currentRoom.x + 1, currentRoom.y);
            }
        } 
        //if currentRoom.y == matrixLength - 1, TutorialRoom can only be at the left or down of StartRoom
        else if (currentRoom.y == matrixLength - 1){
            //choose between left or down
            int randomDirection = Random.Range(0, 2);
            if (randomDirection == 0){
                matrix[currentRoom.x, currentRoom.y - 1] = (int)RoomType.TutorialRoom;
                rooms[roomIndex] = new Vector2Int(currentRoom.x, currentRoom.y - 1);
            } else {
                matrix[currentRoom.x + 1, currentRoom.y] = (int)RoomType.TutorialRoom;
                rooms[roomIndex] = new Vector2Int(currentRoom.x + 1, currentRoom.y);
            }
        } 
        //otherwise can be either left, right, or down
        else {
            //choose between left, right, or down
            int randomDirection = Random.Range(0, 3);
            if (randomDirection == 0){
                matrix[currentRoom.x, currentRoom.y - 1] = (int)RoomType.TutorialRoom;
                rooms[roomIndex] = new Vector2Int(currentRoom.x, currentRoom.y - 1);
            } else if (randomDirection == 1){
                matrix[currentRoom.x, currentRoom.y + 1] = (int)RoomType.TutorialRoom;
                rooms[roomIndex] = new Vector2Int(currentRoom.x, currentRoom.y + 1);
            } else {
                matrix[currentRoom.x + 1, currentRoom.y] = (int)RoomType.TutorialRoom;
                rooms[roomIndex] = new Vector2Int(currentRoom.x + 1, currentRoom.y);
            }
        }
        roomIndex++;
        //set currentRoom to the TutorialRoom
        previousRoom = currentRoom;
        currentRoom = rooms[roomIndex - 1];
    }

    private void ResetValues() {
        //reset all values
        rooms = new Vector2Int[numberOfRooms];
        matrix = new int[matrixLength, matrixLength];
        roomIndex = 0;
        stepIndex = 0;
        combatRoomCount = 0;
        puzzleRoomCount = 0;
        shopRoomCount = 0;
        roomProbabilities = new List<RoomType>
        {
            RoomType.CombatRoom,
            RoomType.PuzzleRoom,
            RoomType.ShopRoom
        };
    }

    private void PrintMatrix(){
        string matrixString = "";
        for (int i = 0; i < matrixLength; i++){
            for (int j = 0; j < matrixLength; j++){
                matrixString += matrix[j, i] + " ";
            }
            matrixString += "\n";
        }
        Debug.Log(matrixString);
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrunkardWalk : MonoBehaviour
{

    private static int maxCombatRooms = 8;
    private static int maxPuzzleRooms = 3;
    private static int maxShopRooms = 2;
    
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private int matrixLength = 9;
    private int numberOfRooms = 16;
    private int maxSteps = 100;

    private int[,] matrix;
    private Vector2Int currentRoom;
    private Vector2Int previousRoom;
    private Vector2Int nextRoom;
    private Vector2Int[] rooms;
    private int roomIndex = 0;
    private int stepIndex = 0;
    private int shopRoomCount, puzzleRoomCount, combatRoomCount, endRoomCount;
    

    private enum RoomType
    {
        
        None, // is 0 in matrix, empty room
        StartRoom, // prints 1 in matrix, only one
        TutorialRoom, // prints 2 in matrix, only one, always adjacent to StartRoom
        ShopRoom, // prints 3 in matrix, total of 2, cannot be adjacent to another ShopRoom
        CombatRoom, // prints 4 in matrix, total of 8
        PuzzleRoom, // prints 5 in matrix, total of 3, cannot be adjacent to another PuzzleRoom
        EndRoom, // prints 6 in matrix, only one, last room
        TempRoom // placeholder for room that is being generated
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
        while(roomIndex <= numberOfRooms - 1){
            //if the number of steps is greater than the max steps, regenerate the matrix

            //choose a random direction
            int randomDirection = Random.Range(0, 4);
            //set nextRoom to the random direction
            nextRoom = currentRoom + directions[randomDirection];

            stepIndex++;

            if(roomIndex ==  numberOfRooms - 1)
            {
                break;
            }

            if (stepIndex > maxSteps){
                ResetValues();
                GenerateMatrix();
                break;
            }

            //if nextRoom is adjacent to startRoom, choose another direction
            if (nextRoom == rooms[0] + Vector2Int.up || nextRoom == rooms[0] + Vector2Int.down || nextRoom == rooms[0] + Vector2Int.left || nextRoom == rooms[0] + Vector2Int.right){
                continue;
            }
            //if nextRoom is out of bounds, choose another direction
            if (nextRoom.x < 0 || nextRoom.x >= matrixLength || nextRoom.y < 0 || nextRoom.y >= matrixLength){
                continue;
            }
            
            // if nextRoom is already a room, choose another direction or go back to previous room
            if (matrix[nextRoom.x, nextRoom.y] != (int)RoomType.None)
            {
                var choose = Random.Range(0, 2);

                switch (choose)
                {
                    case 0:
                        continue;
                    case 1:
                        nextRoom = previousRoom;
                        continue;
                    case 2:
                        nextRoom = currentRoom;
                        continue;
                }   
            }

            //assign the nextRoom as a placeholder room
            matrix[nextRoom.x, nextRoom.y] = (int)RoomType.TempRoom;
            roomIndex++;

            //set the currentRoom to the nextRoom
            rooms[roomIndex] = nextRoom;
            previousRoom = currentRoom;
            currentRoom = nextRoom;
        }

        //generate the shop layer
        GenerateShopLayer();
        //generate the puzzle layer
        GeneratePuzzleLayer();
        //generate the combat layer
        GenerateCombatLayer();
        //set the end room
        GenerateEndRoom();

        // // check if the end room is adjacent to the start room or tutorial room, if it is regenerate the matrix
        // if (rooms[roomIndex - 1] == rooms[1] || rooms[roomIndex - 1] == rooms[2]){
        //     Debug.Log("End room is adjacent to start room or tutorial room");
        //     ResetValues();
        //     GenerateMatrix();
        // }

        PrintMatrix();

        return matrix;
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

    private void GenerateShopLayer(){
        //set random shop rooms, try to set them as far away from each other as possible
        int shopRoomCount = 0;

        List<Vector2Int> roomsShop = new List<Vector2Int>();

        foreach (Vector2Int room in rooms){
            if (matrix[room.x, room.y] == (int)RoomType.TempRoom){
                roomsShop.Add(room);
            }
        }

        while (shopRoomCount < maxShopRooms && roomsShop.Count > 0){

            int randomIndex = Random.Range(0, roomsShop.Count);
            Vector2Int randomRoom = roomsShop[randomIndex];

            //check if randomRoom is adjacent to shopRoom, if it is remove it from the possible rooms
            if(CheckDuplicateAdjacency(randomRoom, RoomType.ShopRoom)){
                roomsShop.Remove(randomRoom);
                continue;
            }
            
            matrix[randomRoom.x, randomRoom.y] = (int)RoomType.ShopRoom;
            roomsShop.Remove(randomRoom);
            shopRoomCount++;
        }
    }

    private void GeneratePuzzleLayer(){
        //set random puzzle rooms, try to set them as far away from each other as possible
        int puzzleRoomCount = 0;

        List<Vector2Int> roomsPuzzle = new List<Vector2Int>();

        foreach (Vector2Int room in rooms){
            if (matrix[room.x, room.y] == (int)RoomType.TempRoom){
                roomsPuzzle.Add(room);
            }
        }

        while (puzzleRoomCount < maxPuzzleRooms && roomsPuzzle.Count > 0){
            int randomIndex = Random.Range(0, roomsPuzzle.Count);
            Vector2Int randomRoom = roomsPuzzle[randomIndex];

            //check if adjacent to PuzzleRoom
            if(CheckDuplicateAdjacency(randomRoom, RoomType.PuzzleRoom)){
                roomsPuzzle.Remove(randomRoom);
                continue;
            }

            matrix[randomRoom.x, randomRoom.y] = (int)RoomType.PuzzleRoom;
            roomsPuzzle.Remove(randomRoom);
            puzzleRoomCount++;
        }
    }

    private void GenerateCombatLayer(){
        //fill the rest of the rooms with combat rooms
        for (int i = 0; i < matrixLength; i++){
            for (int j = 0; j < matrixLength; j++){
                if (matrix[i, j] == (int)RoomType.TempRoom){
                    matrix[i, j] = (int)RoomType.CombatRoom;
                }
            }
        }
    }

    private void GenerateEndRoom(){
        //furthest room from start room
        //should occupy a temp room
        //should be adjacent to any other room
        int maxDistance = 0;
        Vector2Int endRoom = new Vector2Int(0, 0);

        if (endRoomCount == 0){
            for (int i = 0; i < matrixLength; i++){
                for (int j = 0; j < matrixLength; j++){
                    if (matrix[i, j] == (int)RoomType.None){
                        int distance = Mathf.Abs(rooms[0].x - i) + Mathf.Abs(rooms[0].y - j);
                        int checkNeighbors = CheckNeighbors(new Vector2Int(i, j));
                        if(checkNeighbors > 0 && checkNeighbors < 3){
                            if (distance > maxDistance){
                                maxDistance = distance;
                                endRoom = new Vector2Int(i, j);
                            }
                        }
                    }
                }
            }
            
            matrix[endRoom.x, endRoom.y] = (int)RoomType.EndRoom;
            endRoomCount++;
        }
    }

    private int CheckNeighbors(Vector2Int roomCheck){
        int count = 0;
        if (roomCheck.x - 1 >= 0){
            if (matrix[roomCheck.x - 1, roomCheck.y] != (int)RoomType.None){
                count++;
            }
        }
        if (roomCheck.x + 1 < matrixLength){
            if (matrix[roomCheck.x + 1, roomCheck.y] != (int)RoomType.None){
                count++;
            }
        }
        if (roomCheck.y - 1 >= 0){
            if (matrix[roomCheck.x, roomCheck.y - 1] != (int)RoomType.None){
                count++;
            }
        }
        if (roomCheck.y + 1 < matrixLength){
            if (matrix[roomCheck.x, roomCheck.y + 1] != (int)RoomType.None){
                count++;
            }
        }
        return count;
    }

    //checks all adjacent rooms to see if they are the same type
    //should be kept in matrix bounds
    private bool CheckDuplicateAdjacency(Vector2Int roomCheck, RoomType roomType){
        if (roomCheck.x - 1 >= 0){
            if (matrix[roomCheck.x - 1, roomCheck.y] == (int)roomType){
                return true;
            }
        }
        if (roomCheck.x + 1 < matrixLength){
            if (matrix[roomCheck.x + 1, roomCheck.y] == (int)roomType){
                return true;
            }
        }
        if (roomCheck.y - 1 >= 0){
            if (matrix[roomCheck.x, roomCheck.y - 1] == (int)roomType){
                return true;
            }
        }
        if (roomCheck.y + 1 < matrixLength){
            if (matrix[roomCheck.x, roomCheck.y + 1] == (int)roomType){
                return true;
            }
        }
        return false;
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
        endRoomCount = 0;
    }

    private void PrintMatrix(){
        string matrixString = "";
        int RoomCount = 0;
        for (int i = 0; i < matrixLength; i++){
            for (int j = 0; j < matrixLength; j++){
                matrixString += matrix[i, j] + " ";
                if (matrix[i, j] != (int)RoomType.None){
                    RoomCount++;
                }
            }
            matrixString += "\n";
        }
        Debug.Log(matrixString);
        Debug.Log("Room Count: " + RoomCount);
    }
}
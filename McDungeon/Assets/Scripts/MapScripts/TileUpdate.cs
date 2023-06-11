using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Tile tileReplacement;
    [SerializeField] Tile leftGateTile;
    [SerializeField] Tile rightGateTile;
    [SerializeField] Tile middleGateTile;
    private Tilemap replaceTileMap;
    private Vector3Int currentCell, adjacent1, adjacent2;
    private bool beenUpdated = false;
    private List<GameObject> teleporterList = new List<GameObject>();
    private PuzzleController puzzleController;
    private GameObject grandParent;
    void Start()
    {
        replaceTileMap = GetComponent<Tilemap>();
        GameObject parent = transform.parent.gameObject;
        grandParent = parent.transform.parent.gameObject;

        if((grandParent.CompareTag("TutorialRoom") || grandParent.CompareTag("PuzzleRoom"))){
            puzzleController = grandParent.transform.GetChild(5).GetComponent<PuzzleController>();
        }

        //get all teleporter tags under the same parent
        foreach (Transform child in parent.transform.parent){
            if (child.gameObject.tag == "Teleporter"){
                teleporterList.Add(child.gameObject);
            }
        }

    }

    //
    private void Update() {

        // TODO: Make this stop running after the room is completed.
        //check if teleporter game object and tile are at the same position
        if (beenUpdated == false){
            foreach (GameObject teleporter in teleporterList){
                // Get current tile teleporter is on.
                currentCell = replaceTileMap.WorldToCell(teleporter.transform.position);

                // Get adjacent tiles based on rotation of teleporter.
                if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 0)){
                        adjacent1 = new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z);
                        adjacent2 = new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z);
                }
                else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 90)){
                    adjacent1 = new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z);
                    adjacent2 = new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z);
                }
                else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 180)){
                    adjacent1 = new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z);
                    adjacent2 = new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z);
                }
                else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, -90)){
                    adjacent1 = new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z);
                    adjacent2 = new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z);
                }

                if (teleporter.GetComponent<LinkTeleporter>().TargetRoom == null){
                    replaceTileMap.SetTile(adjacent1, tileReplacement);
                    replaceTileMap.SetTile(adjacent2, tileReplacement);
                    replaceTileMap.SetTile(currentCell, tileReplacement);
                }
                else if((grandParent.CompareTag("TutorialRoom") || grandParent.CompareTag("PuzzleRoom"))){
                    if (puzzleController.GetPuzzleRoomState() == PuzzleRoomState.InProgress){
                        //TODO:
                        // Debug.Log("Puzzle room in progress");
                        replaceTileMap.SetTile(adjacent1, tileReplacement);
                        replaceTileMap.SetTile(adjacent2, tileReplacement);
                        replaceTileMap.SetTile(currentCell, tileReplacement);
                    }
                    else if (puzzleController.GetPuzzleRoomState() == PuzzleRoomState.Completed){
                        replaceTileMap.SetTile(adjacent1, leftGateTile);
                        replaceTileMap.SetTile(adjacent2, rightGateTile);
                        replaceTileMap.SetTile(currentCell, middleGateTile);
                    }
                }

                TileRotation(teleporter);
            }
        }
    }

    //Tile rotation 
    void TileRotation (GameObject teleporter){
        //rotate tiles according to rotation of teleporter
        if (teleporter.transform.localRotation == Quaternion.Euler(0,0,90)){
            replaceTileMap.SetTransformMatrix(currentCell, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,90), Vector3.one));
            replaceTileMap.SetTransformMatrix(adjacent1, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,90), Vector3.one));
            replaceTileMap.SetTransformMatrix(adjacent2, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,90), Vector3.one));
        }
        else if(teleporter.transform.localRotation == Quaternion.Euler(0,0,180)){
            replaceTileMap.SetTransformMatrix(currentCell, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,180), Vector3.one));
            replaceTileMap.SetTransformMatrix(adjacent1, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,180), Vector3.one));
            replaceTileMap.SetTransformMatrix(adjacent2, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,180), Vector3.one));
        }
        else if(teleporter.transform.localRotation == Quaternion.Euler(0,0,-90)){
            replaceTileMap.SetTransformMatrix(currentCell, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,270), Vector3.one));
            replaceTileMap.SetTransformMatrix(adjacent1, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,270), Vector3.one));
            replaceTileMap.SetTransformMatrix(adjacent2, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,270), Vector3.one));
        }
    }

    // Reset tiles to original state
    public void ResetAll(){
        foreach (GameObject teleporter in teleporterList){
            // Get current tile teleporter is on.
            currentCell = replaceTileMap.WorldToCell(teleporter.transform.position);

            // Get adjacent tiles based on rotation of teleporter.
            if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 0)){
                    adjacent1 = new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z);
                    adjacent2 = new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z);
            }
            else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 90)){
                adjacent1 = new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z);
                adjacent2 = new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z);
            }
            else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 180)){
                adjacent1 = new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z);
                adjacent2 = new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z);
            }
            else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, -90)){
                adjacent1 = new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z);
                adjacent2 = new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z);
            }

            replaceTileMap.SetTile(adjacent1, leftGateTile);
            replaceTileMap.SetTile(adjacent2, rightGateTile);
            replaceTileMap.SetTile(currentCell, middleGateTile);

            TileRotation(teleporter);
        }
    }
}

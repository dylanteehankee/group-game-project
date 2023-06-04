using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Tile tileReplacement;
    private Tilemap replaceTileMap;
    private Vector3Int currentCell, adjacent1, adjacent2;
    private bool beenUpdated = false;
    private int counter = 0;
    private List<GameObject> teleporterList = new List<GameObject>();
    void Start()
    {
        replaceTileMap = GetComponent<Tilemap>();
        GameObject parent = transform.parent.gameObject;
        //get all teleporter tags under the same parent
        foreach (Transform child in parent.transform.parent){
            if (child.gameObject.tag == "Teleporter"){
                teleporterList.Add(child.gameObject);
            }
        }

    }

    //
    private void Update() {

        //check if teleporter game object and tile are at the same position
        if(beenUpdated == false){
            foreach (GameObject teleporter in teleporterList){
                // get current tile teleporter is on
                if(teleporter.GetComponent<LinkTeleporter>().TargetRoom == null){
                    currentCell = replaceTileMap.WorldToCell(teleporter.transform.position);
                    // if rotation of other object is 0, adjacent1 is left and adjacent2 is right,
                    // else if rotation is 90, adjacent1 is up and adjacent2 is down
                    // else if rotation is 180, adjacent1 is right and adjacent2 is left
                    // else if rotation is 270, adjacent1 is down and adjacent2 is up
                    if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 0)){
                        adjacent1 = new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z);
                        adjacent2 = new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z);
                    }
                    else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 90)){
                        adjacent1 = new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z);
                        adjacent2 = new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z);
                    }
                    else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, 180)){
                        adjacent1 = new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z);
                        adjacent2 = new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z);
                    }
                    else if (teleporter.transform.localRotation == Quaternion.Euler(0, 0, -90)){
                        adjacent1 = new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z);
                        adjacent2 = new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z);
                    }

                    replaceTileMap.SetTile(adjacent1, tileReplacement);
                    replaceTileMap.SetTile(adjacent2, tileReplacement);
                    replaceTileMap.SetTile(currentCell, tileReplacement);

                    if(teleporter.transform.localRotation == Quaternion.Euler(0,0,90)){
                    replaceTileMap.SetTransformMatrix(currentCell, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,90), Vector3.one));
                    replaceTileMap.SetTransformMatrix(adjacent1, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,90), Vector3.one));
                    replaceTileMap.SetTransformMatrix(adjacent2, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,90), Vector3.one));
                    }
                    else if(teleporter.transform.localRotation == Quaternion.Euler(0,0,180)){
                        replaceTileMap.SetTransformMatrix(currentCell, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,180), Vector3.one));
                        replaceTileMap.SetTransformMatrix(adjacent1, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,180), Vector3.one));
                        replaceTileMap.SetTransformMatrix(adjacent2, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,180), Vector3.one));
                    }
                    else if(teleporter.transform.localRotation == Quaternion.Euler(0,0,270)){
                        replaceTileMap.SetTransformMatrix(currentCell, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,270), Vector3.one));
                        replaceTileMap.SetTransformMatrix(adjacent1, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,270), Vector3.one));
                        replaceTileMap.SetTransformMatrix(adjacent2, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,270), Vector3.one));
                    }
                    counter++;
                }

                if(counter == 4){
                    beenUpdated = true;
                }
            }
        }
    }
}

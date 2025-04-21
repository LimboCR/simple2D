using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerCommands", menuName = "Game/Game Manager Commands")]
public class GameManagerCommands: ScriptableObject
{
    [Header("Check if GM has access")]
    public bool GMHasAccess = false;

    [Space, Header("Commands to execute")]
    public bool LoadFromSaveFile;
    public bool UseGameManagerCommandsData = false;

    [Space, Header("Save files details")]
    public string SaveFileName = "QuickSave";
    public string SaveFolderName = "QuickSaves";
}

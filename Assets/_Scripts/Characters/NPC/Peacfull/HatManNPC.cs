using UnityEngine;
using Limbo.DialogSystem;

public class HatManNPC : PeacefulNPCBase
{
    [SerializeField] private CharacterDialogPackager _dialogPackager;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void StartDialog()
    {
        DialogNode startingNode = _dialogPackager.GetStartingNode();
        if (startingNode != null) GlobalEventsManager.StartDialog(startingNode);
    }
}

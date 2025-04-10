using UnityEngine;

public static class CommandsInterpreter
{
    public static void ExecuteCommand(string command)
    {
        //Example: give_item StandardHeal 10 1 (adds 10 healing spells to 1 quick slot if player inventory)
        if (command.StartsWith("give_item"))
        {
            string[] parts = command.Split(' ');

            if (parts.Length >= 2) // Ensure at least "give_item item_name"
            {
                string itemKey = parts[1]; // The item name
                int amount = (parts.Length >= 3) ? int.Parse(parts[2]) : 1; // Default to 1 if no amount is provided

                GiveItem(itemKey, amount);
            }
            else
            {
                Debug.LogWarning("Invalid command format! Use: give_item <item_name> [amount]");
            }
        }
    }

    public static void GiveItem(string itemKey, int? amount = 1)
    {

    }

    public static void SpawnNPC(string npcID, int? npcTeam = 0, Transform position = null, Quaternion? rotation = null)
    {

    }

    public static void ForceToAttackTarget(GameObject attacker, GameObject target)
    {

    }

    public static void ForceAllNPCToTarget(GameObject target, int? npcsTeam = 1, bool? forceChaseForever = true)
    {

    }
}

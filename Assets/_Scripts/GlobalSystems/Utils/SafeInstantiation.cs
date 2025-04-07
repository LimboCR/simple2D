using UnityEngine;

public static class SafeInstantiation
{
    #region World/Local Positioning Structs
    /// <summary>
    /// Structs build to provide World positioning for GameObjects. Use new World(Vector3 Position, Quaternion Rotation).
    /// </summary>
    public struct World
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public World(Vector3? position = null, Quaternion? rotation = null)
        {
            Position = position ?? default;
            Rotation = rotation ?? default;
        }
        public static World Default() => new(Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Structs build to provide Local positioning for GameObjects. Use new Local(Vector3 Position, Quaternion Rotation).
    /// </summary>
    public struct Local
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Local(Vector3? position = null, Quaternion? rotation = null)
        {
            Position = position ?? default;
            Rotation = rotation ?? default;
        }
        public static Local Default() => new(Vector3.zero, Quaternion.identity);
    }
    #endregion

    #region NPC Data Modification Structs
    /// <summary>
    /// Struct, built to provide safe modification for IDamagable values
    /// </summary>
    public struct HealthStats
    {
        public float? MaxHealth { get; private set; }
        public float? RegenRate { get; private set; }
        public float? RegenDelay { get; private set; }

        public HealthStats(float? maxHealth = null, float? regenRate = null, float? regenDelay = null)
        {
            MaxHealth = maxHealth;
            RegenRate = regenRate;
            RegenDelay = regenDelay;
        }

        public bool HasValue => MaxHealth.HasValue || RegenRate.HasValue || RegenDelay.HasValue;
    }

    /// <summary>
    /// Struct, built to provide safe modification for INPCMovable values 
    /// </summary>
    public struct MovableStats
    {
        public float? WalkSpeed { get; private set; }
        public float? RunSpeed { get; private set; }
        public float? JumpHeight { get; private set; }
        public int? LookingSideIndex { get; private set; }

        public MovableStats(float? walkSpeed = null, float? runSpeed = null, float? jumpHeight = null, int? lookingSideIndex = null)
        {
            WalkSpeed = walkSpeed;
            RunSpeed = runSpeed;
            JumpHeight = jumpHeight;
            LookingSideIndex = lookingSideIndex;
        }

        public bool HasValue => WalkSpeed.HasValue || RunSpeed.HasValue || JumpHeight.HasValue || LookingSideIndex.HasValue;
    }

    /// <summary>
    /// Struct, built to provide safe modification for IMultiCharacterData values
    /// </summary>
    public struct CharacterData
    {
        public int? NPCTeam { get; private set; }

        public CharacterData(int? npcTeam = null)
        {
            NPCTeam = npcTeam;
        }

        public bool HasValue => NPCTeam.HasValue;
    }
    #endregion

    #region Instantiate Empty GameObject
    /// <summary>
    /// Safely Instantiates new empty gameobject without unnecessary duplications, returnig refernce to self.
    /// </summary>
    /// <param name="objectName">Name of the object</param>
    /// <param name="world">World positioning</param>
    public static GameObject InstantiateEmpty(string objectName, World world)
    {
        GameObject current = new GameObject(objectName); // Set name of the object
        current.transform.position = world.Position;     // Set position
        current.transform.rotation = world.Rotation;     // Reset rotation

        return current;
    }

    /// <summary>
    /// Safely Instantiates new empty gameobject without unnecessary duplications, returnig refernce to self.
    /// </summary>
    /// <param name="objectName">Name of the object</param>
    /// <param name="world">World positioning</param>
    /// <param name="parrent">Parent GameObject</param>
    /// <returns></returns>
    public static GameObject InstantiateEmpty(string objectName, World world, Transform parrent)
    {
        GameObject current = new GameObject(objectName); // Set name of the object
        current.transform.SetParent(parrent);            // Set parent directly
        current.transform.position = world.Position;     // Set position
        current.transform.rotation = world.Rotation;     // Reset rotation

        return current;
    }

    /// <summary>
    /// Safely Instantiates new empty gameobject without unnecessary duplications, returnig refernce to self.
    /// </summary>
    /// <param name="objectName">Name of the object</param>
    /// <param name="local">Local positioning positioning</param>
    /// <param name="parrent">Parent GameObject</param>
    /// <returns></returns>
    public static GameObject InstantiateEmpty(string objectName, Local local, Transform parrent)
    {
        GameObject current = new GameObject(objectName);  // Set name of the object
        current.transform.SetParent(parrent);             // Set parent directly
        current.transform.localPosition = local.Position; // Set position
        current.transform.localRotation = local.Rotation; // Set rotation

        return current;
    }
    #endregion

    #region Safe NPC Instantiation
    /// <summary>
    /// <br>Performs a perfect instantiation on spawn points, overriding prefab values before Awake functions activates.</br>
    /// <br>You can provide different set of data to be overriden before Awake fucntion calls.</br>
    /// </summary>
    /// <param name="npcPrefab">Prefab of the NPC to instantiate</param>
    /// <param name="world">World positioning of the NPC</param>
    /// <param name="characterData">Setting NPCTeam value before Awake</param>
    /// <param name="healthStats">Modifying IDamagable data if given</param>
    /// <param name="movableStats">Modifying INPCMovable stats if given</param>
    /// <param name="parent">Setting parrent GameObject for NPC if given</param>
    /// <returns></returns>
    public static GameObject SafeNPCInstantiation(GameObject npcPrefab, World world,
        CharacterData? characterData = null, HealthStats ? healthStats = null,
        MovableStats? movableStats = null, Transform parent = null)
    {
        GameObject npc = GameObject.Instantiate(npcPrefab, world.Position, world.Rotation, parent);

        // Apply only the provided data
        var characterComponent = npc.GetComponent<IMultiCharacterData>();
        characterComponent?.SetInitialMultiCharData(characterData);

        var healthComponent = npc.GetComponent<IDamageble>();
        healthComponent?.SetInitialIDamageble(healthStats);

        var movementComponent = npc.GetComponent<INPCMovable>();
        movementComponent?.SetInitialINPCMovable(movableStats);

        return npc;
    }
    #endregion
}

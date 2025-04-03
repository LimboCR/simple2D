using System.Collections.Generic;
using UnityEngine;
using static SafeInstantiation;
using static GlobalEventsManager;

//[RequireComponent(typeof(Messanger))]
public class SpawnPointHandler : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject waypointPrefab;
    [SerializeField] private bool _instantiateOnStart;
    [SerializeField, Range(0.5f, 15f)] private float waypointLeftOffset = 2f;
    [SerializeField, Range(0.5f, 15f)] private float waypointRightOffset = 2f;
    [SerializeField] LayerMask GroundMask;

    [Space]
    [Header("Gizmos")]
    [SerializeField] bool drawWaypointsInEditor;

    [Space]
    [Header("Character To Spawn")]
    //public SpawnPresets NPC;
    public GameObject npc;

    [Space]
    [Header("Character settings")]
    public int NPCTeam;
    public bool InstantiateWaypoints;

    [Header("In game reference")]
    private GameObject _referenceCharacter;

    // To show ui if custom health settings are set to NPC
    [HideInInspector] public bool HealthScriptOverride;
    [HideInInspector] public float NPCMaxHealth;
    [HideInInspector] public float NPCRegenRate;
    [HideInInspector] public float NPCRegenDelay;

    //To show if custom movable stats provided
    [HideInInspector] public bool INPCMovableOverride;
    [HideInInspector] public float WalkSpeed;
    [HideInInspector] public float RunSpeed;
    [HideInInspector] public float JumpHeight;
    [HideInInspector] public int LookingSideIndex;

    void Awake()
    {
        SendSpawnPoint(gameObject);
    }

    private void OnDestroy()
    {
        RemoveSpawnPoint(gameObject);
    }

    void Start()
    {
        if (_instantiateOnStart)
        {
            SpawnCharacter();
        }
    }

    public void SpawnCharacter()
    {
        //Instantiates character and assigns it's reference to spawn point, to then access it's CharacterCommands to set waypoints to move between
        if (HealthScriptOverride && !INPCMovableOverride)
            _referenceCharacter = SafeNPCInstantiation(npc, new World(transform.position), new CharacterData(NPCTeam), new HealthStats(NPCMaxHealth, NPCRegenRate, NPCRegenDelay));

        else if (HealthScriptOverride && INPCMovableOverride)
            _referenceCharacter = SafeNPCInstantiation(npc, new World(transform.position), new CharacterData(NPCTeam), new HealthStats(NPCMaxHealth, NPCRegenRate, NPCRegenDelay), new MovableStats(WalkSpeed, RunSpeed, JumpHeight));

        else
            _referenceCharacter = SafeNPCInstantiation(npc, new World(transform.position), new CharacterData(NPCTeam));

        _referenceCharacter.GetComponent<CloseCombatNPCBase>().WaypointL = (Instantiate(waypointPrefab, 
            new Vector3(transform.position.x - waypointLeftOffset, transform.position.y, transform.position.z), Quaternion.identity, this.transform));
        _referenceCharacter.GetComponent<CloseCombatNPCBase>().WaypointR = (Instantiate(waypointPrefab, 
            new Vector3(transform.position.x + waypointRightOffset, transform.position.y, transform.position.z), Quaternion.identity, this.transform));
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying && drawWaypointsInEditor)
        {
            Vector3 potWaypointL = new Vector3(transform.position.x - waypointLeftOffset,
                transform.position.y, transform.position.z);

            Vector3 potWaypointR = new Vector3(transform.position.x + waypointRightOffset,
                transform.position.y, transform.position.z);

            RaycastHit2D hit = Physics2D.Raycast(potWaypointL, Vector2.down, 1, GroundMask);
            RaycastHit2D hit2 = Physics2D.Raycast(potWaypointR, Vector2.down, 1, GroundMask);

            //Draw sphere on waypoint left
            if (!hit) Gizmos.color = Color.red;
            else Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(potWaypointL, 0.25f);


            //Draw sphere on waypoint right
            if (!hit2) Gizmos.color = Color.red;
            else Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(potWaypointR, 0.25f);

        }
    }
}

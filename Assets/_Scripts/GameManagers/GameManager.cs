using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if(!Application.isPlaying) return null;

            if (instance == null)
                Instantiate(Resources.Load<GameManager>("GameManager"));
#endif
            return instance;
        }
    }

    [Header("GameStates")]
    public bool isGameActive = true;
    [SerializeField] private GameStates currentGameState;
    [SerializeField] private NewPlayerController _player;
    public NewPlayerController Player
    {
        get { return _player; }
        set
        {
            if (_player == null)
            {
                _player = value;
            }
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        GlobalEventsManager.OnStateChange.AddListener(ChangeGameState);
        Player = FindAnyObjectByType<NewPlayerController>();
    }

    private void Update()
    {
        if (Keyboard.current.numpad0Key.wasPressedThisFrame)
        {
            SaveSystem.Save();
        }
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            SaveSystem.Load();
        }
    }

    private void ChangeGameState(GameStates state)
    {
        currentGameState = state;
        GlobalEventsManager.BroadcastActualGameState(currentGameState);
    }

    private void SavePlayerData()
    {

    }
}

public enum GameStates
{
    Start,
    Pause,
    Unpause,
    Save,
    Restart,
    Exit
}

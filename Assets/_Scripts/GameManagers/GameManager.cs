using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("GameStates")]
    public bool isGameActive = true;
    private GameStates currentGameState;
    [SerializeField] protected int playerCoins { get; private set; }

    public int PlayerCoins { get { return playerCoins; } set { playerCoins = value; } }

    private void Awake()
    {
        GlobalEventsManager.OnStateChange.AddListener(ChangeGameState);
    }

    public void AddCoins(int amount)
    {
        PlayerCoins += amount;
    }

    public void DeductCoins(int amount)
    {
        PlayerCoins -= amount;
    }

    private void ChangeGameState(GameStates state)
    {
        currentGameState = state;
        GlobalEventsManager.BroadcastActualGameState(currentGameState);
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

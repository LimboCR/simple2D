using UnityEngine;
using TMPro;
using static CoroutineUtils;
public class Notification : MonoBehaviour
{
    public TMP_Text NotificationText;
    public float NotificationTime = 5f;

    public void SetText(string messageText)
    {
        NotificationText.text = messageText;
    }

    private void Start()
    {
        StartCoroutine(WaitThenDo(NotificationTime, () => Destroy(gameObject)));
    }
}

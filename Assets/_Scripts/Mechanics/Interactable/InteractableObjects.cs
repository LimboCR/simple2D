using System.Collections.Generic;
using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    [SerializeField] private bool _useTriggerZone;
    [SerializeField] private BoxCollider2D _triggerZone;

    [SerializeField] private CircleCollider2D _interactiveZone;
    [SerializeField] private GameObject _interactiveCanvas;

    public List<GameObject> collisionObjects = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collisionObjects.Add(other.gameObject);
        _interactiveCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collisionObjects.Remove(other.gameObject);
        _interactiveCanvas.SetActive(false);
    }
}

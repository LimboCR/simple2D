using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
                if(!IsLootDropped) DropLoot();
            }
        }
        private bool isOpened;

        [FoldoutGroup("Runtime"),Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }


        [FoldoutGroup("Loot")] public List<GameObject> _droppableLoot = new();
        [FoldoutGroup("Loot")] public float DropForce = 5f;
        [FoldoutGroup("Loot")] public bool IsLootDropped = false;

        public void DropLoot()
        {
            IsLootDropped = true;
            if (_droppableLoot.Count > 0)
            {
                foreach (GameObject loot in _droppableLoot)
                {
                    Vector3 spawnPos = transform.position + new Vector3(0, 0.4f, 0);
                    GameObject drop = Instantiate(loot, spawnPos, Quaternion.identity);

                    Vector2 randomDir = Random.insideUnitCircle.normalized + Vector2.up * 0.5f;
                    randomDir.Normalize();

                    if (drop.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                        rb.AddForce(randomDir * DropForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}

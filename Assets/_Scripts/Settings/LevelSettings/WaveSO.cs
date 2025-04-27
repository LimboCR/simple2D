using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings", menuName = "Game/Waves/Create Wave Settings")]
public class WaveSO : ScriptableObject
{
    public List<GameObject> NPCToSpawn;
    public float SpawnDealy = 1f;
    public bool WaveSpawnFinished;

    public IEnumerator InitiateWave(SpawnPointHandler sph, WaveSpawner ws)
    {
        foreach (GameObject obj in NPCToSpawn)
        {
            float elapsedTime = 0f;
            while (elapsedTime < SpawnDealy)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //Debug.Log($"[{ws.gameObject.name}] [WaveSO] [InitiateWave] Spawning NPC({obj.name}).");
            sph.npc = obj;
            sph.SpawnCharacter();
            ws.AddNpcToTrack(sph._referenceCharacter);
            sph.ResetSpawnPoint();
        }

        //Debug.Log($"[{ws.gameObject.name}] [WaveSO] [InitiateWave] Wave spawning finished.");
        WaveSpawnFinished = true;
        yield break;
    }
}

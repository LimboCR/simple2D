using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Limbo.CollectionUtils;
using UnityEngine.Audio;

[RequireComponent (typeof(BoxCollider2D), typeof(AudioSource))]
public class AudioSorceTrigger : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSourceType _audioSourceType;

    [Space, Header("Trigger Settings")]
    [SerializeField] private bool _playOnce;
    [SerializeField] private bool _resetOverTime;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _playOnDelay;

    [Space, Header("Audio Clips Settings")]
    [SerializeField] private TriggerAudioClipVariations _variationToPlay;
    [SerializeField] private AudioClip _clipToPlay;
    [SerializeField] private List<SoundContainer> _listOfClipsToPlay;
    private Dictionary<string, AudioClip> _clipsToPlay;

    private void Awake()
    {
        if(_audioSource == null) _audioSource = GetComponent<AudioSource>();

        if(_variationToPlay == TriggerAudioClipVariations.ClipVariation || _variationToPlay == TriggerAudioClipVariations.ClipsSequence)
        {
            _clipsToPlay = _listOfClipsToPlay.ToDictionarySafe(sound => sound.Key, sound => sound.Sound);
        }

        AudioMixer mixer = AudioManager.Instance.Mixer;
        if(_audioSourceType == AudioSourceType.Nature)
        {
            AudioMixerGroup[] groups = mixer.FindMatchingGroups("Nature");
            if (groups.Length > 0)
            {
                _audioSource.outputAudioMixerGroup = groups[0];
            }
        }
        else if (_audioSourceType == AudioSourceType.Enviroment)
        {
            AudioMixerGroup[] groups = mixer.FindMatchingGroups("Enviroment");
            if (groups.Length > 0)
            {
                _audioSource.outputAudioMixerGroup = groups[0];
            }
        }
    }
}

public enum TriggerAudioClipVariations
{
    Single,
    ClipVariation,
    ClipsSequence
}



[CustomEditor(typeof(AudioSorceTrigger))]
public class AudioSourceTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw all default fields
        DrawPropertiesExcluding(serializedObject, "_clipToPlay", "_listOfClipsToPlay");

        // Access the enum
        SerializedProperty variationProp = serializedObject.FindProperty("_variationToPlay");
        SerializedProperty sourceType = serializedObject.FindProperty("_audioSourceType");
        SerializedProperty audioSourceProp = serializedObject.FindProperty("_audioSource");
        //EditorGUILayout.PropertyField(variationProp);

        // Show the appropriate field based on the enum value
        if ((TriggerAudioClipVariations)variationProp.enumValueIndex == TriggerAudioClipVariations.Single)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_clipToPlay"));
        }
        else if ((TriggerAudioClipVariations)variationProp.enumValueIndex == TriggerAudioClipVariations.ClipVariation ||
            (TriggerAudioClipVariations)variationProp.enumValueIndex == TriggerAudioClipVariations.ClipsSequence)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_listOfClipsToPlay"), true);
        }

        //if ((AudioSourceType)sourceType.enumValueIndex == AudioSourceType.Nature)
        //{
        //    AudioSource audio = audioSourceProp.objectReferenceValue as AudioSource;

        //    if (audio != null)
        //    {
        //        AudioMixer mixer = AudioManager.GetMixer();
        //        AudioMixerGroup[] groups = mixer.FindMatchingGroups("Nature");

        //        if (groups.Length > 0)
        //        {
        //            audio.outputAudioMixerGroup = groups[0];
        //        }
        //        else
        //        {
        //            Debug.LogWarning("Nature group not found in Audio Mixer.");
        //        }
        //    }
        //}

        serializedObject.ApplyModifiedProperties();
    }
}
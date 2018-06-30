using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Dialog", menuName = "Audio/Dialog", order = 1)]
public class DialogSO : ScriptableObject
{
    public AudioClip clip;
    public Subtitle[] subtitles;
}
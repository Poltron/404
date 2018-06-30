using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Subtitle
{
    public float time;
    public string line;

    public Subtitle(float _time, string _line)
    {
        time = _time;
        line = _line;
    }
}

public class AudioTrigger : MonoBehaviour
{
    [SerializeField]
    DialogSO dialog;

    bool hasBeenPlayed;

	void Start()
    {
		
	}
	
    void OnDrawGizmos()
    {
        if (hasBeenPlayed)
            Gizmos.DrawIcon(transform.position, "audio_gizmo_red.png", true);
        else
            Gizmos.DrawIcon(transform.position, "audio_gizmo.png", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            //Debug.LogError("AudioTrigger collided with something else than player : this should not happen. Check the Physics Collision Grid.");
            return;
        }

        if (!hasBeenPlayed)
        {
            FindObjectOfType<AudioManager>().PlayDialog(dialog);
            hasBeenPlayed = true;
        }
    }
}

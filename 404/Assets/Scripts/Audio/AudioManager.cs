using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource musicAudioSource;

    [SerializeField]
    AudioSource dialogAudioSource;

    [SerializeField]
    RectTransform subtitleBox;

    [SerializeField]
    Text subtitleText;

    Coroutine activeSubtitleRoutine;

	[SerializeField]
	private AudioSource prefabSoundEffect;
	[SerializeField]
	private Transform contentSoundsEffect;
	private List<AudioSource> allSoundsEffect;

	private void Awake()
	{
		allSoundsEffect = new List<AudioSource>();
	}

	public void PlaySound(AudioClip clip)
	{
		AudioSource avaibleSource = allSoundsEffect.Find(s => s.isPlaying == false);

		if (avaibleSource == null)
		{
			avaibleSource = Instantiate(prefabSoundEffect, contentSoundsEffect);
			allSoundsEffect.Add(avaibleSource);
		}

		avaibleSource.clip = clip;
		avaibleSource.Play();
	}

    public void PlayDialog(DialogSO dialogObject)
    {
        if (dialogAudioSource.isPlaying)
        {
            dialogAudioSource.Stop();
            StopCoroutine(activeSubtitleRoutine);
        }

        dialogAudioSource.clip = dialogObject.clip;
        dialogAudioSource.Play();

        activeSubtitleRoutine = StartCoroutine(AnimateSubtitles(dialogObject));
    }

    private IEnumerator AnimateSubtitles(DialogSO dialog)
    {
        subtitleBox.gameObject.SetActive(true);

        for (int i = 0; i < dialog.subtitles.Length; ++i)
        {
            subtitleText.text = dialog.subtitles[i].line;
            yield return new WaitForSeconds(dialog.subtitles[i].time);
        }

        subtitleBox.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
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

    public void PlayDialog(DialogSO dialogObject)
    {
        if (dialogAudioSource.isPlaying)
        {
            dialogAudioSource.Stop();
            StopCoroutine(AnimateSubtitles(dialogObject));
        }

        dialogAudioSource.clip = dialogObject.clip;
        dialogAudioSource.Play();

        StartCoroutine(AnimateSubtitles(dialogObject));
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

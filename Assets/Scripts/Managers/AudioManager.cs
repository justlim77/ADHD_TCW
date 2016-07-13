#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;
using System;

[Serializable]
public class AudioManager : Singleton<AudioManager> {

    // Set to protected to prevent calling constructor
    protected AudioManager() { }

    private AudioMixer m_MainMix;
    private AudioSource m_BGM;
    private AudioSource m_SFX;

    private void Awake() {
        m_MainMix = Resources.Load("AudioMixers/MainMix", typeof(AudioMixer)) as AudioMixer;

        m_BGM = gameObject.AddComponent<AudioSource>();
        m_BGM.outputAudioMixerGroup = m_MainMix.FindMatchingGroups("BGM")[0];
        //m_BGM.mute = Convert.ToUInt16(PlayerPrefs.GetInt("bgmPref")) == 0 ? true : false;
        m_BGM.mute = false;
        m_BGM.playOnAwake = true;
        m_BGM.loop = true;

        m_SFX = gameObject.AddComponent<AudioSource>();
        m_SFX.outputAudioMixerGroup = m_MainMix.FindMatchingGroups("SFX")[0];
        //m_SFX.mute = Convert.ToUInt16(PlayerPrefs.GetInt("sfxPref")) == 0 ? true : false;
        m_SFX.mute = false;
        m_SFX.playOnAwake = false;
        m_SFX.loop = false;
    }

    /// <summary>
    /// [BGM] Plays a looping audio clip.
    /// </summary>
    /// <param name="clip">[AudioClip] Clip to be played.</param>
    /// <param name="volume">[float] Volume of SFX</param>
    public void PlayBGM(AudioClip clip, float volume = 1.0f)
    {
        //m_BGM.mute = Convert.ToUInt16(PlayerPrefs.GetInt("bgmPref")) == 0 ? true : false;
        m_BGM.clip = clip;
        m_BGM.volume = volume;

        if (!m_BGM.isPlaying)
            m_BGM.Play();
    }

    /// <summary>
    /// [SFX] Plays a single audio clip.
    /// </summary>
    /// <param name="clip">[AudioClip] Clip to be played.</param>
    /// <param name="pitch">[float] Initial pitch of SFX (+- 0.5f variation)</param>
    /// <param name="volume">[float] Volume of SFX</param>
    public void PlaySFX(AudioClip clip = null, float pitch = 1.0f, float volume = 1.0f)
    {
        if (clip == null) {
            //Debug.Log("No AudioClip parameter, returning null...");
            return;
        }
           
        //if (!m_SFX.isPlaying) {
            //m_SFX.mute = Convert.ToUInt16(PlayerPrefs.GetInt("sfxPref")) == 0 ? true : false;
            m_SFX.pitch = UnityEngine.Random.Range(pitch - 0.05f, pitch + 0.05f);
            m_SFX.volume = volume;

            m_SFX.PlayOneShot(clip);       
        //}
    }

    /// <summary>
    /// [SFX] Plays a random audio clip
    /// </summary>
    /// <param name="clips">[AudioClip[]] Array of sound effects</param>
    public void RandomizeSFX(params AudioClip[] clips)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        float randomPitch = UnityEngine.Random.Range(1.0f - 0.05f, 1.0f + 0.05f);

        m_SFX.pitch = randomPitch;
        m_SFX.clip = clips[randomIndex];
        m_SFX.Play();
    }

    public void ToggleBGM()
    {
        m_BGM.mute = !m_BGM.mute;
    }

    public void ToggleSFX()
    {
        m_SFX.mute = !m_SFX.mute;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsVolume : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] AudioMixer mixer;
    [Header("Objects")]
    [SerializeField] GameObject[] sliders;
    static string SavePath;
    Volume V = new Volume();

    public void ChangeMaster(float volume)
    {
        V.master = volume;
    }

    public void ChangeMusic(float volume)
    {
        V.music = volume;
    }

    public void ChangeEffects(float volume)
    {
        V.effects = volume;
    }

    public void Apply()
    {
        File.WriteAllText(SavePath, JsonUtility.ToJson(V));
        mixer.SetFloat("MasterVolume", V.master);
        mixer.SetFloat("MusicVolume", V.music);
        mixer.SetFloat("EffectsVolume", V.effects);
    }

    async void Start()
    {
        await Task.Delay(1);
        SavePath = Path.Combine(Application.dataPath + "/Saves", "Volume.json");

        if (!File.Exists(SavePath))
            File.WriteAllText(SavePath, JsonUtility.ToJson(V));
        V = JsonUtility.FromJson<Volume>(File.ReadAllText(SavePath));
        Apply();

        sliders[0].GetComponent<Slider>().value = V.master;
        sliders[1].GetComponent<Slider>().value = V.music;
        sliders[2].GetComponent<Slider>().value = V.effects;
    }

    [Serializable]
    public class Volume
    {
        public float master = 0;
        public float music = 0;
        public float effects = 0;
    }
}
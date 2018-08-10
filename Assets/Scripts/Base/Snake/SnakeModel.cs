using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using thelab.mvc;

public class SnakeModel : Model<SnakeApplication> {

    public System.Action Cycle;
    public int cycleCounter = 0;
    public float cycleTime = 0.1f;
    public AudioClip UIsfx;
    public AudioClip music;
    public GameObject box;
    public GameObject food;
    public GameObject snakeBody;
    public Map map;
    public Snake player;
    public bool foodOnMap = false;
    public bool pause = false;
    public bool gamePlaying = false;

    public string optionFileName = "Settings";
    public string[] savePath = new string[] {
        "user"
    };
    public AudioMixer audioMixer;
    public Settings settings;



    public void Init() {
        if (Serializer.Utility.DoesFileExist(Serializer.Utility.GetMainFilePath(optionFileName, "json", savePath))) {
            settings = Serializer.JSONSerializer<Settings>.Deserialize(optionFileName, savePath);
        }
        else {
            settings = new Settings();
            Serializer.JSONSerializer<Settings>.Serialize(app.model.settings, optionFileName, false, savePath);
        }
        LoadSettings();
    }

    public void LoadSettings() {
        settings.snake.musicVolume = Mathf.Clamp01(settings.snake.musicVolume);
        settings.snake.sfxVolume = Mathf.Clamp01(settings.snake.sfxVolume);
        settings.snake.difficulty = Mathf.Clamp01(settings.snake.difficulty);

        app.view.settings.musicSlider.slider.value = settings.snake.musicVolume;
        app.view.settings.sfxSlider.slider.value = settings.snake.sfxVolume;
        app.view.settings.difficultySlider.slider.value = 1 - settings.snake.difficulty;
        app.view.settings.use3D.toggle.isOn = settings.snake.use3D;
        ApplySettings();
    }

    public void SaveSettings() {
        Serializer.JSONSerializer<Settings>.Serialize(app.model.settings, optionFileName, false, savePath);
        ApplySettings();
    }

    public void ApplySettings() {
        audioMixer.SetFloat("SFXVolume", SliderToMixerValue(settings.snake.sfxVolume * settings.mastersfxVolume));
        audioMixer.SetFloat("MusicVolume", SliderToMixerValue(settings.snake.musicVolume * settings.mastersfxVolume));
        app.model.cycleTime = Mathf.Clamp01(settings.snake.difficulty);
        if (app.model.settings.snake.use3D) {
            app.view._2DCamera.Hide();
            app.view._3DCamera.Show();
        } else {
            app.view._2DCamera.Show();
            app.view._3DCamera.Hide();
        }
    }

    float SliderToMixerValue(float value) {
        return value * 65f - 65f;
    }

    public void SubscribeToCycle(System.Action method) {
        Cycle -= method;
        Cycle += method;
    }

    public void UnsubscribeToCycle(System.Action method) {
        Cycle -= method;
    }
}

public enum TileType {
    Nothing = 0,
    Ground = 1,
    Food = 2,
}

public enum Direction {
    Forward,
    Left,
    Right,
    Back
}
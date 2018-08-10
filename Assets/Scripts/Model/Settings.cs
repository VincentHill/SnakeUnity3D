using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings {
    public float masterMusicVolume = 1f;
    public float mastersfxVolume = 1f;
    public SerializedData snake;
    public Settings() {
        this.snake = new SerializedData();
    }
}
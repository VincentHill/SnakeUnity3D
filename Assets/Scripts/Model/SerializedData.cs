using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializedData {
    public float musicVolume;
    public float sfxVolume;
    public float difficulty;
    public bool use3D;
    public SerializedData(float musicVolume = 1f, float sfxVolume = 1f, float difficulty = 0.25f, bool use3D = true) {
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.difficulty = difficulty;
        this.use3D = use3D;
    }
}

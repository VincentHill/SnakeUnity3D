using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class ViewSettings : ViewUI {
    public ViewSFXSlider sfxSlider { get { return m_sfxSlider = Assert<ViewSFXSlider>(m_sfxSlider); } }
    private ViewSFXSlider m_sfxSlider;

    public ViewMusicSlider musicSlider { get { return m_musicSlider = Assert<ViewMusicSlider>(m_musicSlider); } }
    private ViewMusicSlider m_musicSlider;

    public ViewDifficultySlider difficultySlider { get { return m_difficultySlider = Assert<ViewDifficultySlider>(m_difficultySlider); } }
    private ViewDifficultySlider m_difficultySlider;

    public ViewUse3D use3D { get { return m_use3D = Assert<ViewUse3D>(m_use3D); } }
    private ViewUse3D m_use3D;
}

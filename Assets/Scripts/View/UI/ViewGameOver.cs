using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class ViewGameOver : ViewUI {
    public ViewText score { get { return m_score = Assert<ViewText>(m_score); } }
    private ViewText m_score;
}

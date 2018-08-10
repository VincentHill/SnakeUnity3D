using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using thelab.mvc;

public class ViewText : View<SnakeApplication> {

    Text textREF { get { return m_text = Assert<Text>(m_text); } }
    private Text m_text;

    public string text
    {
        get { return textREF.text; }
        set { textREF.text = value; }
    }
}

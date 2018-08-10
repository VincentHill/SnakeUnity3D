using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class ViewUI : View<SnakeApplication> {

    public bool UIEnabled = true;

    public void Hide() {
        UIEnabled = false;
        SetView(UIEnabled);
    }

    public void Show() {
        UIEnabled = true;
        SetView(UIEnabled);
    }

    public void SetView(bool value) {
        gameObject.SetActive(value);
    }
}

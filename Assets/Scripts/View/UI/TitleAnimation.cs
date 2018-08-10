using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour {

    private Text title;
    public float timer = 1f;
    float h;
    float s;
    float v;
    float counter = 0f;

    void Start() {
        Color.RGBToHSV(Color.red, out h, out s, out v);
        title = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

        title.color = Color.HSVToRGB(h, s, v);
        h = counter / timer % 1f;
        counter += Time.fixedDeltaTime;
        if (counter > 60) {
            counter = 0;
        }
    }
}

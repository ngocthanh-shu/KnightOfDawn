using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOpacityModifier : MonoBehaviour
{
    private Image image;
    [SerializeField] private Text childText;

    void Start() {
        image = GetComponent<Image>();
    }
    public void ChangeOpacity(float opacity) {
        Color color = image.color;
        color.a = opacity;
        image.color = color;

        color = childText.color;
        color.a = opacity;
        childText.color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour
{
    Image img;
    public float blinkSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (img == null)
            return;

        img.color = new Color(img.color.r, img.color.g, img.color.b, (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInstance : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject gameOverScreen;

    public static CanvasInstance Instance;
    void Awake()
    {
        Instance = this;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;
    public MainHandler mainHandler;
    public UIManager uiManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}

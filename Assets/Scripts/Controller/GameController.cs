using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{ 

    // Game controller components
    [Header("Game Components")]
    [SerializeField] private Reels reels;

    public bool reelsSpinning = false;

    // Add event listeners
    void Awake()
    {
        /*
         * Subscribe handlers to events
         */
        // Reel handlers 
        reels.OnReelsFullStop += ReelStopHandler;
         
        // Button handlers

    }

    void Start(){
        StartSpin();
    }

    void StartSpin(){
        reelsSpinning = true;
        reels.SpinReels();
        float delay = 2f;
        Invoke("StopReels", delay);
    }

    // Reel handler when reels have stopped
    //    private void ReelStopHandler(object sender, EventArgs eventArgs)
    private void ReelStopHandler()
    {
        float delay = 0.1f;
        Invoke("checkReels", delay);
    }

    private void checkReels(){
        reels.CheckLines();
        StartSpin();
    }

    private void StopReels(){
        reels.StopReels();
    }


    // Unsubscribe handlers to events
    void OnDisable()
    {
        reels.OnReelsFullStop -= ReelStopHandler; 
    }    
}

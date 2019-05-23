using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate void ReelsHandler();

public class Reels : MonoBehaviour
{

    //public event ReelsHandler OnReelsFullStop;
    public Action OnReelsFullStop;


    // Reels components
    public Reel[] reels;
    private Reel currentReel;
    private Reel reel1, reel2, reel3, reel4, reel5;
    private bool stopped;
    private bool allStopped;

    public List<SpriteRenderer> line1;
    public List<SpriteRenderer> line2;
    public List<SpriteRenderer> line3;

    public int credit = 100;

    public GameObject fx1;


    // Initialize Reels components
    void Awake()
    {
        reel1 = reels[0];
        reel2 = reels[1];
        reel3 = reels[2];
        reel4 = reels[3];
        reel5 = reels[4];

        // Add Stop handlers to the event of each reel
        reel1.OnFullStop += CompleteStopHandler;
        reel2.OnFullStop += CompleteStopHandler;
        reel3.OnFullStop += CompleteStopHandler;
        reel4.OnFullStop += CompleteStopHandler;
        reel5.OnFullStop += CompleteStopHandler;

        stopped = true;
        allStopped = true;
    }


    // Set up the spin idle animation for the reels
    private void SpinReels()
    {  
        allStopped = false;
        reel1.Spin();
        reel2.Spin();
        reel3.Spin();
        reel4.Spin();
        reel5.Spin();
    }


    public void CheckLines(){
        // load symbols
        line1.Clear();
        line1.Add(reel1.GetLocation(1));
        line1.Add(reel2.GetLocation(1));
        line1.Add(reel3.GetLocation(1));
        line1.Add(reel4.GetLocation(1));
        line1.Add(reel5.GetLocation(1));

        line2.Clear();
        line2.Add(reel1.GetLocation(2));
        line2.Add(reel2.GetLocation(2));
        line2.Add(reel3.GetLocation(2));
        line2.Add(reel4.GetLocation(2));
        line2.Add(reel5.GetLocation(2));

        line3.Clear();
        line3.Add(reel1.GetLocation(3));
        line3.Add(reel2.GetLocation(3));
        line3.Add(reel3.GetLocation(3));
        line3.Add(reel4.GetLocation(3));
        line3.Add(reel5.GetLocation(3));

        int lineTotal1 = 0;

        if (line1[0].sprite.name ==line1[1].sprite.name && line1[1].sprite.name ==line1[2].sprite.name ) {
            lineTotal1 = 3;
        }

        if (line1[1].sprite.name ==line1[2].sprite.name && line1[2].sprite.name ==line1[3].sprite.name ) {
            lineTotal1 = 3;
        }

        if (line1[2].sprite.name ==line1[3].sprite.name && line1[3].sprite.name ==line1[4].sprite.name ) {
            lineTotal1 = 3;
        }

        if (line1[0].sprite.name ==line1[1].sprite.name  && line1[1].sprite.name == line1[2].sprite.name  && line1[2].sprite.name  == line1[3].sprite.name ) {
            lineTotal1 = 4;
        }

        if (line1[1].sprite.name ==line1[2].sprite.name  && line1[2].sprite.name ==line1[3].sprite.name  && line1[3].sprite.name  == line1[4].sprite.name ) {
            lineTotal1 = 4;
        }

        highLight(line1[0]);
        highLight(line1[1]);
        highLight(line1[2]);
        highLight(line1[3]);
        highLight(line1[4]);

        highLight(line2[0]);
        highLight(line2[1]);
        highLight(line2[2]);
        highLight(line2[3]);
        highLight(line2[4]);

        highLight(line3[0]);
        highLight(line3[1]);
        highLight(line3[2]);
        highLight(line3[3]);
        highLight(line3[4]);
        
        Debug.Log(lineTotal1);

        credit = credit + lineTotal1;
    }

    private void highLight(SpriteRenderer sprite){
        Instantiate(fx1);
        fx1.transform.position = sprite.transform.position;
    }


    // Set up the landing and stop animation for the reels
    private void StopReels()
    {   
        float delay1 = UnityEngine.Random.value;
        float delay2 = UnityEngine.Random.value;
        float delay3 = UnityEngine.Random.value;
        float delay4 = UnityEngine.Random.value;
        float delay5 = UnityEngine.Random.value;
        Invoke("stopReel1", delay1);
        Invoke("stopReel2", delay2);
        Invoke("stopReel3", delay3);
        Invoke("stopReel4", delay4);
        Invoke("stopReel5", delay5);
    }

    private void stopReel1(){
        reel1.Stop();
    }
    private void stopReel2(){
        reel2.Stop();
    }
    private void stopReel3(){
        reel3.Stop();
    }
    private void stopReel4(){
        reel4.Stop();
    }
    private void stopReel5(){
        reel5.Stop();
    }


    // Helper method to check the current moving status of reels
    private bool CheckStatus()
    { 
        //Debug.Log("hit check status");
        for (int i = 0; i < reels.Length; i++)
        { 
            currentReel = reels[i];
            stopped = currentReel.HasStopped(); 
        }  

        return stopped;
    }


    // Handler when all reels have stopped 
    //private void CompleteStopHandler(object sender, EventArgs eventArgs)
    private void CompleteStopHandler()
    {
        // All reels need to be completely stopped before dispatching the event 
        if (CheckStatus())
        {
            // Dispatch Full Stop event
            if (OnReelsFullStop != null && allStopped==false)
            {
                //Debug.Log("SEND EVENT");
                OnReelsFullStop();
                allStopped = true;
            }
            else
            {
                //Debug.Log("FullStop event is null");
            }
        }
    }
    

    // Unubscribe handlers from events
    void OnDisable()
    {
        reel1.OnFullStop -= CompleteStopHandler;
        reel2.OnFullStop -= CompleteStopHandler;
        reel3.OnFullStop -= CompleteStopHandler; 
        reel4.OnFullStop -= CompleteStopHandler;
        reel5.OnFullStop -= CompleteStopHandler; 
    } 


    // PUBLIC METHODS
    public void Spin()
    {
        credit--;
        SpinReels();
        float delay = UnityEngine.Random.value*3;
        Invoke("Stop", delay);
    }

    public void Stop()
    {
        StopReels();
        float delay = UnityEngine.Random.value*3;
        Invoke("Spin", delay);
    } 
}

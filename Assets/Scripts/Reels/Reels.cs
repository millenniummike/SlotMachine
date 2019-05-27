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
    private bool stopped;
    public bool allStopped;

    public List<SpriteRenderer> line1;
    public List<SpriteRenderer> line2;
    public List<SpriteRenderer> line3;

    public List<SpriteRenderer> line4;
    public List<SpriteRenderer> line5;
    public List<SpriteRenderer> line6;
    public List<SpriteRenderer> line7;
    public List<SpriteRenderer> line8;

    public List<List<SpriteRenderer>> lines = new List<List<SpriteRenderer>>();

    public int credit = 100;

    public GameObject fx1;


    // Initialize Reels components
    void Awake()
    {
        lines.Add(line1);
        lines.Add(line2);
        lines.Add(line3);
        lines.Add(line4);
        lines.Add(line5);
        lines.Add(line6);
        lines.Add(line7);
        lines.Add(line8);
        // Add Stop handlers to the event of each reel
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].OnFullStop += CompleteStopHandler;
        }
        stopped = true;
    }


    // Set up the spin idle animation for the reels
    public void SpinReels()
    {  
        credit = credit -3; // assume 3 bet
        allStopped = false;
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].StartSpin();
            reels[i].targetReelPosition = -1;
        }
    }


    public void CheckLines(){
        // load symbols

        // move to serverside

        for (int i = 0; i < 8; i++){
            lines[i].Clear();
        }

        for (int i = 0; i < reels.Length; i++)
        {
            line1.Add(reels[i].GetLocation(1));
            line2.Add(reels[i].GetLocation(2));
            line3.Add(reels[i].GetLocation(3));
        }

        line4.Add(reels[0].GetLocation(1));
        line4.Add(reels[0].GetLocation(2));
        line4.Add(reels[0].GetLocation(3));

        line5.Add(reels[1].GetLocation(1));
        line5.Add(reels[1].GetLocation(2));
        line5.Add(reels[1].GetLocation(3));

        line6.Add(reels[2].GetLocation(1));
        line6.Add(reels[2].GetLocation(2));
        line6.Add(reels[2].GetLocation(3));

        line7.Add(reels[3].GetLocation(1));
        line7.Add(reels[3].GetLocation(2));
        line7.Add(reels[3].GetLocation(3));

        line8.Add(reels[4].GetLocation(1));
        line8.Add(reels[4].GetLocation(2));
        line8.Add(reels[4].GetLocation(3));

        //vertical
        /*
        for (int i = 3; i < 8; i++){
            List<SpriteRenderer> line = lines[i];
            if (line[0].sprite.name ==line[1].sprite.name && line[1].sprite.name == line[2].sprite.name ) {
                Payout(line[0].sprite.name,1);
                highLight(line[0]);
                highLight(line[1]);
                highLight(line[2]);
            }
        }
        */

        //horizontal
        for (int i = 0; i < 3; i++){
            List<SpriteRenderer> line = lines[i];

            // 5 in a row
            if (line[0].sprite.name == line[1].sprite.name  && line[1].sprite.name ==line[2].sprite.name  && line[2].sprite.name ==line[3].sprite.name  && line[3].sprite.name  == line[4].sprite.name ) {
                Payout(line[0].sprite.name,3);
                highLight(line[0]);
                highLight(line[1]);
                highLight(line[2]);
                highLight(line[3]);
                highLight(line[4]);
                continue;
            }

            // 4 in a row

            if (line[0].sprite.name == line[1].sprite.name  && line[1].sprite.name == line[2].sprite.name  && line[2].sprite.name  == line[3].sprite.name ) {
                Payout(line[0].sprite.name,2);
                highLight(line[0]);
                highLight(line[1]);
                highLight(line[2]);
                highLight(line[3]);
                continue;
            }

            if (line[1].sprite.name ==line[2].sprite.name  && line[2].sprite.name ==line[3].sprite.name  && line[3].sprite.name  == line[4].sprite.name ) {
                Payout(line[0].sprite.name,2);
                highLight(line[1]);
                highLight(line[2]);
                highLight(line[3]);
                highLight(line[4]);
                continue;
            }

            // 3 in a row
            if (line[0].sprite.name ==line[1].sprite.name && line[1].sprite.name == line[2].sprite.name ) {
                Payout(line[0].sprite.name,1);
                highLight(line[0]);
                highLight(line[1]);
                highLight(line[2]);
                continue;
            }
           

            if (line[1].sprite.name ==line[2].sprite.name && line[2].sprite.name == line[3].sprite.name ) {
                Payout(line[0].sprite.name,1);
                highLight(line[1]);
                highLight(line[2]);
                highLight(line[3]);
                continue;
            }


            if (line[2].sprite.name ==line[3].sprite.name && line[3].sprite.name ==line[4].sprite.name ) {
                Payout(line[0].sprite.name,1);
                highLight(line[2]);
                highLight(line[3]);
                highLight(line[4]);
                continue;
            }
        }
    }

    private void Payout(string symbol, int number){
        // number 1 = 3 lines, number 2 = 4 lines, number 3 = 5 lines
        int lineTotal = 0;
        switch (symbol) {
            case "cherries":
                lineTotal=3 * number;
            break;
            case "grapes":
                lineTotal=5 * number;
            break;
            case "coconut":
                lineTotal=20 * number;
            break;
            case "lemon":
                lineTotal=50 * number;
            break;
            case "fruitylandlogo":
                lineTotal=100 * number;
            break;
        }
        Debug.Log("Payout = " + lineTotal);
        credit = credit + lineTotal;
    }

    private void highLight(SpriteRenderer sprite){
        if (sprite) {
            GameObject go = Instantiate(fx1);
            go.transform.position = sprite.transform.position;
        }
    }


    // Set up the landing and stop animation for the reels
    public void StopReels()
    {   
        for (int i = 0; i < reels.Length; i++)
        {
            int r = UnityEngine.Random.Range(0,31); //** TODO get from server */ 
            StartCoroutine(stopReel(reels[i],r));
        }
        //StartCoroutine(stopReel(reels[0],0));
        //StartCoroutine(stopReel(reels[1],24));
        //StartCoroutine(stopReel(reels[2],6));
        //StartCoroutine(stopReel(reels[3],0));
        //StartCoroutine(stopReel(reels[4],0));
    }

    IEnumerator stopReel(Reel reel, int position)
    {
 
        float delay = UnityEngine.Random.Range(1, 2.0f);
        yield return new WaitForSeconds(delay);
        reel.StopPosition(position);
    }

    // Helper method to check the current moving status of reels
    private bool CheckStatus()
    { 
        for (int i = 0; i < reels.Length; i++)
        { 
            currentReel = reels[i];
            if (currentReel.isSpinning) {return false;}
        }  

        return true;
    }


    // Handler when all reels have stopped 
    //private void CompleteStopHandler(object sender, EventArgs eventArgs)
    private void CompleteStopHandler()
    {
        //Debug.Log("Stop Handler");
        // All reels need to be completely stopped before dispatching the event 
        //Debug.Log(CheckStatus());
        if (CheckStatus())
        {
            // Dispatch Full Stop event
            if (OnReelsFullStop != null && allStopped==false)
            {
                OnReelsFullStop();
                allStopped = true;
            }
        }
    }

    // Unubscribe handlers from events
    void OnDisable()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].OnFullStop -= CompleteStopHandler;
        }
    }
}

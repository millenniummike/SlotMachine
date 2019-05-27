using System;
using System.Collections;
using System.Collections.Generic;
//using System.Security.Policy;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reel : MonoBehaviour
{  
    //public event ReelsHandler OnFullStop;

    public Action OnFullStop;
    
    // Reel components (icons/sprites)
    public SpriteRenderer[] icons; 
    public Sprite[] symbols;

    // Visual 
    private SpriteRenderer currentIcon;
    private int symbolIndex;
    private float xPos;
    private float yPos;
    private float zPos;
    public int reelPosition;

    // Animation/Measurements
    public float speed = 25f;
    private float smoothTime;
    public float iconHeight;
    public float topBound;
    public float bottomBound;
    public float scaleY;

    // Landing animation & calculations/timing  
    private int topMostIndex;
    private float yVelocity;
    private float endPoint;
    private float landingPos;  
    private float currentYpos; 
    
    // Reel status
    public bool isSpinning;
    public bool stopped;
    public bool isStopping;
    public int targetReelPosition = -1;

    // Initialize reel components
    void Awake()
    {   
        smoothTime = 0.15f;
        //iconHeight = 3f;
        scaleY=transform.parent.parent.transform.localScale.y;
        topBound = (transform.localPosition.y + iconHeight * (icons.Length-2)) ; 
        bottomBound = topBound - iconHeight * (icons.Length);   
        yVelocity = 0.0f;

        // Is the reel spinning at start up?
        isSpinning = false;
        stopped = true;
        targetReelPosition = -1;

        // Create initial set of icons
        SetIcon(symbols); 
    }


    // Execute spin idling and landing animation controlled by flags
    // Do not call Stop function inside because it dispatches the full stop event
    void Update()
    {
        // If reel needs to spin
        if (isSpinning && isStopping == false)
        {
            Spin();
        }
        
        if (isSpinning && isStopping == true)
        {
            Stop(); 
        } 

        if (targetReelPosition==reelPosition) {
            isStopping = true;
            targetReelPosition = -1;
        }
    } 

    // Create initial set of icons
    private void SetIcon(Sprite[] symbol)
    {
        reelPosition = 0;
        for (int i = 0; i < icons.Length; i++)
        {
            // Get reference to the object
            currentIcon = icons[i];

            // Assign a sprite to an icon 
            currentIcon.sprite = symbol[i];

            // Assign the x & y positions of the icons
            xPos = currentIcon.transform.localPosition.x;
            yPos = topBound - (i * iconHeight);
            zPos = currentIcon.transform.localPosition.z;

            // Set its position 
            currentIcon.transform.localPosition = new Vector3(xPos, yPos, zPos);
        }
    }


    // Render the set of icons
    private void RenderIcons(float speed)
    {
        //speed=0;
        // Indicate that the reels have started to spin
        // The reels are now moving
        //isSpinning = true; 

        for (int i = 0; i < icons.Length; i++)
        {
            // Get reference to the object
            currentIcon = icons[i];

            // Apply speed
            xPos = currentIcon.transform.localPosition.x; 
            yPos = currentIcon.transform.localPosition.y;
            zPos = currentIcon.transform.localPosition.z;
            yPos -= speed; 

            // Update the icon's new position
            currentIcon.transform.localPosition = new Vector3(xPos, yPos, zPos);

            // Check bounds
            if (currentIcon.transform.localPosition.y < bottomBound)
            {
                reelPosition++;
                if (reelPosition>=symbols.Length) {reelPosition=0;}
                /*
                 * Update current icon position  
                 */
                yPos = topBound - speed;      
                currentIcon.transform.localPosition = new Vector3(xPos, yPos, zPos);

                // Store its index
                topMostIndex = i; 

                // Give the icon a new random symbol
                //symbolIndex = Random.Range(0, symbols.Length);
                
                // Choose next symbol on reel
                symbolIndex = symbols.Length-reelPosition-1;
                icons[i].GetComponent<SpriteRenderer>().sprite = symbols[symbolIndex];
            }
        } 
    }


    // Start landing animation
    private void StartLanding()
    {  
        // Get reference to the object
        currentIcon = icons[topMostIndex];

        // Define start position for the animation
        currentYpos = currentIcon.transform.localPosition.y;

        // Define the target landing position for the animation
        endPoint = topBound - iconHeight;

        // Apply easing to the icon's landing animation
        landingPos = Mathf.SmoothDamp(currentYpos, endPoint, ref yVelocity, smoothTime);

        // Gradually decrease idling speed until it reaches the "endpoint"   
        RenderIcons(currentYpos - landingPos);
        /*
         * This is the point where the reels have completely landed/stopped
         */
        if (currentYpos == landingPos)
        {
            // The reel is no longer spinning
            isSpinning = false;

            // Dispatch reel stopped state event
            if (OnFullStop != null && stopped == false)
            { 
                OnFullStop(); 
                stopped = true;
                isStopping = false;
                SyncSymbols(); 
            }
            else
            {
                //Debug.Log("OnFullStop event is null");
            } 
        }
    } 

        
    private void SyncSymbols(){
       // Debug.Log("Syncing symbols");
        for (int i = 0; i < icons.Length; i++)
        {
            currentIcon = icons[i];
            Vector3 pos = currentIcon.transform.localPosition;
            float y = pos.y;
            pos.y = (float)Math.Round(y / 3.0f) * 3;
            currentIcon.transform.localPosition = pos;
        }
    }


    // PUBLIC METHODS
    public void Spin()
    {
        // Move icons by speed * fixed delta time
        RenderIcons(speed * Time.fixedDeltaTime);
    }

    public void StartSpin(){
        stopped = false;
        isSpinning = true;
        isStopping = false;
    }

    public SpriteRenderer GetLocation(int loc){
        double yBottom = 3.0;
        if (loc==1) {yBottom=3.0;}
        if (loc==2) {yBottom=0;}
        if (loc==3) {yBottom=-3;}
        for (int i = 0; i < icons.Length; i++)
        {
            // Get reference to the object
            SpriteRenderer icon = icons[i];
            if (icon.transform.localPosition.y >= yBottom && icon.transform.localPosition.y < yBottom + 3){
                return icons[i].GetComponent<SpriteRenderer>();
            }
        }
        return null;
    }

    public void Stop()
    {
        StartLanding();
    }

    public void StopPosition(int position) {
        targetReelPosition = position;
    }
}

using System;
using UnityEngine;

public class Timer : MonoBehaviour

{
   // start declares if timer is runing or not.
    bool start=false;
    float timeElapsed=0f;
    //1 in time = 0.02 seconds thus 50 = 1 second
    float timeAllocatedSeconds;

    //Progress is how close the timer is to being complete 0 being just started and 1 being complete.
    float progress=0f;

    void FixedUpdate()
    {
        progress = timeElapsed / (timeAllocatedSeconds * 50);
      if (start==true&&timeElapsed<=timeAllocatedSeconds*50)
      {
       timeElapsed++;
       if (timeElapsed==timeAllocatedSeconds*50)
       {
        TimerComplete();
       }
      } 
      else
      {
        timeElapsed=0f;
        progress=0f;    
       start = false; 
      } 
    }

    public void StartTimer(float timeSecs){
        if (start ==false)
        {
            timeAllocatedSeconds=timeSecs;
            start = true;
        }  
      

    }

    public float GetProgress (){
        //returns the progress of the timer 
        
        return progress;
    }

    public void TimerComplete(){

        //does something when the timer is complete
    }
}

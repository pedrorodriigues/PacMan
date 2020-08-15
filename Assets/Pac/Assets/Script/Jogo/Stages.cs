using System;
using UnityEngine;

public class Stages : MonoBehaviour
{

    public float[][] speed { get; set; }
    public float[] refind { get; set; }
    public float[][] forward { get; set; }
    public float[][] startTime { get; set; }
    public float[][] cornerTime { get; set; }
    public float[] ScoreMult { get; set; }
    public float[] ScaredStateTime { get; set; }
    public float[][] RefindAfterCorner { get; set; }
    public float[] rndCornerTime { get; set; }
    public float[] timeToRespawn { get; set; }




    public Stages()
    {
        //refind = new float[][] { new float[] { 2, 2, 2, 2 }, new float[] { 0.5f, 0.5f, 0.5f, 0.5f } };
        speed = new float[][] { new float[] { 2, 2, 2, 2 }, new float[] { 4, 4,4,4 } };
        refind = new float[] { 0.5f, 0.5f, 0.5f, 0.5f };
        forward = new float[][] { new float[] { 0, 5.5f, 8, -5.5f }, new float[] { 0, 6 , 7 , -6 } };
        startTime = new float[][] { new float[] {9, 0, 3,6 }, new float[] { 5, 2,2,2 } };
        cornerTime = new float[][] { new float[] { 20, 20, 20, 20 }, new float[] { 28, 28, 28, 28 } };
        RefindAfterCorner = new float[][] { new float[] { 10, 10, 10, 10 }, new float[] { 28, 28, 28, 28 } };
        ScoreMult = new float[] {10,15,20,25,30};
        ScaredStateTime = new float[] { 15, 15, 20, 25, 30 };
        rndCornerTime = new float[] { 12, 15, 20, 25, 30 };
        timeToRespawn = new float[] { 5, 5, 20, 25, 30 };

    }
  


}

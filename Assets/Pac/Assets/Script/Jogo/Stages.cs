using System;
using UnityEngine;

//seta os parametros que controlaram o jogo
public class Stages : MonoBehaviour
{
    public int[] ghostActive { get; set; }
    public float[][] speed { get; set; }
    public float refind { get; set; }
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
        ghostActive = new int[] {1, 2, 3, 4};
        speed = new float[][] { new float[] { 2 }, new float[] { 2.2f, 2 }, new float[] { 2.7f, 2.5f,2.5f }, new float[] { 3.4f, 3f, 3,2f,2.8f } };
        refind =   0.5f ;
        forward = new float[][] { new float[] { 0, 5.5f, 8, -5.5f }, new float[] { 0, 5.5f, 8, -5.5f }, new float[] { 0, 5.5f, 8, -5.5f }, new float[] { 0, 5.5f, 8, -5.5f } };
        startTime = new float[][] { new float[] {0 }, new float[] { 0, 2 },  new float[] { 0, 2,4}, new float[] { 0, 2, 4,6 } };
        cornerTime = new float[][] { new float[] { 15 }, new float[] { 20, 20 }, new float[] { 25, 20, 20 } , new float[] { 30, 25, 25,25} };
        RefindAfterCorner = new float[][] { new float[] { 10 }, new float[] { 10, 12 }, new float[] { 7, 10,12 }, new float[] { 5, 7,10,12 } };
        ScoreMult = new float[] {10,15,20,25,30};
        ScaredStateTime = new float[] { 15, 12, 10, 7 };
        rndCornerTime = new float[] { 12, 15, 20, 25, 30 };
        timeToRespawn = new float[] { 10, 8, 7, 5};
    }
  


}

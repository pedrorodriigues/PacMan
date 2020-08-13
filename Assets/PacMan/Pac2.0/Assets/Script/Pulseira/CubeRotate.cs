using System;
using System.Collections;
using System.Collections.Generic;
using AHRS;
using UnityEngine;

public class CubeRotate : MonoBehaviour {

    double[] euler = new double[3];
    List<double[]> rawData = new List<double[]>();
    double[] quaternions = new double[4];
	public GameObject Cube;
	public float speed = 3.0f;
    float vx = 0;
    float vy = 0;
    float vz = 0;

    private Vector3 v0;

    private Vector3 v;
    private Vector3 d;
    private Vector3 d0;
    private Vector3 lAcc;
    private Vector3 lastAcc;

    public Vector3 linearAccel;

    public Vector3 gravity;
    private Quaternion inicialPosition;
    private float t0 = 0;
    float alpha = 0.8f;

    const int MATRIX_ROWS = 3;
    const int MATRIX_COLUMNS = 3;

    const float samplePeriod = 1 / 256;

    double[,] rotMatrix = new double[MATRIX_ROWS, MATRIX_COLUMNS];
    double[,] inverseRotMatrix = new double[MATRIX_ROWS, MATRIX_COLUMNS];

    Rigidbody rb;
	// Use this for initialization
	void Start () {

        //lastAcc = new Vector3((float)rawData[0][0], (float)rawData[0][1], (float)rawData[0][2]);
        lAcc = new Vector3();
        gravity = new Vector3();
        v0 = Vector3.zero;
        d0 = Vector3.zero;
        v = Vector3.zero;
        d = Vector3.zero;
        inicialPosition = transform.rotation;
        
        //euler = BSNHardwareInterface.ReceiveEuler();
        // rotMatrix = BSNHardwareInterface.ReceiveRotationMatrix();
        rb = GetComponent<Rigidbody>();
        // StartCoroutine(waiter());
	}


    IEnumerator waiter()
    {
        //Rotate 90 deg
        yield return new WaitForSeconds(2);
        quaternions = BSNHardwareInterface.ReceiveQuaternions();

        //Wait for 4 seconds
        // yield return new WaitForSeconds(3);

        //Rotate 40 deg
        // rawData = BSNHardwareInterface.ReceiveRawData();

        //Wait for 2 seconds
        // yield return new WaitForSeconds(2);

    }
	// Update is called once per frame
	void Update () {

        //transform.eulerAngles = new Vector3((float)euler[0], (float)euler[1], (float)euler[2]);
        // float [] quat;
        // if(rawData.Count > 0){
        //     //linearAccel = linAcc();
        //     quat = linAcc();
        //     transform.rotation = new Quaternion(quat[0], quat[1], quat[2], quat[3]);
        //     //transform.rotation = new Quaternion((float)quaternions[1], (float)quaternions[2], (float)quaternions[3], (float)quaternions[0]);
        // }
        // Debug.Log(">>>>>>>>>ACEL X = " + linearAccel.x);
        // Debug.Log(">>>>>>>>>ACEL Y = " + linearAccel.y);
        // Debug.Log(">>>>>>>>>ACEL Z = " + linearAccel.z);
        //Vector3 actualLinearAceleration = new Vector3((float)rawData[0], (float)rawData[1], (float)rawData[2]);
        //linearAceleration = actualLinearAceleration - lastLinearAceleration;

        Debug.Log(">>>>>>>>>CUBO W = " + DebuggerQuaternions.quaternions[0]);
        Debug.Log(">>>>>>>>>CUBO X = " + DebuggerQuaternions.quaternions[1]);
        Debug.Log(">>>>>>>>>CUBO Y = " + DebuggerQuaternions.quaternions[2]);
        Debug.Log(">>>>>>>>>CUBO Z = " + DebuggerQuaternions.quaternions[3]);
        // Vector3 acc = Input.acceleration;
        // Debug.Log(linearAccel.x);
        // Debug.Log(linearAccel.y);
        // Debug.Log(linearAccel.z);
        // Debug.Log(SystemInfo.supportsAccelerometer);
        //rb.AddForce(acc.x * speed, 0, acc.y * speed);
        // transform.Translate((float)rawData[0] * speed, 0, (float)rawData[2] * speed);
        // transform.Translate((float)rawData[0] * speed, 0, (float)rawData[2] * speed);

        // if (linearAccel.y >= 0.5f)
        // {
        //     transform.Translate(new Vector3(0, linearAccel.y * 2, 0));
        //     if (linearAccel.x >= 0.5f || linearAccel.x <= -0.5f)
        //     {
        //         transform.Translate(new Vector3(linearAccel.x * 2, 0, 0));
        //     }
        //     if (linearAccel.z >= 0.5f || linearAccel.z <= -0.5f)
        //     {
        //         transform.Translate(new Vector3(0, 0, -linearAccel.z * 2));
        //     }
        // }
        Cube.transform.rotation = new Quaternion((float)DebuggerQuaternions.quaternions[1], (float)DebuggerQuaternions.quaternions[2], (float)DebuggerQuaternions.quaternions[3], (float)DebuggerQuaternions.quaternions[0]);
       
        //transform.Translate(linearAccel.x, linearAccel.y, 0);
        // transform.Translate(linearAccel.x * Time.deltaTime , linearAccel.y * Time.deltaTime , 0);
        // transform.Translate((float)rawData[0] * speed, (float)rawData[1] * speed, 0);

        // transform.position = new Vector3(transform.position.x + linearAccel.x, transform.position.y + linearAccel.y , transform.position.z);




        // vx += Input.acceleration.x * Time.deltaTime;
        // vy += Input.acceleration.y * Time.deltaTime;
        // vx += Input.acceleration.z * Time.deltaTime;

        // transform.Translate(vx* speed, vy * speed, 0);

	}

    // public Vector3 linAcc()
    // {
    //     lAcc = (Input.acceleration - lastAcc);
    //     lastAcc = Input.acceleration;
    //     return lAcc;
    // }
    // public Vector3 linAcc()
    // {
    //     lAcc = (Input.acceleration - lastAcc);
    //     lastAcc = Input.acceleration;
    //     return lAcc;
    // }
    // public Vector3 linAcc()
    // {


    //     gravity = alpha * gravity + (1 - alpha) * lastAcc;
    //     // gravity[0] = alpha * gravity[0] + (1 - alpha) * event.values [0];
    //     // gravity [1] = alpha * gravity [1] + (1 - alpha) * event.values [1];
    //     // gravity [2] = alpha * gravity [2] + (1 - alpha) * event.values [2];

    //     lAcc  = lastAcc - gravity;

    //     // linear_acceleration [0] = event.values [0] - gravity [0];
    //     // linear_acceleration [1] = event.values [1] - gravity [1];
    //     // linear_acceleration [2] = event.values [2] - gravity [2];
    //     return lAcc;
    // }


    // public Vector3 linAcc()
    // {

    // v = v0 + Input.acceleration * Time.deltaTime;
    // v0 = v;

    // d = d0 + v * Time.deltaTime;
    // d0 = d;

    // return v;
    // }


    public float[] linAcc()
    {

        Debug.Log(rawData[0][0]);
        Debug.Log(rawData[0][1]);
        Debug.Log(rawData[0][2]);
        Debug.Log(rawData[1][0]);
        Debug.Log(rawData[1][1]);
        Debug.Log(rawData[1][2]);
        List<float[,]> R = new List<float[,]>();
        R.Add(new float[3,3]);
        R.Add(new float[3,3]);
        R.Add(new float[3,3]);
        MahonyAHRS ahrs = new MahonyAHRS(samplePeriod, 1);
        Vector3 acel = new Vector3();


        ahrs.Update((float)(rawData[1][0] * (Math.PI / 180)), (float)(rawData[1][1] * (Math.PI / 180)), (float)(rawData[1][2] * (Math.PI / 180)), (float)rawData[0][0], (float)rawData[0][1], (float)rawData[0][2], (float)rawData[2][0], (float)rawData[2][1], (float)rawData[2][2]);  // gyroscope units must be radians
        
        for (int i = 0; i < rawData[1].Length; i++)
        {
            //ahrs.UpdateIMU(gyr(i,:) * (Math.PI / 180), acc(i,:));  % gyroscope units must be radians
            R[i] = Transpose(quaternions2RotateMatrixF(ahrs.Quaternion));    //% transpose because ahrs provides Earth relative to sensor
        }

        return ahrs.Quaternion;
        float[,] tcAcc = new float[3,3];

        for (int i = 0; i < rawData[0].Length; i++)
        {
            float[] vec = multiplyMatrixVectorF(R[i], rawData[0]);
            tcAcc[i,0] = vec[0];
            tcAcc[i,1] = vec[1];
            tcAcc[i,2] = vec[2];
        }



        float[,] gravitySub = new float[3, 3];

        for (int i = 0; i < 3; i++)
        {
            gravitySub[i, 2] = 1f;
        }

        float[,] linAcc = new float[3, 3];
        linAcc = SubtractMatrix(tcAcc, gravitySub);



        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                linAcc[i, j] = linAcc[i, j] * 9.8f;
            }
        }



        float[,] linVel = new float[3, 3];
        for (int i = 1; i < 3; i++)
        {
            linVel[i, 0] = linVel[i - 1, 0] + linAcc[i, 0] * samplePeriod;
            linVel[i, 1] = linVel[i - 1, 1] + linAcc[i, 1] * samplePeriod;
            linVel[i, 2] = linVel[i - 1, 2] + linAcc[i, 2] * samplePeriod;
        }

        Debug.Log("#################");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Debug.LogFormat(string.Format("{0} ", linVel[i, j]));
            }
            Debug.LogFormat(Environment.NewLine + Environment.NewLine);
        }
        Debug.Log("#################");

        float[,] linPos = new float[3, 3];
        for (int i = 1; i < 3; i++)
        {
            linPos[i, 0] = linPos[i - 1, 0] + linVel[i, 0] * samplePeriod;
            linPos[i, 1] = linPos[i - 1, 1] + linVel[i, 1] * samplePeriod;
            linPos[i, 2] = linPos[i - 1, 2] + linVel[i, 2] * samplePeriod;
        }

        //    Debug.Log(">>>>>>>>>>QUATERNIONS X = " + quaternions[1] + " Y = " + quaternions[2] + " Z = " + quaternions[3] + " W" + quaternions[0]);
        // Debug.Log(">>>>>>>>>>ACELEROMETRO X = " + rawData[0] + " Y = " + rawData[1] + " Z = " + rawData[2]);
        //rotMatrix = quaternions2RotateMatrixD(quaternions);
        //inverseRotMatrix = inverseMatrix(rotMatrix);

        //acel = multiplyMatrixVector(inverseRotMatrix, rawData);
        //acel *= 1000;
        // acel /= 100000000;
        // Debug.Log(">>>>>>>>>>ACEL X = " + acel.x);
        // Debug.Log(">>>>>>>>>>ACEL Y = " + acel.y);
        // Debug.Log(">>>>>>>>>>ACEL Z = " + acel.z);

        //v = v0 + acel * Time.realtimeSinceStartup;
        // Debug.Log(">>>>>>>>>>V X = " + v.x);
        // Debug.Log(">>>>>>>>>>V Y = " + v.y);
        // Debug.Log(">>>>>>>>>>V Z = " + v.z);


        //d = d0 + (v + v0)/2 * (Time.realtimeSinceStartup - t0);
        // = Time.realtimeSinceStartup;

        //v0 = v;

        //d0 = d;



       // return d;
    }

    public double[,] quaternions2RotateMatrixD(double[] quaternions){
        double[,] rotationMatrix = new double[MATRIX_ROWS, MATRIX_COLUMNS];

        rotationMatrix[0,0] = 1 - 2 * Math.Pow(quaternions[2], 2) - 2 * Math.Pow(quaternions[3], 2);
        rotationMatrix[0,1] = 2 * quaternions[1] * quaternions[2] - 2 * quaternions[3] * quaternions[0];
        rotationMatrix[0,2] = 2 * quaternions[1] * quaternions[3] + 2 * quaternions[2] * quaternions[1];
        rotationMatrix[1,0] = 2 * quaternions[1] * quaternions[2] + 2 * quaternions[3] * quaternions[0];
        rotationMatrix[1,1] = 1 - 2 * Math.Pow(quaternions[1], 2) - 2 * Math.Pow(quaternions[3], 2);
        rotationMatrix[1,2] = 2 * quaternions[2] * quaternions[3] - 2 * quaternions[1] * quaternions[0];
        rotationMatrix[2,0] = 2 * quaternions[1] * quaternions[3] - 2 * quaternions[2] * quaternions[0];
        rotationMatrix[2,1] = 2 * quaternions[2] * quaternions[3] + 2 * quaternions[1] * quaternions[0];
        rotationMatrix[2,2] = 1 - 2 * Math.Pow(quaternions[1], 2) - 2 * Math.Pow(quaternions[2], 2);

        return rotationMatrix;

    }
    public float[,] quaternions2RotateMatrixF(float[] quaternions){
        float[,] rotationMatrix = new float[3, 3];

        rotationMatrix[0,0] = 1 - 2 * (float)Math.Pow(quaternions[2], 2) - 2 * (float)Math.Pow(quaternions[3], 2);
        rotationMatrix[0,1] = 2 * quaternions[1] * quaternions[2] - 2 * quaternions[3] * quaternions[0];
        rotationMatrix[0,2] = 2 * quaternions[1] * quaternions[3] + 2 * quaternions[2] * quaternions[1];
        rotationMatrix[1,0] = 2 * quaternions[1] * quaternions[2] + 2 * quaternions[3] * quaternions[0];
        rotationMatrix[1,1] = 1 - 2 * (float)Math.Pow(quaternions[1], 2) - 2 * (float)Math.Pow(quaternions[3], 2);
        rotationMatrix[1,2] = 2 * quaternions[2] * quaternions[3] - 2 * quaternions[1] * quaternions[0];
        rotationMatrix[2,0] = 2 * quaternions[1] * quaternions[3] - 2 * quaternions[2] * quaternions[0];
        rotationMatrix[2,1] = 2 * quaternions[2] * quaternions[3] + 2 * quaternions[1] * quaternions[0];
        rotationMatrix[2,2] = 1 - 2 * (float)Math.Pow(quaternions[1], 2) - 2 * (float)Math.Pow(quaternions[2], 2);

        return rotationMatrix;
    }

    public float[,] Transpose(float[,] matrix)
    {
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        float[,] result = new float[h, w];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    public float[,] SubtractMatrix(float[,] matrix1, float[,] matrix2)
    {
        float[,] matrixResult = new float[3,3];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matrixResult[i, j] = matrix1[i, j] - matrix2[i, j];
            }
        }
        return matrixResult;
    }


    public double[,] inverseMatrix(double[,] rotationMatrix){

        double[,] invertMatrix = new double[MATRIX_ROWS, MATRIX_COLUMNS];

        invertMatrix[0,0] = 1;
        invertMatrix[1,1] = 1;
        invertMatrix[2,2] = 1;

        double f = rotationMatrix[1,0] / rotationMatrix[0,0];
        rotationMatrix[1, 0] = rotationMatrix[1, 0] - rotationMatrix[0, 0] * f;
        rotationMatrix[1, 1] = rotationMatrix[1, 1] - rotationMatrix[0, 1] * f;
        rotationMatrix[1, 2] = rotationMatrix[1, 2] - rotationMatrix[0, 2] * f;

        invertMatrix[1,0] = invertMatrix[1, 0] - invertMatrix[0, 0] * f;

        double e = rotationMatrix[2, 0] / rotationMatrix[0, 0];
        rotationMatrix[2, 0] = rotationMatrix[2, 0] - rotationMatrix[0, 0] * e;
        rotationMatrix[2, 1] = rotationMatrix[2, 1] - rotationMatrix[0, 1] * e;
        rotationMatrix[2, 2] = rotationMatrix[2, 2] - rotationMatrix[0, 2] * e;

        invertMatrix[2, 0] = invertMatrix[2, 0] - invertMatrix[0, 0] * e;

        double k = rotationMatrix[2, 1] / rotationMatrix[1, 1];
        rotationMatrix[2, 1] = rotationMatrix[2, 1] - rotationMatrix[1, 1] * k;
        rotationMatrix[2, 2] = rotationMatrix[2, 2] - rotationMatrix[1, 2] * k;

        invertMatrix[2, 0] = invertMatrix[2, 0] - invertMatrix[1, 0] * k;
        invertMatrix[2, 1] = invertMatrix[2, 1] - invertMatrix[1, 1] * k;

        double n = rotationMatrix[1, 2] / rotationMatrix[2, 2];
        rotationMatrix[1, 2] = rotationMatrix[1, 2] - rotationMatrix[2, 2] * n;

        invertMatrix[1, 0] = invertMatrix[1, 0] - invertMatrix[2, 0] * n;
        invertMatrix[1, 1] = invertMatrix[1, 1] - invertMatrix[2, 1] * n;
        invertMatrix[1, 2] = invertMatrix[1, 2] - invertMatrix[2, 2] * n;

        double o = rotationMatrix[0, 2] / rotationMatrix[2, 2];
        rotationMatrix[0, 2] = rotationMatrix[0, 2] - rotationMatrix[2, 2] * o;

        invertMatrix[0, 0] = invertMatrix[0, 0] - invertMatrix[2, 0] * o;
        invertMatrix[0, 1] = invertMatrix[0, 1] - invertMatrix[2, 1] * o;
        invertMatrix[0, 2] = invertMatrix[0, 2] - invertMatrix[2, 2] * o;

        double m = rotationMatrix[0, 1] / rotationMatrix[1, 1];
        rotationMatrix[0, 1] = rotationMatrix[0, 1] - rotationMatrix[1, 1] * m;
        
        invertMatrix[0, 0] = invertMatrix[0, 0] - invertMatrix[1, 0] * m;
        invertMatrix[0, 1] = invertMatrix[0, 1] - invertMatrix[1, 1] * m;
        invertMatrix[0, 2] = invertMatrix[0, 2] - invertMatrix[1, 2] * m;

        invertMatrix[0, 0] /= rotationMatrix[0, 0];
        invertMatrix[0, 1] /= rotationMatrix[0, 0];
        invertMatrix[0, 2] /= rotationMatrix[0, 0];

        invertMatrix[1, 0] /= rotationMatrix[1, 1];
        invertMatrix[1, 1] /= rotationMatrix[1, 1];
        invertMatrix[1, 2] /= rotationMatrix[1, 1];

        invertMatrix[2, 0] /= rotationMatrix[2, 2];
        invertMatrix[2, 1] /= rotationMatrix[2, 2];
        invertMatrix[2, 2] /= rotationMatrix[2, 2];

        rotationMatrix[0, 0] = 1;
        rotationMatrix[1, 1] = 1;
        rotationMatrix[2, 2] = 1;


        return invertMatrix;
    }

    public Vector3 multiplyMatrixVector(double[,] matrix , double[] vector)
    {
        Vector3 aux = new Vector3();
        for (int i = 0; i < 3; ++i)
        {
            aux[i] = 0;
            for (int j = 0; j < 3; ++j)
            {
                aux[i] += (float)(vector[j] * matrix[j, i]);
            }
        }
        return aux;
    }
    public float[] multiplyMatrixVectorF(float[,] matrix , double[] vector)
    {
        float[] aux = new float[3];
        for (int i = 0; i < 3; ++i)
        {
            aux[i] = 0;
            for (int j = 0; j < 3; ++j)
            {
                aux[i] += ((float)vector[j] * matrix[j, i]);
            }
        }
        return aux;
    }
    
    public void resetCube()
    {
        transform.rotation =  inicialPosition;
        // transform.SetPositionAndRotation(new Vector3(0, 0, 1), inicialPosition);
        linearAccel = Vector3.zero;
        d = Vector3.zero;
        v = Vector3.zero;
        v0 = Vector3.zero;
        d0 = Vector3.zero;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerACELERO_BSN_MOB : MonoBehaviour
{

    float tempTime;
    private float sendRate = 0.05f;
    List<GameObject> lineList = new List<GameObject>();
    private DD_DataDiagram m_DataDiagram;

    //private RectTransform DDrect;

    private bool m_IsContinueInput = false;
    private float m_Input = 0f;
    private float h = 0;
    private double[] acelerometer = new double[3];
    private double[] quaternions = new double[4];

    private float[] temp = new float[3];

    public int count_default = 10;

    public int count = 10;



    //Acelerometro
    private Vector3 linearaccel;
    private Vector3 v0;

    private Vector3 v;
    private Vector3 d;
    private Vector3 d0;
    double[,] rotMatrix = new double[3, 3];
    double[,] inverseRotMatrix = new double[3, 3];
    void AddALine()
    {

        if (null == m_DataDiagram)
            return;

        Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
        GameObject line = m_DataDiagram.AddLine(color.ToString(), color);

        if (null != line)
            lineList.Add(line);
    }


    void AddAcelerometer()
    {

        if (null == m_DataDiagram)
            return;

        Color color = Color.blue;
        GameObject line = m_DataDiagram.AddLine("X_BSN", color);
        lineList.Add(line);

        Color color2 = Color.red;
        GameObject line2 = m_DataDiagram.AddLine("Y_BSN", color2);
        lineList.Add(line2);

        Color color3 = Color.yellow;
        GameObject line3 = m_DataDiagram.AddLine("Z_BSN", color3);
        lineList.Add(line3);

        Color color4 = Color.cyan;
        GameObject line4 = m_DataDiagram.AddLine("X_MOB", color4);
        lineList.Add(line4);

        Color color5 = Color.gray;
        GameObject line5 = m_DataDiagram.AddLine("Y_MOB", color5);
        lineList.Add(line5);

        Color color6 = Color.green;
        GameObject line6 = m_DataDiagram.AddLine("Z_MOB", color6);
        lineList.Add(line6);
		

    }

    // Use this for initialization
    void Start()
    {

        GameObject dd = GameObject.Find("DataDiagramAcelerometerBSNMOB");
        if (null == dd)
        {
            Debug.LogWarning("can not find a gameobject of DataDiagram");
            return;
        }
        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();
        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };
        AddAcelerometer();
        //acelerometer = BSNHardwareInterface.ReceiveRawDataTest();
        StartCoroutine(waiter());
        d0 = Vector3.zero;
        d = Vector3.zero;
        v0 = Vector3.zero;
        v = Vector3.zero;
        linearaccel = Vector3.zero;
    }

    IEnumerator waiter()
    {
        //Rotate 90 deg
        quaternions = BSNHardwareInterface.ReceiveQuaternions();

        //Wait for 4 seconds
        yield return new WaitForSeconds(2);

        //Rotate 40 deg
        acelerometer = BSNHardwareInterface.ReceiveRawDataTest();

        //Wait for 2 seconds
        yield return new WaitForSeconds(2);

        

    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > sendRate)
        {
            tempTime -= sendRate;
            ContinueInput(m_Input);
        }
    }

    // private void FixedUpdate() {

    //     m_Input += Time.deltaTime;
    //     if(m_Input % 60 == 0) {
    //         ContinueInput(m_Input);
    //     }
    // }

    private void ContinueInput(float f)
    {

        if (null == m_DataDiagram)
            return;

        // if (false == m_IsContinueInput)
        //     return;

        float d = 0f;

        // foreach (GameObject l in lineList) {
        //     m_DataDiagram.InputPoint(l, new Vector2(0.1f,
        //         (Mathf.Sin(f + d) + 1f) * 2f));
        //     d += 1f;
        // }



        // temp[0] += (float)acelerometer[0];
        // temp[1] += (float)acelerometer[1];
        // temp[2] += (float)acelerometer[2];
        // count = count - 1;
        // //Debug.Log("COUNT +++++++++++++++  " + count);


        // if(count == 0){
        //    // Debug.Log("ENTROU NA MEDIA");
        //     for (int i = 0; i < 3; i++)
        //     {
        //         temp[i] = temp[i] / count_default;
        //         //Debug.Log("MEDIA >>>>>>> " + temp[i] );

        //     }

        //     for (int i = 0; i < lineList.Count; i++)
        //     {
        //         m_DataDiagram.InputPoint(lineList[i], new Vector2(1f, temp[i]));
        //         d += 1f;
        //     }
        //     count = count_default;
        // }
		
		Debug.Log("X CELLLLL = "+ Input.acceleration.x);
        Debug.Log("Y CELLLLL = " + Input.acceleration.y);
        Debug.Log("Z CELLLLL = " + Input.acceleration.z);
        linearaccel = linAcc();

        // for (int i = 0; i < lineList.Count; i++)
        // for (int i = 0; i < 3; i++)
        // {
        //     m_DataDiagram.InputPoint(lineList[i], new Vector2(1f, (float)linearaccel[i]));
        //     d += 1f;
        // }

        m_DataDiagram.InputPoint(lineList[0], new Vector2(1f,linearaccel.x));
        m_DataDiagram.InputPoint(lineList[1], new Vector2(1f,linearaccel.y));
        m_DataDiagram.InputPoint(lineList[2], new Vector2(1f,linearaccel.z));
        m_DataDiagram.InputPoint(lineList[3], new Vector2(1f, Input.acceleration.x));
        m_DataDiagram.InputPoint(lineList[4], new Vector2(1f, Input.acceleration.y));
        m_DataDiagram.InputPoint(lineList[5], new Vector2(1f, Input.acceleration.z));
        //transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);


        // foreach (GameObject l in lineList)
        // {
        //     m_DataDiagram.InputPoint(l, new Vector2(0.1f,
        //         (Mathf.Sin(f + d) + 1f) * 2f));
        //     d += 1f;
        // }

    }

    public void onButton()
    {

        if (null == m_DataDiagram)
            return;

        foreach (GameObject l in lineList)
        {
            m_DataDiagram.InputPoint(l, new Vector2(1, UnityEngine.Random.value * 4f));
        }
    }

    public Vector3 linAcc()
    {
        Vector3 acel = new Vector3();

        rotMatrix = quaternions2RotateMatrix(quaternions);
        inverseRotMatrix = inverseMatrix(rotMatrix);

        acel = multiplyMatrixVector(inverseRotMatrix, acelerometer);
        acel *= 1000;
        Debug.Log(">>>>>>>>>>ACEL X = " + acel.x);
        Debug.Log(">>>>>>>>>>ACEL Y = " + acel.y);
        Debug.Log(">>>>>>>>>>ACEL Z = " + acel.z);

        v = v0 + acel * Time.realtimeSinceStartup;
        Debug.Log(">>>>>>>>>>V X = " + v.x);
        Debug.Log(">>>>>>>>>>V Y = " + v.y);
        Debug.Log(">>>>>>>>>>V Z = " + v.z);


        d = d0 + (v + v0) / 2 * Time.realtimeSinceStartup;

        v0 = v;
        d0 = d;

        Debug.Log(">>>>>>>>>>D X = " + d.x);
        Debug.Log(">>>>>>>>>>D Y = " + d.y);
        Debug.Log(">>>>>>>>>>D Z = " + d.z);


        return d;
    }

    public double[,] quaternions2RotateMatrix(double[] quaternions)
    {
        double[,] rotationMatrix = new double[3, 3];

        rotationMatrix[0, 0] = 1 - 2 * Math.Pow(quaternions[2], 2) - 2 * Math.Pow(quaternions[3], 2);
        rotationMatrix[0, 1] = 2 * quaternions[1] * quaternions[2] - 2 * quaternions[3] * quaternions[0];
        rotationMatrix[0, 2] = 2 * quaternions[1] * quaternions[3] + 2 * quaternions[2] * quaternions[1];
        rotationMatrix[1, 0] = 2 * quaternions[1] * quaternions[2] + 2 * quaternions[3] * quaternions[0];
        rotationMatrix[1, 1] = 1 - 2 * Math.Pow(quaternions[1], 2) - 2 * Math.Pow(quaternions[3], 2);
        rotationMatrix[1, 2] = 2 * quaternions[2] * quaternions[3] - 2 * quaternions[1] * quaternions[0];
        rotationMatrix[2, 0] = 2 * quaternions[1] * quaternions[3] - 2 * quaternions[2] * quaternions[0];
        rotationMatrix[2, 1] = 2 * quaternions[2] * quaternions[3] + 2 * quaternions[1] * quaternions[0];
        rotationMatrix[2, 2] = 1 - 2 * Math.Pow(quaternions[1], 2) - 2 * Math.Pow(quaternions[2], 2);

        return rotationMatrix;

    }


    public double[,] inverseMatrix(double[,] rotationMatrix)
    {

        double[,] invertMatrix = new double[3, 3];

        invertMatrix[0, 0] = 1;
        invertMatrix[1, 1] = 1;
        invertMatrix[2, 2] = 1;

        double f = rotationMatrix[1, 0] / rotationMatrix[0, 0];
        rotationMatrix[1, 0] = rotationMatrix[1, 0] - rotationMatrix[0, 0] * f;
        rotationMatrix[1, 1] = rotationMatrix[1, 1] - rotationMatrix[0, 1] * f;
        rotationMatrix[1, 2] = rotationMatrix[1, 2] - rotationMatrix[0, 2] * f;

        invertMatrix[1, 0] = invertMatrix[1, 0] - invertMatrix[0, 0] * f;

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
    public Vector3 multiplyMatrixVector(double[,] matrix, double[] vector)
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

    public void OnAddLine()
    {
        AddALine();
    }

    public void OnRectChange()
    {

        if (null == m_DataDiagram)
            return;

        Rect rect = new Rect(UnityEngine.Random.value * Screen.width, UnityEngine.Random.value * Screen.height,
            UnityEngine.Random.value * Screen.width / 2, UnityEngine.Random.value * Screen.height / 2);

        m_DataDiagram.rect = rect;
    }

    public void OnContinueInput()
    {

        m_IsContinueInput = !m_IsContinueInput;

    }

}

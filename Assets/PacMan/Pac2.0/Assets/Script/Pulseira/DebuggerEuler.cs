using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuggerEuler : MonoBehaviour
{

    float tempTime;
    public float sendRate = 0.05f;
    List<GameObject> lineList = new List<GameObject>();
    private DD_DataDiagram m_DataDiagram;
    public Text rollText;
    public Text pitchText;
    public Text yawText;

    //private RectTransform DDrect;

    private bool m_IsContinueInput = false;
    private float m_Input = 0f;
    private float h = 0;

    private double[] euler = new double[3];

    void AddALine()
    {

        if (null == m_DataDiagram)
            return;

        Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
        GameObject line = m_DataDiagram.AddLine(color.ToString(), color);

        if (null != line)
            lineList.Add(line);
    }


    void AddEuler()
    {

        if (null == m_DataDiagram)
            return;

        Color color1 = Color.blue;
        GameObject line1 = m_DataDiagram.AddLine("X", color1);
        lineList.Add(line1);

        Color color2 = Color.red;
        GameObject line2 = m_DataDiagram.AddLine("Y", color2);
        lineList.Add(line2);

        Color color3 = Color.yellow;
        GameObject line3 = m_DataDiagram.AddLine("Z", color3);
        lineList.Add(line3);

    }

    // Use this for initialization
    void Start()
    {

        GameObject dd = GameObject.Find("DataDiagramEuler");
        if (null == dd)
        {
            Debug.LogWarning("can not find a gameobject of DataDiagram");
            return;
        }
        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();
        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };
        AddEuler();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        //Wait for 3 seconds
        yield return new WaitForSeconds(1);
        euler = BSNHardwareInterface.ReceiveEuler();
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

        for (int i = 0; i < lineList.Count; i++)
        {
            m_DataDiagram.InputPoint(lineList[i], new Vector2(1f, (float)euler[i]));
            d += 1f;
        }

        rollText.text = ((float)euler[0]).ToString();
        pitchText.text = ((float)euler[1]).ToString();
        yawText.text = ((float)euler[2]).ToString();

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    [SerializeField] public LineRenderer lineRender;

    [SerializeField] public List<Transform> posList;

    const int countBetween2Point = 20;

    Vector3[] curvePoints;

    void Start()
    {
        // 为当前物体添加  LineRenderer 组件。
        // lineRender = gameObject.AddComponent<LineRenderer>();
        // 设置材料的属性
        //lineRender.material = new Material(Shader.Find("Particles/Additive"));
        // 设置线段的起点颜色和终点颜色
        // lineRender.startColor = Color.blue;
        // lineRender.endColor = Color.red;
        // 设置线段起点宽度和终点宽度
        // lineRender.startWidth = 0.3f;
        // lineRender.endWidth = 0.3f;

        curvePoints = new Vector3[countBetween2Point * (posList.Count - 1)];

        lineRender.positionCount = curvePoints.Length;
    }

    void Update()
    {
        CalculateCurve();
        lineRender.SetPositions(curvePoints);
    }

    Vector3 firstPos, curPos, nextPos, lastPos;
    void CalculateCurve()
    {
        //依次计算相邻两点间曲线
        //由四个点确定一条曲线（当前相邻两点p1,p2，以及前后各一点p0,p3）
        for (int i = 0; i < posList.Count - 1; i++)
        {
            //特殊位置增加虚拟点
            //如果p1点是第一个点，不存在p0点，由p1,p2确定一条直线，在向量(p2p1)方向确定虚拟点p0
            if (i == 0)
                firstPos = posList[i].position * 2 - posList[i + 1].position;
            else
                firstPos = posList[i - 1].position;
            //中间点
            curPos = posList[i].position;
            nextPos = posList[i + 1].position;
            //特殊位置增加虚拟点，同上
            if (i == posList.Count - 2)
                lastPos = posList[i + 1].position * 2 - posList[i].position;
            else
                lastPos = posList[i + 2].position;

            CatmulRom(firstPos, curPos, nextPos, lastPos, ref curvePoints, countBetween2Point * i);
        }
        //加入最后一个点位
        // curvePoints[curvePoints.Length - 1] = posList[posList.Count - 1].position;
    }

    //平滑过渡两点间曲线（p1,p2为端点，p0,p3是控制点）
    void CatmulRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, ref Vector3[] points, int startIndex)
    {
        //计算Catmull-Rom样条曲线
        float t0 = 0;
        float t1 = GetT(t0, p0, p1);
        float t2 = GetT(t1, p1, p2);
        float t3 = GetT(t2, p2, p3);

        float t;
        for (int i = 0; i < countBetween2Point; i++)
        {
            t = t1 + (t2 - t1) / countBetween2Point * i;

            Vector2 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
            Vector2 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
            Vector2 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

            Vector2 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
            Vector2 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

            Vector2 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

            points[startIndex + i] = C;
        }
    }

    float GetT(float t, Vector2 p0, Vector2 p1)
    {
        return t + Mathf.Pow(Mathf.Pow((p1.x - p0.x), 2) + Mathf.Pow((p1.y - p0.y), 2), 0.5f);
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    NavMeshAgent navAgent;
    NavMeshPath path;
    List<Vector3> corners = new List<Vector3>();
    [SerializeField] LineRenderer movementLine = null;
    bool isWalking = false;

    [SerializeField] Text pathLengthText = null;
    int costAP = 0;
    int distancesPerAP = 0;

    float predictionTime = 0.1f;
    float currentTime = 0f;

    public void SetMovingCost(int ap)
    {
        distancesPerAP = ap;
    }

    public bool IsMoving()
    {
        return isWalking;
    }

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    private void Start()
    {

        //float width = lineRenderer.startWidth;
        //lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
    }

    void Update()
    {

        if(isWalking && !navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)  //到達目的地
        {
            isWalking = false;
            GetComponent<Character>().ActionEnd();
            CleanPathLine();
        }
        
    }

    public int ToMove()
    {
        if (corners.Count <= 0) return 0;

        navAgent.SetDestination(corners[corners.Count - 1]);
        isWalking = true;
        return costAP;
    }

    public void CleanPathLine()
    {
        movementLine.positionCount = 0;
    }

    public int PredictedPath(RaycastHit hit,int currentAP)
    {
        currentTime += Time.deltaTime;
        if (currentTime < predictionTime) return costAP;
        else currentTime = 0f;


        if (isWalking) return 0;  //如果正在走路就停止預測路線

        corners.Clear();

        NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path);
        if (path.corners.Length <= 0)
        {
            CleanPathLine();
            costAP = 0;
            return 0;  //如果無法產生路線就停止
        }

        float pathLength = 0f;

        for (int i = 0; i < path.corners.Length; i++)        
        {
            
            corners.Add(path.corners[i]);

            if (i < 1) continue; //第一個corner還不用計算

            float newLength = pathLength + Vector3.Distance(path.corners[i - 1], path.corners[i]);  //前一個corner與現在的corner距離

            if (newLength > currentAP * distancesPerAP) //預測路線長度大於最高可走長度
            {
                
                corners.RemoveAt(i);
                //costAP--; //回到上一次計算的AP


                float lastLength = currentAP * distancesPerAP - pathLength;  //剩下可以走的距離
                float percent = lastLength / Vector3.Distance(path.corners[i - 1], path.corners[i]);  //剩下可以走的步數/原本應該要走的步數
                Vector3 lastVector = path.corners[i] - path.corners[i - 1];  //最後一段的方向(向量)

                Vector3 lastPos = path.corners[i - 1] + lastVector * percent;
                corners.Add(lastPos);  //將新的終點加到路徑當中

                pathLength = pathLength + lastLength;

                break;
            }
            else  //沒有超過就可以算進pathLength
            {
                pathLength = newLength;
            }

        }

        costAP = Mathf.CeilToInt(pathLength / 3f);


        //顯示路徑線段
        movementLine.positionCount = corners.Count;
        movementLine.SetPositions(corners.ToArray());
        //顯示路徑總長度
        pathLengthText.text = "距離: " + pathLength.ToString("F1");

        return costAP;
    }
}

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

        if(isWalking && !navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)  //��F�ت��a
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


        if (isWalking) return 0;  //�p�G���b�����N����w�����u

        corners.Clear();

        NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path);
        if (path.corners.Length <= 0)
        {
            CleanPathLine();
            costAP = 0;
            return 0;  //�p�G�L�k���͸��u�N����
        }

        float pathLength = 0f;

        for (int i = 0; i < path.corners.Length; i++)        
        {
            
            corners.Add(path.corners[i]);

            if (i < 1) continue; //�Ĥ@��corner�٤��έp��

            float newLength = pathLength + Vector3.Distance(path.corners[i - 1], path.corners[i]);  //�e�@��corner�P�{�b��corner�Z��

            if (newLength > currentAP * distancesPerAP) //�w�����u���פj��̰��i������
            {
                
                corners.RemoveAt(i);
                //costAP--; //�^��W�@���p�⪺AP


                float lastLength = currentAP * distancesPerAP - pathLength;  //�ѤU�i�H�����Z��
                float percent = lastLength / Vector3.Distance(path.corners[i - 1], path.corners[i]);  //�ѤU�i�H�����B��/�쥻���ӭn�����B��
                Vector3 lastVector = path.corners[i] - path.corners[i - 1];  //�̫�@�q����V(�V�q)

                Vector3 lastPos = path.corners[i - 1] + lastVector * percent;
                corners.Add(lastPos);  //�N�s�����I�[����|��

                pathLength = pathLength + lastLength;

                break;
            }
            else  //�S���W�L�N�i�H��ipathLength
            {
                pathLength = newLength;
            }

        }

        costAP = Mathf.CeilToInt(pathLength / 3f);


        //��ܸ��|�u�q
        movementLine.positionCount = corners.Count;
        movementLine.SetPositions(corners.ToArray());
        //��ܸ��|�`����
        pathLengthText.text = "�Z��: " + pathLength.ToString("F1");

        return costAP;
    }
}

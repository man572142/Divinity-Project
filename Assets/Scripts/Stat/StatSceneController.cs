using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatSceneController : MonoBehaviour
{
    [SerializeField] RectTransform canvasMover = null;
    [SerializeField] Transform myCamera = null;
    [SerializeField] Vector3 camFirstPos = new Vector3(0f, 1.5f, -10f);
    [SerializeField] Vector3 camSecondPos = new Vector3(12f, 1.5f, -10f);
    [SerializeField] Vector2 canvasFirstPos = new Vector2(0f, 0f);
    [SerializeField] Vector2 canvasSecondPos = new Vector2(-1200f, 0f);
    private bool isMoving = false;
    bool isPlayerOne = true;
    float t = 0f;
    [SerializeField] StatChecker statOne = null;
    [SerializeField] StatChecker statTwo = null;

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (isMoving) MoveCameraAndCanvas();


    }


    private void MoveCameraAndCanvas()
    {
        t += Time.deltaTime * 2f;
        if (isPlayerOne)
        {
            canvasMover.anchoredPosition = Vector2.Lerp(canvasSecondPos, canvasFirstPos, t);
            myCamera.position = Vector3.Lerp(camSecondPos, camFirstPos, t);
        }
        else
        {
            canvasMover.anchoredPosition = Vector2.Lerp(canvasFirstPos, canvasSecondPos, t);
            myCamera.position = Vector3.Lerp(camFirstPos, camSecondPos, t);
        }

        if (t >= 1)
        {
            t = 0;
            isMoving = false;
        }
    }

    #region ±q«ö¶s©I¥s
    public void PlayerTwo()
    {
        if (!statOne.StatCheck()) return;

        isPlayerOne = false;     
        isMoving = true;
    }

    public void PlayerOne()
    {
        if (!statTwo.StatCheck()) return;  

        isPlayerOne = true;
        isMoving = true;
    }

    public void StartGame()
    {
        if (!statOne.StatCheck() || !statTwo.StatCheck()) return;

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }

    #endregion
}

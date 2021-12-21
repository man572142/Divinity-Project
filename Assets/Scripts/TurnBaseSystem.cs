using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TurnBaseSystem : MonoBehaviour
{
    int currentTrun = 0;
    List<Character> characters = new List<Character>();

    [SerializeField] FollowCamera followCamera = null;
    [SerializeField] float nextTurnDelay = 1f;
    

    void Start()
    {

        for(int i = 0; i < transform.childCount; i++)  //遊戲開始時先將所有角色的charater加入清單並先關掉
        {
            Character charater = transform.GetChild(i).GetComponent<Character>();
            if (charater == null) continue;

            characters.Add(charater);
            characters[i].InitializeCharacter();
            characters[i].ActiveCharater(false);
        }

        characters[0].ActiveCharater(true);  //開啟第一個行動的角色
        followCamera.SetFollow(characters[0].transform);
    }

    IEnumerator NextTurn()
    {
        characters[currentTrun].ActiveCharater(false);  //先把這一輪的角色關掉
        yield return new WaitForSeconds(nextTurnDelay);
        

        currentTrun += 1; // +1回合
        if (currentTrun > transform.childCount - 1)  currentTrun = 0;  //如果+1後超出角色數量,代表已輪過一輪,需回到第一位
        characters[currentTrun].ActiveCharater(true);  //角色開啟

        followCamera.SetFollow(characters[currentTrun].transform);
    }

    public Transform GetCurrentPlayer()
    {
        return characters[currentTrun].transform;
    }



    //public void ToMoveMode()
    //{
    //    if (!canMove) return;

    //    characters[currentTrun].GetComponent<Movement>().SetMoveMode(true);
    //    moveButton.color = Color.white;
    //    characters[currentTrun].GetComponent<Attack>().SetAttackMode(false);
    //    attackButton.color = Color.gray;
    //}

    //public void ToAttackMode()
    //{
    //    if (!canAttack) return;

    //    characters[currentTrun].GetComponent<Attack>().SetAttackMode(true);
    //    attackButton.color = Color.white;
    //    characters[currentTrun].GetComponent<Movement>().SetMoveMode(false);
    //    moveButton.color = Color.gray;
    //}

    public void TurnFinished()
    {
        
        StartCoroutine(NextTurn());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetMouseButton(1))
        {
            UseSkill(0);
        }

    }

    private void UseSkill(int index)
    {
        Skill characterSkill = characters[currentTrun].GetComponent<Skill>();
        if (characterSkill != null)
        {
            characterSkill.SetSkillIndex(index);
        }
    }

    public void UseSkillOne()
    {
        UseSkill(1);
    }

    public void UseSkillTwo()
    {
        UseSkill(2);

    }

    public void UseSkillThree()
    {
        UseSkill(3);
    }
}

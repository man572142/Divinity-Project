using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region SetSingleton
    static GameManager m_instance = null;
    static public GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameManager();
                Debug.Log("create GameManager");
            }
            return m_instance;
        }
    }

    #endregion

    [System.Serializable]
    public struct CharacterStat
    {
        public Occupation occupation;
        public float health;
        public float ability;
    }

    /// <summary>0~3¬OPlayer1,4~7¬OPlayer2 </summary>
    public CharacterStat[] characterStats = new CharacterStat[8];
}

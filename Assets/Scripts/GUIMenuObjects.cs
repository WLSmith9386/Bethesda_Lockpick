using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIMenuObjects : MonoBehaviour {

    public GameControl GameController;
    public AnimationControls animController;
    public Text m_lockType;
    
    public void StartNovice()
    {
        GameController.m_selectedDifficulty = GameControl.difficulty.novice;
        m_lockType.text = "Novice Lock";
        ResetLock();
    }

    public void StartAdvance()
    {
        GameController.m_selectedDifficulty = GameControl.difficulty.advance;
        m_lockType.text = "Advanced Lock";
        ResetLock();
    }

    public void StartElite()
    {
        GameController.m_selectedDifficulty = GameControl.difficulty.elite;
        m_lockType.text = "Elite Lock";
        ResetLock();
    }

    public void StartMaster()
    {
        GameController.m_selectedDifficulty = GameControl.difficulty.master;
        m_lockType.text = "Master Lock";
        ResetLock();
    }

    void ResetLock()
    {
        animController.PlayAnimation("FadeIn");
        GameController.Reset();
    }
}

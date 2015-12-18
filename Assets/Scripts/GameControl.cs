using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public AudioClip scratching;
    public AudioClip click;
    public AudioControl audioController;
    //public Vector3 m_baseRot = new Vector3(0, 180, 0);
    public enum difficulty//difficulties
    {
        master,
        elite,
        advance,
        novice
    }
    public difficulty m_selectedDifficulty;

    //public variables
    public Transform m_PickObject;//pick object
    public Transform m_lockObject;//lock object
    public float m_rotSpeed = 1.0f;//how fast the lock/pick will rotate

    float percentage = 0.0f;//how much the lock can move
    //pick movement limites, default is 0
    private float m_minEularAngle = -90;//the furthest left the pick can rotate
    private float m_maxEularAngle = 90;//furthese rigth the pick can rotate
    private float m_lockEularZ = 0.0f;//float value for the z axis of the lock
    private float m_pickEularZ = 0.0f;//float value for the z axis of the pick
    private bool m_lockMoving = false;//boolean to stop pick moving

    float m_SweetSpot;//sweet spot of the lock
    public float m_masterRange = 20;//range for master locks
    public float m_eliteRange = 30;//range for elite locks
    public float m_advanceRange = 40;
    public float m_noviceRange = 50;
    Vector2 m_range = Vector2.zero;//x is least, y is max
    

    void GetSweetSpot()
    {
        m_SweetSpot = Random.Range(-90, 90);//a random spot in the range rotation range
    }

    void SetArea()
    {
        GetSweetSpot();
        float p_range = GetRange(m_selectedDifficulty);
        m_range.x = m_SweetSpot - p_range;
        m_range.y = m_SweetSpot + p_range;
    }

    float GetRange(difficulty range)
    {
        switch (range)
        {
            case difficulty.master:
                return m_masterRange;
            case difficulty.elite:
                return m_eliteRange;
            case difficulty.advance:
                return m_advanceRange;
            case difficulty.novice:
                return m_noviceRange;
        }

        //unhandled exception
        return 0;
    }

    float GetDistance()
    {
        float p_distance = Mathf.Abs(m_pickEularZ - m_SweetSpot);
        return p_distance;
    }

    void LockControl()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))//move towards goal when key is down
        {
            m_lockMoving = true;//lock the pick
            m_lockEularZ -= m_rotSpeed;//adjust the lock rotation
        }
        else//no key input
        {
            m_lockMoving = false;//allow pick movement
            m_lockEularZ += m_rotSpeed * 2;//move back to the base position
        }

        m_lockEularZ = Mathf.Clamp(m_lockEularZ, (m_minEularAngle * percentage), 0.0f);
        m_lockObject.localEulerAngles = new Vector3(0, 0, m_lockEularZ);

        if (Mathf.Abs(m_lockEularZ) == Mathf.Abs(m_minEularAngle))
        {
            WinConditionMet();
        }
    }

    IEnumerator WinConditionMet()
    {
        audioController.StopPlayingSound();
        audioController.PlaySound(click);
        yield return new WaitForSeconds(click.length);
        Reset();
    }

    void PickControll()
    {
        if (!m_lockMoving)//don't move the pick when the lock is moving
        {
            if (Input.GetAxis("Mouse X") != 0)//move the pick when the mouse moves
            {
                audioController.PlaySoundIfNotPlaying(scratching);
                float p_mouseSpeed = Input.GetAxis("Mouse X");//get the mouse speed

                m_pickEularZ -= m_rotSpeed * p_mouseSpeed;//adjust the eulars of the pick
                m_pickEularZ = Mathf.Clamp(m_pickEularZ, m_minEularAngle, m_maxEularAngle);//limit ththe z rotataion
                m_PickObject.transform.localEulerAngles = new Vector3(0, 0, m_pickEularZ);//set the new eulara
            }
            else
            {
                audioController.StopPlayingSound();
            }
        }
    }

    void PercentageCalculation()
    {
        //percentage needs to be 0 when completely out of range and 1 when in the sweet spot
        // use the distance from the sweet spot to get the percentage
        percentage = (1 - (GetDistance() / GetRange(m_selectedDifficulty)));//subtract by one to get right result
        //instead of using the pinpoint of a sweet spot we want to give it a range, so we add to the percentage
        percentage += (GetRange(m_selectedDifficulty) / 100);
        //clamp the percentage to 0-1 to avoid going over 1 and below 0
        percentage = Mathf.Clamp(percentage, 0, 1);
        //print(m_SweetSpot + " " + GetDistance() + " / " + GetRange(m_selectedDifficulty) + " = " + percentage);
    }

    public void Reset()
    {
        m_pickEularZ = 0.0f;
        m_lockEularZ = 0.0f;
        m_PickObject.localEulerAngles = Vector3.zero;
        m_lockObject.localEulerAngles = Vector3.zero;
        SetArea();
    }

    void Start()
    {
        Reset();
        //print("Sweet spot is at " + m_SweetSpot);
    }
    
    void Update()
    {
        PickControll();
        LockControl();
        PercentageCalculation();
    }
}

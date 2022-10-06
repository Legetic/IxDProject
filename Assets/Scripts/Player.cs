using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
    public PlayerContainer playerContainer;
    private Animator _animator;
    public float velocity = 5.0f;
    private float _animationMultiplier = 2.0f;
    public float rotationSpeed = 3.0f;
    public float acceleration = .2f;
    int velocityHash;
    public Transform targetWayPoint;
    [SerializeField] private int maxHealth = 10;

    [SerializeField] private int jumpSpeed = 70;
    public int currentHealth;
    [HideInInspector] public float score;

    [SerializeField] private int pointsForObstacle = 50;
    public PlayerStance stance = PlayerStance.high;

    //Lane Switching
    private float targetLaneTargetOffset = 0;
    private float targetLaneOffset = 0;
    [SerializeField] private float laneSwitchTime = 1f;
    private float laneSwitchTimeElapse;

    private Vector3 moveToPosition;


    Obstacles lastObstacle = null;

    bool jump;
    bool slide;
    bool kick;
    public bool damage;
    public event Action<PlayerStance> OnStanceChanged;
    private string jumpAnimation;
    private PlayerStance _playerStance = PlayerStance.idle;
    private int _animationMultiplierHash;
    void Awake()
    {
        currentHealth = maxHealth;

    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        _animationMultiplierHash = Animator.StringToHash("AnimationMultiplier");
        _animator.SetBool("Jump", false);
        laneSwitchTimeElapse = laneSwitchTime;
        damage = true;
    }

    void Update()
    {
        _animator.SetFloat(velocityHash, velocity);
        _animator.SetFloat(_animationMultiplierHash, velocity / 4);

        if (targetWayPoint != null)
        {
            moveToWaypoint();
        }
        if (Input.GetKeyUp("d"))
        {
            moveRight();
        }
        if (Input.GetKeyUp("a"))
        {
            moveLeft();
        }
        if (Input.GetKeyUp("i"))
        {
            setStance(PlayerStance.low);
        }
        if (Input.GetKeyUp("o"))
        {
            setStance(PlayerStance.medium);
        }
        if (Input.GetKeyUp("p"))
        {
            setStance(PlayerStance.high);
        }
        score += Time.deltaTime;
        velocity += Time.deltaTime * acceleration;
    }
    void setStance(PlayerStance playerStance)
    {
        this._playerStance = playerStance;
        OnStanceChanged?.Invoke(_playerStance);

    }

    void FixedUpdate()
    {
        if (jump)
        {
            jump = false;
        }

        else if (slide)
        {
            slide = false;
        }

        else if (kick)
        {
            kick = false;
        }
    }

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    private void moveToWaypoint()
    {
        //The old, working version.
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, velocity * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, velocity * Time.deltaTime);

        //2.5 is half of the waypoint length
        //Getting the offset of the component that is forward between waypoint and player. so if z is of forward, what is their z offset.
        // float playerToTargetForwardOffset = Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) - Vector3.Dot(targetWayPoint.forward, transform.position);
        // moveToPosition = targetWayPoint.position + targetWayPoint.forward * (2.5f - playerToTargetForwardOffset);

        // //print("movetoPos: " + moveToPosition);
        // //print("targetWaypoinyPos: " + targetWayPoint.position);

        // //Old smooth curve version
        // //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        // //transform.position = Vector3.MoveTowards(transform.position, moveToPosition, velocity * Time.deltaTime);

        // //This is used to take account for the speed forward becoming slower when switching lanes which we dont want
        // // print("reached lane? : " + hasReachedLane());
        // float laneSwitchSpeedAdjustmentFactor = hasReachedLane() ? 1 : Mathf.Sqrt(2);
        // transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        // transform.position = Vector3.MoveTowards(transform.position, moveToPosition, laneSwitchSpeedAdjustmentFactor * velocity * Time.deltaTime);
        // ////////////////////////////
        /* Vector3 newTargetWaypoint = transform.position;
        Vector3 moveToPositionForwardOffset = Vector3.Scale(targetWayPoint.forward, moveToPosition);
        List<float> vectorArray = new List<float>();
        vectorArray.Add(targetWayPoint.forward.x);
        vectorArray.Add(targetWayPoint.forward.y);
        vectorArray.Add(targetWayPoint.forward.z);
        

        int forwardIndex = vectorArray.IndexOf(1);
        forwardIndex = forwardIndex == -1 ? vectorArray.IndexOf(-1) : 1;
        print("targetWayPoint.forward: " + targetWayPoint.forward + "forward index:" + forwardIndex + " vectorArray: " + vectorArray[0] + "," + vectorArray[1] + "," + vectorArray[2]);
        newTargetWaypoint[forwardIndex] = moveToPositionForwardOffset[forwardIndex];
        //newTargetWaypoint = newTargetWaypoint targetWayPoint.forward * moveToPositionForwardOffset;

        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, newTargetWaypoint, velocity * Time.deltaTime); */

        //offset for lane switch

        /* if (laneSwitchTimeElapse < laneSwitchTime && laneSwitchTime != 0)
{
    targetLaneOffset = Mathf.Lerp(0, targetLaneTargetOffset, laneSwitchTimeElapse / laneSwitchTime);
    laneSwitchTimeElapse += Time.deltaTime;
    //print("HEJJJJJJJJJJJJ");

    print("Offset: " + targetLaneOffset);
    print("LaneSwitchOffset: " + targetLaneOffset);
    //transform.position += transform.right * targetLaneOffset;
}
*/
        /* Vector3 newLaneSwitchTarget = transform.position;
              Vector3 moveToPositionSideOffset = Vector3.Scale(targetWayPoint.right, moveToPosition);
              List<float> vectorArray2 = new List<float>();
              vectorArray2.Add(targetWayPoint.right.x);
              vectorArray2.Add(targetWayPoint.right.y);
              vectorArray2.Add(targetWayPoint.right.z);
              int forwardIndex2 = vectorArray2.IndexOf(1f);
              newLaneSwitchTarget[forwardIndex2] = moveToPositionSideOffset[forwardIndex2]; */


        //transform.position = transform.right * targetLaneOffset;
        //transform.position = Vector3.Lerp(transform.position, transform.right * targetLaneOffset, 0.5f * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, newLaneSwitchTarget, velocity * Time.deltaTime);

    }

    private void startLerpBetweenLanes(float targetOffset)
    {
        targetLaneTargetOffset = targetOffset;
        targetLaneOffset = 0;
        laneSwitchTimeElapse = 0;
    }

    private void moveLeft()
    {
        // if (hasReachedLane() != true) return;
        Transform move = targetWayPoint.GetComponent<Waypoint>().getLeftMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * -5;
            playerContainer.offset += transform.right * 5;
            //transform.position += transform.right * -5;

            // startLerpBetweenLanes(-5);

        }
    }
    private void moveRight()
    {
        // if (hasReachedLane() != true) return;
        Transform move = targetWayPoint.GetComponent<Waypoint>().getRightMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * 5;
            playerContainer.offset += transform.right * -5;
            //transform.position += transform.right * 5;
            // startLerpBetweenLanes(5);
        }
    }
    public bool hasReachedTarget()
    {
        return Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetWayPoint.transform.position.x, 0, targetWayPoint.transform.position.z)) < 0.2;
        // return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

        //print("Target forward offset: " + (Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position)));

        // return Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) >= 0;
    }

    public bool hasReachedLane()
    {
        //return transform.position == targetWayPoint.transform.position;
        //return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

        //print("Target forward offset: " + (Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position)));
        float playerToLaneSideOffset = Vector3.Dot(targetWayPoint.right, targetWayPoint.position) - Vector3.Dot(targetWayPoint.right, transform.position);
        return playerToLaneSideOffset <= 0.1;
    }


    public void takeDamage(Obstacles.BlockadeType blockadeType)
    {
        if ((blockadeType == Obstacles.BlockadeType.High && _playerStance == PlayerStance.high) ||
        (blockadeType == Obstacles.BlockadeType.Full && _playerStance == PlayerStance.medium) ||
        (blockadeType == Obstacles.BlockadeType.Low && _playerStance == PlayerStance.low))
        {
            return;
        }
        else
        {
            currentHealth -= 1;

            if (currentHealth <= 0)
            {
                PlayerPrefs.SetInt("Score", (int)score);
                GameManager.Instance.latestScore = (int)score;
                SceneManager.LoadScene(2);
            }
        }
    }


    public void avoidObstacle(Obstacles obstacle)
    {
        //print(obstacle.blockadeType);

        if (obstacle == lastObstacle) return;
        lastObstacle = obstacle;

        if (obstacle.blockadeType == Obstacles.BlockadeType.High && _playerStance == PlayerStance.high)
        {
            damage = false;
            slide = true;
            _animator.SetBool("Slide", true);
            score += pointsForObstacle;
        }

        else if (obstacle.blockadeType == Obstacles.BlockadeType.Low && _playerStance == PlayerStance.low)
        {
            damage = false;
            jump = true;
            _animator.SetBool("Jump", true);
            score += pointsForObstacle;


        }
        else if (obstacle.blockadeType == Obstacles.BlockadeType.Full && _playerStance == PlayerStance.medium)
        {
            damage = false;
            kick = true;
            _animator.SetBool("Kick", true);
            score += pointsForObstacle;
        }

    }

    public void keepRunning()
    {
        _animator.SetBool("Slide", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Kick", false);

        jump = false;
        slide = false;
        kick = false;

        damage = true;
        //setStance(PlayerStance.idle);

    }
}
public enum PlayerStance
{
    low, medium, high, idle
}

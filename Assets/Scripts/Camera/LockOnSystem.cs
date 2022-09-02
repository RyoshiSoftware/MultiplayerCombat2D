using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class LockOnSystem : MonoBehaviour
{
    Player player;
    
    [Header("Lock On Game Object Component")]
    public GameObject lockOnUI;
    Image lockOnImage;
    public Vector2 offset;
    [SerializeField] private Sprite nearLockOnSprite;
    [SerializeField] private Vector3 nearLockOnScale;
    [SerializeField] private Sprite farLockOnSprite;
    [SerializeField] private Vector3 farLockOnScale;
    

    [Header("Cinemachine Target Group")]
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCameraTargetGroup;
    [SerializeField] private Animator stateDrivenCamAnim;

    float maxOrthographicSize;
    float height;
    float width;

    #region Other Component

    Transform target;
    public bool isLock  {get; private set;} 
    bool lockOffCoroutineRunning = false;

    PlayerInputHandler inputHandler;

    #endregion

    private void Awake() {
        
        InitializeCameraParameter();

        player = GetComponent<Player>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void InitializeCameraParameter()
    {
        height = Settings.cameraHeight;
        width = Settings.cameraHeight * Settings.cameraRatio;

        isLock = false;
    }

    void Start()
    {
        lockOnImage = lockOnUI.GetComponent<Image>();
        lockOnUI.SetActive(false);

        maxOrthographicSize = cinemachineVirtualCameraTargetGroup.m_Lens.OrthographicSize + 2;
    }

    private void OnEnable() {
        inputHandler.onLockOn += LockTarget;
    }

    private void OnDisable() {
        inputHandler.onLockOn -= LockTarget;
    }

    void LockTarget()
    {
        if (!isLock)
        {
            FindTheNearestTarget();
            LockOn();
        }
        else
        {
            if (lockOffCoroutineRunning)
            {
                return;
            }

            StartCoroutine(LockOff());
        }
    }

    void FindTheNearestTarget()
    {
        float closestDistance = Mathf.Infinity;
        
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(transform.position, new Vector2(width, height), 0);
        foreach(Collider2D collider2D in collider2DArray)
        {
            if (collider2D.gameObject == gameObject || !collider2D.CompareTag("Player")) continue;

            Vector3 distanceToTarget = collider2D.transform.position - transform.position;
            float distance = distanceToTarget.sqrMagnitude;
            if (distance < closestDistance)
            {
                target = collider2D.transform;
                closestDistance = distance;
            }
            
        }

        player.target = target;
    }

    void LockOn()
    {
        if (target != null)
        {
            lockOnUI.SetActive(true);
            isLock = true;

            cinemachineTargetGroup.AddMember(target, 1, 0);

            stateDrivenCamAnim.Play("LockOn State");

            EventHandler.CallLockOnAction(target.transform);
        }
    }

    
    void Update()
    {
        FollowingTarget();
        CheckingTargetIsInRange();
    }  

    IEnumerator LockOff()
    {
        lockOffCoroutineRunning = true;

        lockOnUI.SetActive(false);
        
        stateDrivenCamAnim.Play("LockOff State");
        EventHandler.CallLockOffAction();

        yield return new WaitForSeconds(0.5f);
        
        isLock = false;
        cinemachineTargetGroup.RemoveMember(target);
        target = null;

        player.target = target;

        lockOffCoroutineRunning = false;
    }

    void FollowingTarget()
    {
        if (isLock && target != null)
        {
            Vector2 position = Camera.main.WorldToScreenPoint(target.transform.position);
            lockOnUI.transform.position = position + offset;

            if (Vector2.Distance(target.transform.position, transform.position) <= maxOrthographicSize / 1.2)
            {
                lockOnImage.sprite = nearLockOnSprite;
                lockOnUI.transform.localScale = nearLockOnScale;
            }
            else
            {
                lockOnImage.sprite = farLockOnSprite;
                lockOnUI.transform.localScale = farLockOnScale;
            }
        }
    }

    void CheckingTargetIsInRange()
    {
        if (target != null)
        {
            Vector2 distanceVector = target.transform.position - transform.position;
            if (Mathf.Abs(distanceVector.y) > maxOrthographicSize || Mathf.Abs(distanceVector.x) > maxOrthographicSize * Settings.cameraRatio)
            {
                StartCoroutine(LockOff());
            }
        }
    }
    
    

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}

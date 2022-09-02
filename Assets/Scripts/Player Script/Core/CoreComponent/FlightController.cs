using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MEC;

public class FlightController : CoreComponent
{
    [SerializeField] Transform main;

    Movement movement;
    LayerController colController;

    Vector2 groundPosition = new Vector2(0,0);
    
    public bool isFlying;
    public bool isFlyingCoroutine;
    public float flyHeight;
    public Vector2 flyPosition;
    float flyTime;
    Vector2 tempPositionForHover;

    public Action onStartFlyCoroutine;
    public Action onFinishFlyCoroutine;
    
    protected override void Awake()
    {
        base.Awake();
    }

    void Start() 
    {
        movement = core.GetCoreComponent<Movement>();
        colController = core.GetCoreComponent<LayerController>();
    }

    #region Fly

    public void Fly(Vector2 velocity)
    {
        main.Translate(velocity * Time.deltaTime);
    }

    public IEnumerator<float> FlyingUpCoroutine(float flySpeed, float maxFlyHeight)
    {
        onStartFlyCoroutine?.Invoke();

        isFlyingCoroutine = true;
        isFlying = true;

        flyHeight = 0;

        while (flyHeight < maxFlyHeight)
        {
            Fly(Vector2.up * flySpeed);
            CalculateFlyHeight();

            yield return Timing.WaitForOneFrame;
        }

        isFlyingCoroutine = false;

        SetFlyPosition();

        ChangeLayer(LayerData.Air);
        onFinishFlyCoroutine?.Invoke();
    }

    public IEnumerator<float> FlyingDownCoroutine(float flySpeed, float maxFlyHeight)
    {
        onStartFlyCoroutine?.Invoke();

        isFlyingCoroutine = true;

        while (flyHeight > 0)
        {
            Fly(Vector2.down * flySpeed);
            CalculateFlyHeight();

            yield return Timing.WaitForOneFrame;
        }

        SetGroundPosition();

        isFlyingCoroutine = false;
        isFlying = false;

        ChangeLayer(LayerData.Ground);

        onFinishFlyCoroutine?.Invoke();
    }

    public void Hover(float hoverSpeed, float hoverHeight)
    {
        if (isFlyingCoroutine) return;
 
        tempPositionForHover.y = Mathf.Sin((Time.time - flyTime) * hoverSpeed) * hoverHeight;
        tempPositionForHover.x = 0;

        SetHoverPosition(tempPositionForHover);
    }

    public void SetHoverPosition(Vector2 position)
    {
        main.position = flyPosition + position;
    }

    public void SetFlyPosition()
    {
        flyTime = Time.time;

        flyPosition = main.position;
    }

    public void SetBackToFlyPosition()
    {
        main.position = flyPosition;
    }


    public void SetGroundPosition()
    {
        main.localPosition = groundPosition;
    }

    public void CalculateFlyHeight()
    {
        flyHeight = main.position.y - movement.GetPosition().y;
    }

    #endregion

    #region Layer Change

    void ChangeLayer(int index)
    {
        colController.ChangeLayer(index);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

public class Movement : CoreComponent
{
    [SerializeField] Rigidbody2D playerRb;

    LayerController colController;

    protected override void Awake() 
    {
        base.Awake();
    }

    void Start() 
    {
        colController = core.GetCoreComponent<LayerController>();
    }

    #region Move (for playerRb)

    #region Velocity

    public void Move(Vector2 velocity)
    {
        playerRb.MovePosition((Vector2)playerRb.transform.position + velocity * Time.deltaTime);
    } 

    public void SetVelocity(Vector2 velocity)
    {
        StopMoving();
        
        playerRb.velocity = velocity;
    }

    public void StopMoving()
    {
        playerRb.velocity = Vector2.zero;
    }

    public void MultiplyVelocityFactor(float value)
    {
        playerRb.velocity *= value;
    }

    public Vector2 GetVelocity()
    {
        return playerRb.velocity;
    }

    public float GetVelocityMagnitude()
    {
        return playerRb.velocity.magnitude;
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode2D)
    {
        playerRb.AddForce(force, forceMode2D);
    }

    #endregion

    #region Position

    public void SetPosition(Vector2 position)
    {
        playerRb.transform.position = position;
    }

    public void SetHeight(float height)
    {
        playerRb.transform.position = new Vector2(playerRb.transform.position.x, height);
    }

    public Vector2 GetPosition()
    {
        return playerRb.transform.position;
    }

    #endregion

    #endregion

    #region Layer Change

    void ChangeLayer(int index)
    {
        colController.ChangeLayer(index);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{
    [Header("Rotation component")]
    public static Quaternion upRotation = Quaternion.Euler(0, 0, 90);
    public static Quaternion leftRotation = Quaternion.Euler(0, 0, 180);
    public static Quaternion downRotation = Quaternion.Euler(0, 0, 270);
    public static Quaternion rightRotation = Quaternion.Euler(0, 0, 0);


    [Header("Camera component")]
    public static float cameraHeight = Camera.main.orthographicSize * 2.0f;
    public static float cameraRatio = Screen.width * 1.0f / Screen.height;

    [Header("KnockBack type component")]
    public static float minStrongKnockBack = 1800;
    public static float minMediumKnockBack = 500;
}

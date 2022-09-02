using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerData
{
    public static int AllLayer = 0;
    
    static int layerGround = 0;
    public static int Ground
    {
        get 
        {
            if (layerGround == 0)
            {
                layerGround = LayerMask.NameToLayer("Ground");
            }
            return layerGround;
        }
        private set{}
    } 


    static int layerAir = 0;
    public static int Air
    {
        get 
        {
            if (layerAir == 0)
            {
                layerAir = LayerMask.NameToLayer("Air");
            }
            return layerAir;
        }
        private set{}
    } 
}

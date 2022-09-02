using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : CoreComponent
{
    Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
    }

    private void OnEnable() {
        player.onChangeLayer += ChangeLayer;
    }

    private void OnDisable() {
        player.onChangeLayer -= ChangeLayer;
    }

    public void ChangeLayer(int index)
    {
        if (index == LayerData.Ground)
        {
            player.gameObject.layer = LayerData.Ground;
        }
        else
        {
            player.gameObject.layer = LayerData.Air;
        }    
    }
}

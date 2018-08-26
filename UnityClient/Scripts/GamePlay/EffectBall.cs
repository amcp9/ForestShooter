using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class EffectBall : Effect 
{
    public Type type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            //to ins other effect

            //to send request
            if (other.tag == "Me")
            {
                switch(type)
                {
                    case Type.Speed:
                        GameFacade.Instance.SendGainRequest(BaseStatType.SpeedPoint, 20);
                        break;
                    case Type.Health:
                        break;
                }
            }
        }
    }
}

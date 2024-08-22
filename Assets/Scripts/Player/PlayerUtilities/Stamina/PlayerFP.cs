using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Outlander.Player
{
    public class PlayerFP : NetworkBehaviour, IStatus
    {
        public float Fpoint { get => fpoint; set => fpoint = value; }
        public float MaxFpoint { get => maxFpoint; set => maxFpoint = value; }

        [SyncVar, SerializeField] private float fpoint;
        private float maxFpoint = 100;

        void RechargeFP()
        {
            fpoint = maxFpoint;
        }


    }
}


﻿using ORST.Foundation.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ORST.Core.Movement {
    public class TeleportPointORST : BaseMonoBehaviour {
        [SerializeField, Required] private Transform m_DestinationTransform;

        public Transform DestinationTransform => m_DestinationTransform;

        private void OnEnable() {
            TeleportPointManager.Register(this);
        }

        private void OnDisable() {
            TeleportPointManager.Unregister(this);
        }
    }
}
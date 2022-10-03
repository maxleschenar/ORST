using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class TeleportInputHandlerHands : TeleportInputHandlerHMD
{
    public Transform LeftHand;
    public Transform RightHand;

    private Hand m_OvrHandLeft;
    private Hand m_OvrHandRight;

    private void Start() {
        m_OvrHandLeft = LeftHand.GetComponent<Hand>();
        m_OvrHandRight = RightHand.GetComponent<Hand>();
    }
    
    public override LocomotionTeleport.TeleportIntentions GetIntention() {
        if (!isActiveAndEnabled) {
            return LocomotionTeleport.TeleportIntentions.None;
        }

        if (m_OvrHandRight.GetIndexFingerIsPinching() &&
            LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.Aim) {
            return LocomotionTeleport.TeleportIntentions.Teleport;
        }
        if (m_OvrHandLeft.GetIndexFingerIsPinching()) {
            return LocomotionTeleport.TeleportIntentions.Aim;
        }
        return LocomotionTeleport.TeleportIntentions.None;
    }
    
    
    public override void GetAimData(out Ray aimRay)
    {
        var t = LocomotionTeleport.LocomotionController.CameraRig.leftHandAnchor.GetComponentInChildren<OVRHand>().PointerPose;
        aimRay = new Ray(LocomotionTeleport.LocomotionController.CameraRig.leftHandAnchor.position, transform.parent.TransformDirection(t.forward));
    }
}

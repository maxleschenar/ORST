using System;
using System.Collections.Generic;
using UnityEngine;
using ORST.Core.UI;

namespace ORST.Core.Interactions {
    public class HandHeadProximity : MonoBehaviour {
        public event Action ForbiddenSpaceEntered;
        public event Action ForbiddenSpaceExited;
        public event Action<float> ForbiddenSpaceUpdated;

        [SerializeField] private Collider m_InnerCollider;
        [SerializeField] private Collider m_OuterCollider;
        [Header("Title & Message")]
        [SerializeField] private string m_TitleOnInnerEnter;
        [SerializeField] private string m_MessageOnInnerEnter;

        private readonly List<Transform> m_Intersector = new();
        private GameObject m_ForbiddenGameObject;

        private void Update() {
            ForbiddenSpaceUpdated?.Invoke(GetClosestIntersectLinear());
        }

        private float GetClosestIntersectLinear() {
            if (m_Intersector.Count <= 0) {
                return 0.0f;
            }

            Vector3 innerColliderPosition =  m_InnerCollider.transform.position;
            float shortestDist = float.PositiveInfinity;
            Transform shortestTransform = null;
            foreach (Transform intersect in m_Intersector) {
                if (!m_OuterCollider.bounds.Contains(intersect.position)) {
                    continue;
                }

                if (m_InnerCollider.bounds.Contains(intersect.position)) {
                    if (!PopupManager.Instance.IsPopupShown()) {
                        PopupManager.Instance.OpenPopup();
                        PopupManager.Instance.DisplayInfo(m_TitleOnInnerEnter, m_MessageOnInnerEnter);
                    }
                } else {
                    PopupManager.Instance.ClosePopup();
                }

                Vector3 innerColliderDirVec = m_InnerCollider.transform.position - intersect.position;
                if (!m_InnerCollider.Raycast(new Ray(intersect.position, innerColliderDirVec),
                                             out RaycastHit hit, innerColliderDirVec.magnitude * 2.0f)) {
                    //@Maurice - In this case the raycast is unsuccessful because the "intersector" is inside
                    //the inner collider.
                    if (m_InnerCollider.bounds.Contains(intersect.position) && m_ForbiddenGameObject == null) {
                        m_ForbiddenGameObject = intersect.gameObject;
                        ForbiddenSpaceEntered?.Invoke();
                    }

                    return 1.0f;
                }

                if (m_ForbiddenGameObject != null
                 && m_ForbiddenGameObject.Equals(intersect.gameObject)) {
                    m_ForbiddenGameObject = null;
                    ForbiddenSpaceExited?.Invoke();
                }

                if (hit.distance < shortestDist) {
                    shortestDist = hit.distance;
                    shortestTransform = intersect;
                }
            }

            if (shortestTransform == null) {
                return 0.0f;
            }

            Vector3 shortestTransformPos = shortestTransform.position;
            Vector3 outerPoint = shortestTransformPos + (shortestTransformPos - innerColliderPosition).normalized * m_OuterCollider.transform.lossyScale.magnitude;
            Ray ray = new(outerPoint, (innerColliderPosition - shortestTransformPos).normalized * 10.0f);

            float inBetweenTotalDist = 1.0f;
            if (m_OuterCollider.Raycast(ray, out RaycastHit hit2, 100.0f)) {
                inBetweenTotalDist = shortestDist + (hit2.point - shortestTransform.position).magnitude;
            }

            return 1.0f - shortestDist / inBetweenTotalDist;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("MainCamera") || other.gameObject.CompareTag("LeftHand") ||
                other.gameObject.CompareTag("RightHand")) {
                m_Intersector.Add(other.transform);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!m_Intersector.Contains(other.transform)) {
                return;
            }
            m_Intersector.Remove(other.transform);
        }
    }
}
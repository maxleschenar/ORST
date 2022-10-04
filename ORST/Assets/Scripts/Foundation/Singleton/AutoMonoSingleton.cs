﻿using UnityEngine;

namespace ORST.Foundation.Singleton {
    public abstract class AutoMonoSingleton<T> : MonoBehaviour where T : AutoMonoSingleton<T> {
        private static T s_Instance;

        public static T Instance {
            get {
                // Fix unity behaviour with fake null so we can use null coalescing operators
                if (s_Instance is not null && s_Instance == null) {
                    s_Instance = null;
                }

                // ReSharper disable once Unity.NoNullCoalescing
                return s_Instance ?? (s_Instance = FindObjectOfType<T>()) ?? (s_Instance = new GameObject(typeof(T).Name).AddComponent<T>());
            }
        }

        private void Awake() {
            if (s_Instance != null && s_Instance != this) {
                Debug.LogError($"Multiple instances of {typeof(T).Name} found");
                Destroy(this);
                OnAwake();
                return;
            }

            s_Instance = this as T;
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}
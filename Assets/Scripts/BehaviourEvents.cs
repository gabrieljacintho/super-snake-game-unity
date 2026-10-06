using UnityEngine;
using UnityEngine.Events;

namespace GabrielBertasso
{
    public class BehaviourEvents : MonoBehaviour
    {
        public UnityEvent OnEnableEvent;
        public UnityEvent OnDisableEvent;

        private void OnEnable()
        {
            OnEnableEvent?.Invoke();
        }

        private void OnDisable()
        {
            OnDisableEvent?.Invoke();
        }
    }
}

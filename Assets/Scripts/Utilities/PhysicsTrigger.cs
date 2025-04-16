using UnityEngine;
using UnityEngine.Events;

namespace JTUtility
{
    public class PhysicsTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool triggerOnlyOnce = false;

        [SerializeField]
        private string[] triggerableTags = new string[0];

        [SerializeField]
        private UnityEvent onTrigged = new UnityEvent();

        private bool triggeed = false;

        [SerializeField] private ColliderEvent OnEnterTrigger = new ColliderEvent();
        [SerializeField] private ColliderEvent OnStayTrigger = new ColliderEvent();
        [SerializeField] private ColliderEvent OnExitTrigger = new ColliderEvent();

        [SerializeField] private CollisionEvent OnStartCollision = new CollisionEvent();
        [SerializeField] private CollisionEvent OnStayCollision = new CollisionEvent();
        [SerializeField] private CollisionEvent OnExitCollision = new CollisionEvent();

        [SerializeField] private Collider2DEvent OnEnterTrigger2D = new Collider2DEvent();
        [SerializeField] private Collider2DEvent OnStayTrigger2D = new Collider2DEvent();
        [SerializeField] private Collider2DEvent OnExitTrigger2D = new Collider2DEvent();

        [SerializeField] private Collision2DEvent OnStartCollision2D = new Collision2DEvent();
        [SerializeField] private Collision2DEvent OnStayCollision2D = new Collision2DEvent();
        [SerializeField] private Collision2DEvent OnExitCollision2D = new Collision2DEvent();

        private void OnTriggerEnter(Collider collider)
        {
            if (!IsTriggerable(collider)) return;

            triggeed = true;
            if (OnEnterTrigger != null)
            {
                OnEnterTrigger.Invoke(collider);
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if (!IsTriggerable(collider)) return;

            triggeed = true;
            if (OnStayTrigger != null)
            {
                OnStayTrigger.Invoke(collider);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (!IsTriggerable(collider)) return;

            triggeed = true;
            if (OnExitTrigger != null)
            {
                OnExitTrigger.Invoke(collider);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsTriggerable(collision.collider)) return;

            triggeed = true;
            if (OnStartCollision != null)
            {
                OnStartCollision.Invoke(collision);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!IsTriggerable(collision.collider)) return;

            triggeed = true;
            if (OnStayCollision != null)
            {
                OnStayCollision.Invoke(collision);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!IsTriggerable(collision.collider)) return;

            triggeed = true;
            if (OnExitCollision != null)
            {
                OnExitCollision.Invoke(collision);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!IsTriggerable(collider)) return;

            triggeed = true;
            if (OnEnterTrigger2D != null)
            {
                OnEnterTrigger2D.Invoke(collider);
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (!IsTriggerable(collider)) return;

            triggeed = true;
            if (OnStayTrigger2D != null)
            {
                OnStayTrigger2D.Invoke(collider);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!IsTriggerable(collider)) return;

            triggeed = true;
            if (OnExitTrigger2D != null)
            {
                OnExitTrigger2D.Invoke(collider);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsTriggerable(collision.collider)) return;

            triggeed = true;
            if (OnStartCollision2D != null)
            {
                OnStartCollision2D.Invoke(collision);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!IsTriggerable(collision.collider)) return;

            triggeed = true;
            if (OnStayCollision2D != null)
            {
                OnStayCollision2D.Invoke(collision);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!IsTriggerable(collision.collider)) return;

            triggeed = true;
            if (OnExitCollision2D != null)
            {
                OnExitCollision2D.Invoke(collision);
            }
        }

        private bool IsTriggerable(Collider collider)
        {
            if (triggeed && triggerOnlyOnce) return false;

            if (triggerableTags.Length == 0) return true;

            foreach (string s in triggerableTags)
            {
                if (s == collider.tag) return true;
            }

            return false;
        }

        private bool IsTriggerable(Collider2D collider)
        {
            if (triggeed && triggerOnlyOnce) return false;

            if (triggerableTags.Length == 0) return true;

            foreach (string s in triggerableTags)
            {
                if (s == collider.tag) return true;
            }

            return false;
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

namespace RpgAdventure
 {
    public class BanditBehaviourScript : MonoBehaviour
    {
        public float detectionRadius = 10.0f;
        public float detectionAngle = 90.0f;
        public float timeToStopPursuit = 2.0f;

        private PlayerController m_Target;
        private NavMeshAgent m_NavMestAgent;
        private float m_TimeSinceLostTarget = 0;
        private void Awake()
        {
            m_NavMestAgent= GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            var target = LookforPlayer();

            if (m_Target == null)
            {
                if (target != null)
                {
                    m_Target = target;
                }
            }
            else
            {
                m_NavMestAgent.SetDestination(m_Target.transform.position);

                if (target == null)
                {
                    m_TimeSinceLostTarget += Time.deltaTime;

                    if(m_TimeSinceLostTarget >= timeToStopPursuit)
                    {
                        m_Target = null;
                    }
                    else
                    {
                        m_TimeSinceLostTarget = 0;
                    }
                }
            }

           

           

        }

        private PlayerController LookforPlayer()
        {
                if (PlayerController.Instance == null)
                {
                    return null;
                }
            
            Vector3 enemyPosition = transform.position;
            Vector3 toplayer = PlayerController.Instance.transform.position - enemyPosition;
            toplayer.y = 0;

            if (toplayer.magnitude <= detectionRadius) 
            {
                if(Vector3.Dot(toplayer.normalized, transform.forward) >
                   Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {
                    return PlayerController.Instance;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color c = new Color(0.8f, 0, 0.7f, 0.4f);
            UnityEditor.Handles.color = c;

            Vector3 rotatedforward = Quaternion.Euler(
                0, -detectionAngle * 0.5f, 0) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position, 
                Vector3.up, rotatedforward, detectionAngle, detectionRadius);
        }
#endif

    }
  }
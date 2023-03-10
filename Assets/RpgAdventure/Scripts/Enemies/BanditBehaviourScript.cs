using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RpgAdventure
 {
    public class BanditBehaviourScript : MonoBehaviour
    {
        public PlayerScanner playerScanner;
        public float timeToStopPursuit = 2.0f;
        public float timeToWaitOnPursuit = 2.0f;

        private PlayerController m_Target;
        private EnemyController m_EnemyController;
        private Animator m_Animator;
        private float m_TimeSinceLostTarget = 0;
        private Vector3 m_OriginPosition;

        private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
        private void Awake()
        {
            m_EnemyController = GetComponent<EnemyController>();
            m_Animator= GetComponent<Animator>();
            m_OriginPosition = transform.position;
        }

        private void Update()
        {
            var target = playerScanner.Detect(transform);

            if (m_Target == null)
            {
                if (target != null)
                {
                    m_Target = target;
                }
            }
            else
            {
                m_EnemyController.SetFollowTarget(m_Target.transform.position);
                m_Animator.SetBool(m_HashInPursuit, true);

                if (target == null)
                {
                    m_TimeSinceLostTarget += Time.deltaTime;

                    if (m_TimeSinceLostTarget >= timeToStopPursuit)
                    {
                        m_Target = null;
                        m_Animator.SetBool(m_HashInPursuit, false);
                        StartCoroutine(waitOnPursuit());
                    }
                }
                else
                {
                    m_TimeSinceLostTarget = 0;
                }   
            }
            Vector3 toBase = m_OriginPosition - transform.position;
            toBase.y = 0;

            m_Animator.SetBool(m_HashNearBase, toBase.magnitude < 0.01f);
        }


        private IEnumerator waitOnPursuit()
        {
            yield return new WaitForSeconds(timeToWaitOnPursuit);
            m_EnemyController.SetFollowTarget(m_OriginPosition);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color c = new Color(0.8f, 0, 0.7f, 0.4f);
            UnityEditor.Handles.color = c;

            Vector3 rotatedforward = Quaternion.Euler(
                0, -playerScanner.detectionAngle * 0.5f, 0) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position, 
                Vector3.up, rotatedforward, 
                playerScanner.detectionAngle, 
                playerScanner.detectionRadius);
        }
#endif

    }
  }
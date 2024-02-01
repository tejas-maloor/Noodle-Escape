using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float moveSpeed = 5f;
    //public float attackDistance = 2f;
    public float stoppingDistance = 2f;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private float coolDown;
    bool attackPlayer = false;

    Transform target;
    private float lastAttackedAt = -9999f;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        target = PlayerManager.instance.player.transform;
    }

    private void Update()
    {
        if (PlayerManager.instance.isDead) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius) 
        {
            //var newPos = Vector3.MoveTowards(transform.position, target.position - new Vector3(stoppingDistance, 0, stoppingDistance), moveSpeed * Time.deltaTime);
            //transform.position = new Vector3(newPos.x, 0, newPos.y);

            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance)
            {
                if (Time.time > lastAttackedAt + coolDown)
                {
                    anim.SetTrigger("Attack");

                    if(Vector3.Distance(PlayerManager.instance.player.transform.position, transform.position) <= 5)
                    {
                        Debug.Log("HitPlayer");
                        PlayerManager.instance.playerController.TakeDamage();
                    }

                    lastAttackedAt = Time.time;
                }
            }
        }

        anim.SetBool("Running", (agent.velocity.magnitude > 0));

        FaceTarget();
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius); 
    }

}

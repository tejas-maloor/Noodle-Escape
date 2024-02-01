using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;

    public void OnHit(int flag)
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position + (transform.forward * 2), 3f, enemyLayer);

        if (enemies.Length != 0)
        {
            if(flag == 1) 
            {
                foreach(var enemy in enemies) 
                {
                    enemy.GetComponent<Enemy>().TakeHit();
                }
            }
            else
            {
                enemies[0].GetComponent<Enemy>().TakeHit();
            }

            if(PlayerManager.instance.stateMachine.CurrentState.GetType() == typeof(GroundFinisherState))
            {
                SoundManager.instance.PlaySFX(PlayerManager.instance.playerController.hitSound);
                SoundManager.instance.PlayDialogue(PlayerManager.instance.playerController.kickDialogue);
            }
            else if(PlayerManager.instance.stateMachine.CurrentState.GetType() == typeof(GroundEntryState))
            {
                SoundManager.instance.PlaySFX(PlayerManager.instance.playerController.hitSound);
            }
            else if(PlayerManager.instance.stateMachine.CurrentState.GetType() == typeof(GroundComboState))
            {
                SoundManager.instance.PlaySFX(PlayerManager.instance.playerController.hitSound);
            }

            Vector3 direction = (enemies[0].transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.parent.rotation = lookRotation * Quaternion.Euler(0, 15, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (transform.forward * 2), 3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

  
     [Header ("Attack Parameters")]
    [SerializeField] private float colliderDistance;
    
    [Header ("Player Layer")]
    
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private LayerMask wallLayer;
    private bool canSeePlayer;
    private EnemyPatrol enemyPatrol;
    private float cooldownTimer = Mathf.Infinity;
    private Health playerHealth;
    private Animator anim; 

    private void Awake(){
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update(){
        
        cooldownTimer += Time.deltaTime;

        DamagePlayer();
        
        /*if(PlayerInSight()){
            if(cooldownTimer >= attackCooldown){
            cooldownTimer = 0; 
            anim.SetTrigger("melee");
        }
        }*/

        if(enemyPatrol!=null){
            enemyPatrol.enabled = !PlayerInSight();
        }
    }  

    private bool PlayerInSight(){
        //RaycastHit2D hit = Physics2D.BoxCast(
        //    boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        //    new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
        //    0, 
        //    Vector2.left, 
        //    0, 
        //    playerLayer);
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            Vector2.left,
            0,
            playerLayer);



        if (hit.collider != null){
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null && !canSeePlayer;
    }

    void CanSee(bool seeablePlayer) {
        canSeePlayer = seeablePlayer;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    
    private void DamagePlayer(){
        if(PlayerInSight()) {
            playerHealth.TakeDamage(damage);
        }
    }

}

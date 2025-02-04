using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth{get; private set;}
    private Animator anim;
    private bool dead; 

    [Header ("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numFlashes;
    private SpriteRenderer spriteRend; 

    private void Awake(){
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage){
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth > 0){
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        } else{
            if(!dead){
                anim.SetTrigger("die");
                ////Player
                //if(GetComponent < PlayerMovement>() != null){
                //    GetComponent<PlayerMovement>().enabled = false;
                //    dead = true; 
                //}
                
                //Enemy
                if(GetComponent <MeleeEnemy>() != null){
                    GetComponentInParent<EnemyPatrol>().enabled = false;
                    GetComponent<MeleeEnemy>().enabled = false;
                    dead = true;
                } 
            } 
        }
    }

    public bool isDead(){
        return dead; 
    }
    
    public void AddHealth(float _value){
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability(){
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for(int i = 0; i<numFlashes; i++){
            spriteRend.color = new Color (1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/(numFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/(numFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

}

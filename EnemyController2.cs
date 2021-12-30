using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2 : MonoBehaviour
{
    [SerializeField] Transform _heroPos;
    float dist;
    Animator anim;
    int Can;
    void Start()
    {
        anim=gameObject.GetComponent<Animator>();
        Can=5;
    }   
    void Update()
    {
        dist=Vector3.Distance(_heroPos.position,transform.parent.position);
        
        if (dist < 3f) anim.SetBool("isAttack", true);
            else anim.SetBool("isAttack", false);
    }
    public void Damage(){
        Can--;
        
        if(Can<=0){
            anim.SetTrigger("isDeath");
            Invoke("Destroyer",0.5f);
        }else anim.SetTrigger("isHit");
        Debug.Log(Can);
    }
    void Destroyer(){
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("HeroDamage"))
        {
            collision.transform.parent.gameObject.GetComponent<HeroController>().Damage();
            collision.transform.parent.gameObject.GetComponent<HeroController>().HitAnim();
        }
    }
}

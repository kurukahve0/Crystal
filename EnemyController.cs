using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    Transform _heroPos;
    
    UIControl UIControl;

    [Header("Hareket")]
    float _moveSpeed=2f,dist;
    float direction = -1;
    bool _hareketDevam;
    float hareketOrtalama;
    int Can;

    [Header("Takip")]
    public bool _isHeroSignal;
    public float _takipHiz;
    [Header("Sınır")]

    [SerializeField] float _xMin,_xMax;
    [SerializeField] float _yMin,_yMax;

    [Header("Animator")]
    Animator anim;


    void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        UIControl=GameObject.Find("Sınır").gameObject.GetComponent<UIControl>();
        _heroPos = GameObject.Find("HeroPer").transform;
        _isHeroSignal = false;
        _hareketDevam=true;
        anim = gameObject.GetComponent<Animator>();
        Can=3;
        
    }

    
    void Update()
    {
        
        dist = Vector3.Distance(_heroPos.position,transform.parent.position);

        rb.velocity = dist > 0.7f ? new Vector2(_moveSpeed * direction, rb.velocity.y): Vector2.zero;
 
        if(_isHeroSignal)
        {            
            direction = Mathf.Sign(_heroPos.position.x-transform.parent.position.x);
            if (dist < 0.7f) anim.SetBool("isAttack", true);
            else anim.SetBool("isAttack", false);
        }
        heroSignal();
        MaxPosition();



    }
    private void FixedUpdate()
    {
        if ((transform.parent.position.x+0.2f<_heroPos.position.x) || 
            (transform.parent.position.x - 0.2f>_heroPos.position.x))
        {
            transform.parent.rotation = direction <= 0 ? (Quaternion.Euler(0f, 180f, 0f)) : 
            (Quaternion.Euler(0f, 0f, 0f));
        }
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("HeroDamage"))
        {
            collision.transform.parent.gameObject.GetComponent<HeroController>().Damage();
            collision.transform.parent.gameObject.GetComponent<HeroController>().HitAnim();
        }
    }
   
    void MaxPosition(){
        if ((transform.parent.position.x>=_xMax || transform.parent.position.x<=_xMin) && _hareketDevam)
        {
            direction *= -1; 
            _hareketDevam=false;              
        }
        hareketOrtalama=(_xMax+_xMin)/2;
        if(hareketOrtalama+1>=transform.parent.position.x 
        && hareketOrtalama-1<=transform.parent.position.x
        &&  !_hareketDevam)
        _hareketDevam=true;
        
    }
    void heroSignal(){
        if ((_heroPos.position.x <=_xMax && _heroPos.position.x>=_xMin) 
        && (_heroPos.position.y <=_yMax && _heroPos.position.y>=_yMin))
        { _hareketDevam=true;
            _isHeroSignal=true;
        }else _isHeroSignal=false;
    }

    public void Damage(){
        Can--;
        Debug.Log(Can.ToString());
        if(Can<=0)
        {
        
        gameObject.GetComponent<CapsuleCollider2D>().isTrigger=true;
        gameObject.GetComponent<BoxCollider2D>().enabled=false;
        Invoke("Destroyer",0.5f);
        anim.SetTrigger("isDeath");
        }
        anim.SetTrigger("isHit");      
    }

    void Destroyer(){
        Destroy(gameObject);
        UIControl.PlatformOpen();
    }
    
}

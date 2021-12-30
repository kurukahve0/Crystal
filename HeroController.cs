using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class HeroController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;


    [Header("Mekanik")]
    [SerializeField] float _upSpeed,_axisSpeed;
    bool _isGround = false;
    float horizontal=0f;
    EnemyController enemy;
    bool _attackKontrol;
    
    [Header("UI")]
    int Can; 
    [SerializeField] GameObject[] _canBari;
    bool _oyunBitti;

    [Header("Check")]
    [SerializeField] Vector3[] _checkPoint;
    [SerializeField] GameObject[] _checkGameObject;
    

    [Header("Sınır")]
    [SerializeField] Transform _cameraPos;
    int _checkNo;

    [Header("Text")]
    public TextMeshProUGUI _text;
   
    [Header("Joystick")]
    public FixedJoystick _joyStick;
    
    private void Update()
    {
        if(!_oyunBitti){
        //Attack();
        //jump();
        Run();
        }
        
    }
    private void Start()
    {
        _joyStick=FindObjectOfType<FixedJoystick>();
        _oyunBitti=false;
        _attackKontrol=true;
        Can=3;
        _checkNo=2;
        for(int i=0;i<=_checkNo;i++){
            _checkPoint[i]=_checkGameObject[i].transform.position;
        }
        _text.text="Ali eren akyuz";
            Invoke("Sil",1f);
        horizontal=0f;
        anim.SetBool("isDeath",false);
    }
    public void Attack()
    {
        if(_attackKontrol && !_oyunBitti){
        anim.SetTrigger("isAD"); 
        _attackKontrol=false;
        Invoke("DeAttack",0.65f);
        }     
        
    }
    public void DeAttack()
    {
              
        //anim.SetBool("isAttack", false);
        Debug.Log("deattachh");
        _attackKontrol=true;
    }

    public void jump()
    {
        
        if ( _isGround&& !_oyunBitti)
        {
            rb.AddForce(Vector3.up * _upSpeed,ForceMode2D.Force);  
            anim.SetBool("isJump", true);
            _isGround = false;
        }
        
    }
   /* private void Run(){
             
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _axisSpeed,rb.velocity.y);
        if (Input.GetAxisRaw("Horizontal") > 0)
            transform.parent.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (Input.GetAxisRaw("Horizontal") < 0)
            transform.parent.rotation = Quaternion.Euler(0f, 180f, 0f);
        if (Input.GetAxisRaw("Horizontal") == 0 ) anim.SetBool("isRun", false);
        else anim.SetBool("isRun",true);
    }*/
    private void Run(){
         
         
         horizontal=_joyStick.Horizontal>=0.5f || _joyStick.Horizontal<=-0.5f 
         ? Mathf.Sign(_joyStick.Horizontal):0f;            
        rb.velocity = new Vector2(horizontal * _axisSpeed,rb.velocity.y);
        if (horizontal > 0)
            transform.parent.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (horizontal < 0)
            transform.parent.rotation = Quaternion.Euler(0f, 180f, 0f);
        if (horizontal == 0 ) anim.SetBool("isRun", false);
        else anim.SetBool("isRun",true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _isGround = true;
            anim.SetBool("isJump", false);
            anim.SetBool("isAir", false);
        }
        if(collision.transform.CompareTag("Son")){
            Debug.Log("WİN");
            _text.text="Win";
            Invoke("Sil",0.5f);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            
            anim.SetBool("isAir", true);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.CompareTag("EnemyDamage") )
        {
             collision.transform.parent.gameObject.GetComponent<EnemyController>().Damage();           
        }
        if (collision.transform.CompareTag("Enemy2Damage") )
        {
             collision.transform.parent.gameObject.GetComponent<EnemyController2>().Damage();           
        }
        if (collision.transform.CompareTag("check"))
        {
            _checkNo--;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled=false; 
            _text.text="checkpoint";
            Invoke("Sil",1f);

        }
        if (collision.transform.CompareTag("thorny")){
            CheckHero();
        }
        if(collision.transform.CompareTag("Kalp")){
            Destroy(collision.gameObject);
            CanAl();
        }
    }

    private void Sil(){
            _text.text="";
            if(_oyunBitti)
            {
            SceneManager.LoadScene(0);
            }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Signal"))
        {           
            enemy._isHeroSignal = false;
        }
    }
    public void Damage()
    {
        Can--;
        
        if(Can>=0)
        _canBari[Can].SetActive(false);        
        else   {
        _oyunBitti=true;              
        _text.text="Game Over";
        anim.SetBool("isDeath",true);
        rb.velocity=Vector2.zero;
        rb.gravityScale=0f;
        Invoke("Sil",1f);
        }  
    }
    private void CanAl()
    {        
        if(Can<3){       
        _canBari[Can].SetActive(true); 
        Can++;
        }
    }
    public void HitAnim(){
        anim.SetTrigger("isHit");
    }
    private void CheckHero()
    {
        Damage();
        if(Can!=-1)
        gameObject.transform.parent.position=_checkPoint[_checkNo];

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform hero;
    [SerializeField] float _cameraSpeed,_cam1Son;
    Vector3 target;
    bool _baslangicDeger;
    Transform _camPos;
       
       public SpriteRenderer rink;

    private void Start()
    {
        _baslangicDeger = false;
        target = new Vector3 (transform.position.x,transform.position.y+2f,transform.position.z);
        _camPos=gameObject.transform;
        _baslangicDeger=true;
        Camera.main.orthographicSize = rink.bounds.size.x * Screen.height / Screen.width * 0.5f; // ekran ayarlama
    }
    void FixedUpdate()
    {
        if (_baslangicDeger && hero.transform.position.x>=0)
        {   if(hero.position.x<=55f && hero.position.y<=34.5f) // oyun sonu pozisyon
            transform.position = Vector3.Lerp(transform.position, hero.position + target, _cameraSpeed);                    
            else
            transform.position = Vector3.Lerp(transform.position,
             new Vector3(transform.position.x,hero.position.y,-10f) + target, _cameraSpeed); 
        }
        if(((hero.position.y>=-1.9 && hero.position.y<=17f) && hero.position.x>=37) || hero.position.x<=0) // ilk katman pozisyon
            {                   
                _cam1Son = hero.position.x<=0 ? 0f : 37f; // ilk son pozisyon ayarlama
                
                //gameObject.transform.position=new Vector3(_cam1Son,transform.position.y,transform.position.z);
                gameObject.transform.position=Vector3.Lerp(transform.position,
                new Vector3(_cam1Son,hero.position.y,hero.position.z) + target
                , _cameraSpeed);
            }
      
    }

    void Baslangic()
    {
       /* if (hero.position.y>-2.5 || hero.position.x>1)
        {
            _baslangicDeger = true;
        }
        Debug.Log(_baslangicDeger);*/
    }


}

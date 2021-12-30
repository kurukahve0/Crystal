using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIControl : MonoBehaviour
{   
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] GameObject[] Platformlar;

    int Count;
    void Start(){
        Count=0;
    }
    public void PlatformOpen()
    {   if(Count<2){
        _text.text="Platform Open";
        Invoke("Sil",1f);
        Platformlar[Count].SetActive(true);
        
        Count++;
    }
    }
     private void Sil()
    {
        _text.text="";            
    }
    
}

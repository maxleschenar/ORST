
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PointerEvent : MonoBehaviour
{
    public GameObject[] game_object;
    private GameObject GameObjectHit;

    private GameObject child;

   
    void FixedUpdate ()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Ray ray = new Ray(transform.position, forward);
        Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit;
        if ( Physics.Raycast (ray,out hit,100.0f))
        {
            for (int i=0;i<game_object.Length;i++)
            {
                if (hit.transform.gameObject.name==game_object[i].name)
                {
                    game_object[i].GetComponent<Renderer>().material.SetFloat("_enable",1.0f);
                    GameObjectHit = hit.transform.gameObject;
                    child = GameObjectHit.transform.gameObject;
                    Debug.Log(GameObjectHit);
                }
                else 
                {
                game_object[i].GetComponent<Renderer>().material.SetFloat("_enable",0.0f);  
                Debug.Log("Theres no hit :(");
                }          
            }

        }
        
        else
        {
            for (int i=0;i<game_object.Length;i++)
            {
                game_object[i].GetComponent<Renderer>().material.SetFloat("_enable",0.0f);  
                Debug.Log("Theres no hit :(");
                
            }          
        }
    }
}
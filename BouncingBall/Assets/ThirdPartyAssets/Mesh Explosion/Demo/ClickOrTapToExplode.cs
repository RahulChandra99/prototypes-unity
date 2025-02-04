using System;
using System.Collections;
using System.Resources;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClickOrTapToExplode : MonoBehaviour
{

#if UNITY_EDITOR || (!NITY_EDITOR && !(UNITY_IPHONE || UNITY_ANDROID))

#endif

	/*private void Start()
	{
		StartExplosion();
	}*/

	/*void Update()
    {
         var thisCollider = GetComponent<Collider>();
        
         foreach (var i in Input.touches) {
        	if (i.phase != TouchPhase.Began) {
        		continue;
         	}
         	
         	// It's kinda wasteful to do this raycast repeatedly for every ClickToExplode in the
         	// scene, but since this component is just for testing I don't think it's worth the
         	// bother to figure out some shared static solution.
         	RaycastHit hit;
         	if (!Physics.Raycast(Camera.main.ScreenPointToRay(i.position), out hit)) {
         		continue;
         	}
         	if (hit.collider != thisCollider) {
         		continue;
         	}
         	
         	StartExplosion();
         	return;
        }
    }*/

    public void DestroyStuff()
    {
        StartExplosion();
    }

    void StartExplosion()
    {
        BroadcastMessage("Explode");
        
        
        gameObject.SetActive(false);
        
    }
    

}

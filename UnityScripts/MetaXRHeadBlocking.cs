using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Meta XR Head Blocking Script (Synaptic Response)
// Blocks camera movement through objects (configurable through collision layers)
// Tested with Unity 6 (6000.0.31f1 LTS) + Meta XR All-in-One SDK (71.0.0)
// 1) Add this script to the CenterEyeAnchor component of the OVRCameraRig
// 2) Set the Player field to your root object (i.e. "OVRCameraRig")
// 3) If your player component has colliders set their label to "Player"
// Initial Implementation: Scott Lupton (12.21.2024)

public class MetaXRHeadBlocking : MonoBehaviour
{     
    
    [SerializeField] public GameObject player = null; 
    [SerializeField] private LayerMask _collisionLayers = 1 << 0;
    [SerializeField] private float _collisionRadius = 0.2f;
    
    private Vector3 prevHeadPos;
    
    private void Start()
    {
        prevHeadPos = transform.position;
    }

    private bool DetectHit(Vector3 loc)
    {
            Collider[] objs = new Collider[10];
            int size = Physics.OverlapSphereNonAlloc(loc, _collisionRadius, objs,
                       _collisionLayers, QueryTriggerInteraction.Ignore);
            for (int i = 0 ; i < size; i++) {
                if (objs[i].tag != "Player") {                    
                    return true;
                }                
            }
            return false;
    }
    
    public void Update()
    {        
        if (player != null) {                                   
            
            bool collision = DetectHit(transform.position);
            
            // No collision
            if (!collision) prevHeadPos = transform.position;                           
                
            // Collision
            else {             
                Vector3 headDiff = transform.position - prevHeadPos;
                Vector3 adjHeadPos = new Vector3(player.transform.position.x-headDiff.x,
                                                 player.transform.position.y,
                                                 player.transform.position.z-headDiff.z);
                player.transform.SetPositionAndRotation(adjHeadPos, player.transform.rotation);
            }
        }
    }
}

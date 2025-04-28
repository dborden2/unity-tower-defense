using UnityEngine;
using System.Collections;
using System.Collections.Generic

public class TOWERPLACEMENT : MonoBehavior
{
    public Vector3 place;
    public GameObject tower;

    private RaycastHit _hit;

    public bool placing;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && placing == true)
        {
            if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition, out _hit)))
            {
                if(_hit.transfrom.tag == "ground")
                {
                place = new Vector3(_hit.point.x, _hit.point.y,_hit.point.z);

                Instantiate(tower, place, Quanternion.identity);

                placing = false;
                }
            }
        }
    }

    
    
}

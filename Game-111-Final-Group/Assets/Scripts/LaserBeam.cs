using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public LayerMask mask;
    public Transform target;
    public LineRenderer laser;
    public BoxCollider boxCollider;
    private float distance;
    private RaycastHit hitInfo;
    private Vector3 direction;
    private Transform blocker;
    private void Awake()
    {
        direction = Vector3.Normalize( target.position - transform.position);
    }
    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.right, out hitInfo,20, mask))
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, hitInfo.point);
            distance = Vector3.Distance(transform.position, hitInfo.transform.position);
            boxCollider.size = new Vector3(distance, 0.25f, 0.25f);
            boxCollider.center = new Vector3(distance * 0.5f, 0, 0);
        }
        else
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, transform.position + (transform.right * 20));
            distance = Vector3.Distance(transform.position, transform.position + (transform.right * 20));
            boxCollider.size = new Vector3(distance, 0.25f, 0.25f);
            boxCollider.center = new Vector3(distance * 0.5f, 0, 0);
        }
    }
    private void OnDrawGizmos()
    {
        if (Physics.Raycast(transform.position, transform.right, out hitInfo, mask))
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, hitInfo.point);
            distance = Vector3.Distance(transform.position, hitInfo.transform.position);
            boxCollider.size = new Vector3(distance, 0.25f, 0.25f);
            boxCollider.center = new Vector3(distance * 0.5f, 0, 0);
        }
        else
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, transform.position + (transform.right * 20));
            distance = Vector3.Distance(transform.position, transform.position + (transform.right * 20));
            boxCollider.size = new Vector3(distance, 0.25f, 0.25f);
            boxCollider.center = new Vector3(distance * 0.5f, 0, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private Camera _topDownCamera;
    [SerializeField]
    private GameObject _navTargetObject;

    private NavMeshPath _path;
    private LineRenderer _lineRenderer;

    private bool _lineToggle = false;


    // Start is called before the first frame update
    void Start()
    {
        _path = new NavMeshPath();
        _lineRenderer = transform.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began )) 
        {
            _lineToggle = !_lineToggle;
        }

        if (_lineToggle)
        {
            NavMesh.CalculatePath(transform.position, _navTargetObject.transform.position, NavMesh.AllAreas, _path);
            _lineRenderer.positionCount = _path.corners.Length;
            _lineRenderer.SetPositions(_path.corners);
            _lineRenderer.enabled = true;
        }
    }
}

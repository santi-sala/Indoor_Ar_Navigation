using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _navigationTargetDropdown;
    [SerializeField]
    private List<Target> _navigationTargetObjects = new List<Target>();
    

    //private Camera _topDownCamera;
    //[SerializeField]
    //private GameObject _navTargetObject;

    private NavMeshPath _path;
    private LineRenderer _lineRenderer;
    private Vector3 _targetPosition = Vector3.zero;


    private bool _lineToggle = false;


    // Start is called before the first frame update
    void Start()
    {
        _path = new NavMeshPath();
        _lineRenderer = transform.GetComponent<LineRenderer>();
        _lineRenderer.enabled = _lineToggle;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lineToggle && _targetPosition != Vector3.zero) 
        {
            NavMesh.CalculatePath(transform.position, _targetPosition, NavMesh.AllAreas, _path);
            _lineRenderer.positionCount = _path.corners.Length;
            _lineRenderer.SetPositions(_path.corners);
            _lineRenderer.enabled = true;
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        _targetPosition = Vector3.zero;
        string _selectedText = _navigationTargetDropdown.options[selectedValue].text;
        Target _currentTarget = _navigationTargetObjects.Find(selectedTarget => selectedTarget.Name.Equals(_selectedText));
        if (_currentTarget != null)
        {
            _targetPosition = _currentTarget.PositionObject.transform.position;
        }
    }

    public void ToggleVisibility()
    {
        _lineToggle = !_lineToggle;
        _lineRenderer.enabled = _lineToggle;
    }
}

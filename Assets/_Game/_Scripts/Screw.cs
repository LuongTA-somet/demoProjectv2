using UnityEngine;
using DG.Tweening;

public class Screw : MonoBehaviour
{

    [SerializeField] private Animation _anim;
    [SerializeField] private LayerMask _boltLayerMask;
    [SerializeField] private LayerMask _holeLayerMask;
    [SerializeField] private GameObject _hingeJointPrefab;
    private CircleCollider2D _circleCollider;
    private bool _isUp = false;
    private Hole _currentHole = null;
    public bool IsUp => _isUp;

    private void Start()
    {
        _anim = GetComponent<Animation>();
        _circleCollider = GetComponent<CircleCollider2D>();
        Initialize();
    }

    private void Initialize()
    {
        DetectHole(GetRaycastHits(_holeLayerMask));
        DetectBolts(GetRaycastHits(_boltLayerMask));
    }

    private void DetectBolts(RaycastHit2D[] hits)
    {
        foreach (var hit in hits)
        {

            if (!hit.transform.CompareTag("Bolt")) continue;

            Rigidbody2D rb = hit.transform.GetComponent<Rigidbody2D>();

            float distance = Vector2.Distance(transform.position, hit.transform.position);


            if (distance <= 5f && rb != null)
            {
                var hingeJoint = Instantiate(_hingeJointPrefab, transform).GetComponent<HingeJoint2D>();
                hingeJoint.connectedBody = rb;
            }
        }
    }
    private void DetectHole(RaycastHit2D[] hits)
    {
        foreach (var hit in hits)
        {
            if (!hit.transform.CompareTag("Hole")) continue;
            Hole hole = hit.transform.GetComponent<Hole>();
            if (hole != null)
            {
                hole.AssignScrew(this);
                _currentHole = hole;
                break;
            }
        }
    }

    private RaycastHit2D[] GetRaycastHits(LayerMask layerMask)
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        return Physics2D.RaycastAll(position, Vector2.zero, layerMask);
    }

    public void SelectThis()
    {
        ToggleScrewState();
    }

    public void SelectHole(Hole hole)
    {
        if (_currentHole == hole)
        {
            ToggleScrewState();
        }
        else if (_isUp)
        {
            MoveToHole(hole);
        }
    }

    private void ToggleScrewState()
    {
        _isUp = !_isUp;
        if (_isUp)
        {
            PlayScrewAnimation("ScrewUp2");
        }
        else
        {
            PlayScrewAnimation("ScrewDown2");
        }
    }
    public void ResetToInitialPosition()
    {
        if (_isUp)
        {
            ToggleScrewState();
        }
    }
    private void PlayScrewAnimation(string animationName)
    {
        _anim.Play(animationName);
    }


    private void MoveToHole(Hole hole)
    {
        _currentHole.RemoveScrew();
        RemoveJoints();
        _circleCollider.enabled = false;
        transform.DOMove(hole.transform.position, 0.2f).OnComplete(() =>
        {
            ToggleScrewState();
            hole.AssignScrew(this);
            _currentHole = hole;
            _circleCollider.enabled=true;
        });
    }

    private void RemoveJoints()
    {
        foreach (Transform t in this.transform)
        {
            if (!t.GetComponent<HingeJoint2D>()) continue;
            Destroy(t.gameObject);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerController : MonoBehaviour
{
    private Screw _currentScrew;
    [SerializeField] private Texture2D texture2D;

    private void Start()
    {
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPosition2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition2D, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                Hole hole = hit.transform.GetComponent<Hole>();
                if (hole != null)
                {
                    hole.SelectThis(this);
                    break;
                }
            }
        }
   
    }
    public void SelectHole(Hole hole)
    {
        if (hole == null || _currentScrew == null) return;
        _currentScrew.SelectHole(hole);
    }

    public void SelectScrew(Screw screw)
    {
        if (screw == null) return;

        if (_currentScrew != null && _currentScrew != screw)
        {
            _currentScrew.ResetToInitialPosition();
        }
        _currentScrew = screw;
        _currentScrew.SelectThis();
    }

}
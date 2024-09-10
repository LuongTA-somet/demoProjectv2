using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private Screw _currentScrew = null;
    private void Start()
    {
        CreateJointForNuts();
    }
    private void CreateJointForNuts()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Screw screw = hit.transform.GetComponent<Screw>();
            if (screw != null)
            {
                AssignScrew(screw);
                break;
            }
        }
    }
    public void AssignScrew(Screw screw)
    {
        _currentScrew = screw;
    }

    public void SelectThis(PlayerController playerController)
    {
        if (_currentScrew && !_currentScrew.IsUp)
            playerController.SelectScrew(_currentScrew);
        else
            playerController.SelectHole(this);
    }
    public void RemoveScrew()
    {
        _currentScrew = null;
    }
}
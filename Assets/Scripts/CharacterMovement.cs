using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    public GameObject grid;
    public Vector2Int currentLocation;
    public Vector2Int targetLocation;
    private List<GameObject> path;
    private int index = -1;

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (currentLocation != targetLocation)
        {
            grid.GetComponent<GridBehavior>().start = currentLocation;
            grid.GetComponent<GridBehavior>().end = targetLocation;
            grid.GetComponent<GridBehavior>().findDistance();
            path = grid.GetComponent<GridBehavior>().path;
            index = 0;
        }
        if (currentLocation != targetLocation && path.Count != 0)
        {
            currentLocation = path[index].GetComponent<GridStat>().location;
            transform.position = grid.GetComponent<GridBehavior>().GetTransformFromLocation(currentLocation).position;
            ++index;
        }
        if (path != null && path.Count == index)
        {
            index = -1;
            path.Clear();
        }
    }
}

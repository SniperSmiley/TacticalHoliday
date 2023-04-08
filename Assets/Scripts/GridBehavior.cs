using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0,0,0);
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;
    public bool findDistance = false;
    public List<GameObject> path = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPrefab)
            GenerateGrid();
        else
            print("missing gridprefab, please assign");
    }

    // Update is called once per frame
    void Update()
    {
        if (findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < columns; ++i)
        {
            for (int j = 0; j < rows; ++j)
            {
                GameObject obj = Instantiate(gridPrefab,new Vector3(leftBottomLocation.x + i * scale, leftBottomLocation.y, leftBottomLocation.z + j * scale), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStat>().x = i;
                obj.GetComponent<GridStat>().y = j;
                gridArray[i, j] = obj;
            }
        }
    }
    void SetDistance()
    {
        InitialSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for (int step = 1; step < rows * columns; ++step)
        {
            foreach (GameObject obj in gridArray)
            {
                if (obj && obj.GetComponent<GridStat>().visited == step - 1)
                    TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
            }
        }
    }
    void SetPath()
    {
        int step = gridArray[endX, endY].GetComponent<GridStat>().visited;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX,endY]&&gridArray[endX,endY].GetComponent<GridStat>().visited>=0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
        }
        else
        {
            print("Can't reach the desired location");
            return;
        }
        for (int i = step;i>=1; --i)
        {
            if (TestDirection(x, y, i, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, i, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, i, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, i, 4))
                tempList.Add(gridArray[x - 1, y]);
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStat>().x;
            y = tempObj.GetComponent<GridStat>().y;
            tempList.Clear();
        }
    }
    void InitialSetup()
    {
        foreach(GameObject obj in gridArray)
        {
            obj.GetComponent<GridStat>().visited = -1;
        }
        gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }
    bool TestDirection(int x, int y, int step, int direction)
    {
        switch(direction)
        {
            case 1:
                return y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step;
            case 2:
                return x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step;
            case 3:
                return y - 1 >= 0 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step;
            case 4:
                return x - 1 >= 0 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step;
        }
        return false;
    }
    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            setVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2))
            setVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3))
            setVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))
            setVisited(x - 1, y, step);
    }
    void setVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStat>().visited = step;

    }
    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;
        for( int i = 0; i < list.Count;++i)
        {
            float test = Vector3.Distance(targetLocation.position, list[i].transform.position);
            if (test < currentDistance)
            {
                currentDistance = test;
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
}

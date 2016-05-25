using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class GameLogic : MonoBehaviour
{
    public int row;
    public int column;
    int next;
    Transform canvas;
    Transform[,] imageArray;
    public Text nextNumberText;
    int[,] numberArray;
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        numberArray = new int[row, column];
        imageArray = new Transform[row, column];
        next = 1;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                int a = i;
                int b = j;
                GameObject g = Instantiate(Resources.Load<GameObject>("Image"));
                g.transform.SetParent(canvas, false);
                numberArray[i, j] = 0;
                imageArray[i, j] = g.transform;
                g.GetComponent<RectTransform>().anchoredPosition = new Vector2(100 + i * 55, 100 + j * 55);
                Button btn = g.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    click(btn.transform, a, b, next);
                    next = Random.Range(1, 3);
                });
            }
        }
    }
    void Update()
    {
        nextNumberText.text = next.ToString();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                imageArray[i, j].Find("Text").GetComponent<Text>().text = numberArray[i, j].ToString();
            }
        }
    }
    public void click(Transform btn, int x, int y, int n)
    {
        tileList.Clear();
        numberArray[x, y] = n;
        checkTile(x, y, n);
        while (tileList.Count >= 3)
        {
            n++;
            setNumber(tileList, x, y, n);
            tileList.Clear();
            checkTile(x, y, n);
        }
        Debug.Log("lt:" + tileList.Count);
    }
    void setNumber(List<Tile> lt, int x, int y, int n)
    {
        for (int i = 0; i < lt.Count; i++)
        {
            numberArray[lt[i].x, lt[i].y] = 0;
        }
        numberArray[x, y] = n;
    }
    [System.Serializable]
    public struct Tile
    {
        public int x;
        public int y;
        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public List<Tile> tileList = new List<Tile>();
    void checkTile(int x, int y, int n)
    {
        if (x < 0 || y < 0 || x >= column || y >= row)
        {
            return;
        }
        if (numberArray[x, y] == n)
        {
            if (!tileList.Contains(new Tile(x, y)))
            {
                tileList.Add(new Tile(x, y));
            }
        }
        if (x + 1 < column && numberArray[x + 1, y] == n)
        {
            if (!tileList.Contains(new Tile(x + 1, y)))
            {
                checkTile(x + 1, y, n);
            }
        }
        if (y + 1 < row && numberArray[x, y + 1] == n)
        {
            if (!tileList.Contains(new Tile(x, y + 1)))
            {
                checkTile(x, y + 1, n);
            }
        }
        if (x - 1 >= 0 && numberArray[x - 1, y] == n)
        {
            if (!tileList.Contains(new Tile(x - 1, y)))
            {
                checkTile(x - 1, y, n);
            }
        }
        if (y - 1 >= 0 && numberArray[x, y - 1] == n)
        {
            if (!tileList.Contains(new Tile(x, y - 1)))
            {
                checkTile(x, y - 1, n);
            }
        }
        return;
    }
}

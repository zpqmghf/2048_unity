using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxManager : MonoBehaviour
{
    private Box[,] boxes;
    private List<GameObject> boxesList;

    public Box[,] Boxes { get => boxes; set => boxes = value; }
    public List<GameObject> BoxesList { get => boxesList; set => boxesList = value; }

    public enum controllWay
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public bool keyBoardIsAllowed = true;
    public bool createNewBoxIsAllowed = false;
    void Start()
    {
        if (boxes == null)
        {
            boxes = new Box[4, 4];
            BoxesList = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    boxes[i, j] = new Box(i, j);
                }
            }
        }
        //createABoxAndAddToBoxes(0, 3, 4);
        //createABoxAndAddToBoxes(1, 3, 8);
        //createABoxAndAddToBoxes(2, 3, 2);
        //createABoxAndAddToBoxes(3, 3, 2);

    }

    public void createABoxAndAddToBoxes(int indexX, int indexY, int number)
    {
        GameObject box = new GameObject();
        box.name = "box_" + indexX + "_" + indexY;
        box.AddComponent<Image>();
        Texture2D txt2d = (Texture2D)Resources.Load("box_" + number);
        Image img = box.GetComponent<Image>();
        img.sprite = Sprite.Create(txt2d, new Rect(0, 0, txt2d.width, txt2d.height), new Vector2(0, 0));
        RectTransform rectTransform = box.GetComponent<RectTransform>();
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rectTransform.sizeDelta = new Vector2(165, 165);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        box.transform.SetParent(gameObject.transform);
        rectTransform.localPosition = new Vector3((float)82.5 + 8 + indexY % 4 * 173, (float)-82.5 - 8 - indexX % 4 * 173, 0);
        Box data = new Box(indexX, indexY, number);
        boxes[indexX, indexY] = data;
        box.transform.localScale = new Vector3(0, 0, 0);
        iTween.ScaleTo(box, iTween.Hash("x", 1.0f, "y", 1.0f, "speed", 5f, "easetype", iTween.EaseType.easeOutQuad));
        BoxesList.Add(box);
    }
    public void updateBoxes(controllWay type)
    {
        switch (type)
        {
            case controllWay.UP:
                mergeBoxWithUp();
                break;
            case controllWay.DOWN:
                mergeBoxWithDown();
                break;
            case controllWay.LEFT:
                mergeBoxWithLeft();
                break;
            case controllWay.RIGHT:
                mergeBoxWithRight();
                break;
        }
    }
    public void mergeBoxWithUp()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (boxes[i, j].HasNumber)  //遍历boxes，如果该格子有数字
                {
                    if (i > 0) //不是边界上的格子
                    {
                        int currIndex = i;
                        for (int k = i - 1; k >= 0; k--) //判断格子的上面有没有其他格子
                        {
                            if (boxes[k, j].HasNumber)
                            {
                                if (mergeTwoBoxes(k, j, currIndex, j) == true)
                                    moveBoxToTargetIndex(currIndex, j, k, j);
                                break;
                            }
                            moveBoxToTargetIndex(currIndex, j, k, j);
                            currIndex = k;
                        }
                    }
                }
            }
        }
    }
    public void mergeBoxWithLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (boxes[i, j].HasNumber)  //遍历boxes，如果该格子有数字
                {
                    if (j > 0) //不是边界上的格子
                    {
                        int currIndex = j;
                        for (int k = j - 1; k >= 0; k--) //判断格子的左面有没有其他格子
                        {
                            if (boxes[i, k].HasNumber)
                            {
                                if (mergeTwoBoxes(i, k, i, currIndex) == true)
                                    moveBoxToTargetIndex(i, currIndex, i, k);
                                break;
                            }
                            moveBoxToTargetIndex(i, currIndex, i, k);
                            currIndex = k;
                        }
                    }
                }
            }
        }
    }
    public void mergeBoxWithDown()
    {
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 3; j >= 0; j--)
            {
                if (boxes[i, j].HasNumber)  //遍历boxes，如果该格子有数字
                {
                    if (i < 3) //不是边界上的格子
                    {
                        int currIndex = i;
                        for (int k = i + 1; k <= 3; k++) //判断格子的下面有没有其他格子
                        {
                            if (boxes[k, j].HasNumber)
                            {
                                if (mergeTwoBoxes(k, j, currIndex, j) == true)
                                    moveBoxToTargetIndex(currIndex, j, k, j);
                                break;
                            }
                            moveBoxToTargetIndex(currIndex, j, k, j);
                            currIndex = k;
                        }
                    }
                }
            }
        }
    }
    public void mergeBoxWithRight()
    {
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 3; j >= 0; j--)
            {
                if (boxes[i, j].HasNumber)  //遍历boxes，如果该格子有数字
                {
                    if (j < 3) //不是边界上的格子
                    {
                        int currIndex = j;
                        for (int k = j + 1; k <= 3; k++) //判断格子的右边有没有其他格子
                        {
                            if (boxes[i, k].HasNumber)
                            {
                                if (mergeTwoBoxes(i, k, i, currIndex) == true)
                                    moveBoxToTargetIndex(i, currIndex, i, k);
                                break;
                            }
                            moveBoxToTargetIndex(i, currIndex, i, k);
                            currIndex = k;
                        }
                    }
                }
            }
        }
    }
    private bool mergeTwoBoxes(int indexX1, int indexY1, int indexX2, int indexY2) //合并格子，包括数据和显示
    {
        if (boxes[indexX1, indexY1].Number == boxes[indexX2, indexY2].Number)
        {
            //boxes[indexX1, indexY1].IsMerged = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void moveBoxToTargetIndex(int indexX, int indexY, int targetX, int targetY) //移动格子到指定位置
    {
        createNewBoxIsAllowed = true;
        GameObject box = findBoxObjectByName(indexX, indexY);
        Hashtable hash = new Hashtable();
        hash.Add("indexX", indexX);
        hash.Add("indexY", indexY);
        hash.Add("targetX", targetX);
        hash.Add("targetY", targetY);
        hash.Add("box", box);
        Vector3 vec = new Vector3((float)82.5 + 8 + targetY % 4 * 173, (float)-82.5 - 8 - targetX % 4 * 173, 0);
        iTween.MoveTo(box, iTween.Hash("position", vec, "time", 0.15, "islocal", true, "onstarttarget", gameObject, "oncomplete", "afterBoxMove", "oncompletetarget", gameObject, "oncompleteparams", hash, "easetype", iTween.EaseType.linear));
        if (boxes[targetX, targetY].HasNumber)
        {
            boxes[targetX, targetY].Number += boxes[indexX, indexY].Number;
            boxes[indexX, indexY].HasNumber = false;
            boxes[indexX, indexY].Number = 0;
            box.name = "destory_0_0";
        }
        else
        {
            boxes[targetX, targetY].Number = boxes[indexX, indexY].Number;
            boxes[targetX, targetY].HasNumber = true;
            boxes[indexX, indexY].HasNumber = false;
            boxes[indexX, indexY].Number = 0;
            box.name = "box_" + targetX + "_" + targetY;
        }
    }
    public List<Box> findAllFreeIndex()
    {
        List<Box> list = new List<Box>();
        foreach (Box box in boxes)
        {
            if (!box.HasNumber)
                list.Add(box);
        }
        return list;
    }
    private void afterBoxMove(Hashtable hash)
    {
        int indexX = (int)hash["indexX"];
        int indexY = (int)hash["indexY"];
        int targetX = (int)hash["targetX"];
        int targetY = (int)hash["targetY"];
        GameObject box = (GameObject)hash["box"];
        if (findBoxObjectByIndex(targetX, targetY) && findBoxObjectByIndex(targetX, targetY) != box)
        {
            GameObject.Destroy(box);
            BoxesList.Remove(box);
            GameObject box2 = findBoxObjectByIndex(targetX, targetY);
            int box2X = int.Parse(box2.name.Split('_')[1]);
            int box2Y = int.Parse(box2.name.Split('_')[2]);
            Texture2D txt2d = (Texture2D)Resources.Load("box_" + boxes[box2X, box2Y].Number);
            box2.GetComponent<Image>().sprite = Sprite.Create(txt2d, new Rect(0, 0, txt2d.width, txt2d.height), new Vector2(0, 0));
        }
        Invoke("openKeyBoard", 0.2f);
        createNewBoxIsAllowed = false;
    }
    public void openKeyBoard()
    {
        keyBoardIsAllowed = true;
    }
    public void stopKeyBoard()
    {
        keyBoardIsAllowed = false;
    }
    private GameObject findBoxObjectByName(int indexX, int indexY) //通过index找到box的gameObj
    {
        for (int i = 0; i < BoxesList.Count; i++)
        {
            if (int.Parse(BoxesList[i].name.Split('_')[1]) == indexX && int.Parse(BoxesList[i].name.Split('_')[2]) == indexY)
            {
                return BoxesList[i];
            }
        }
        return null;
    }
    private GameObject findBoxObjectByIndex(int indexX, int indexY) //通过index找到box的gameObj
    {
        Vector3 vec = convertIndexToPostion(indexX, indexY);
        for (int i = 0; i < BoxesList.Count; i++)
        {
            if (BoxesList[i].transform.localPosition == vec && BoxesList[i].name.IndexOf("destory") == -1)
            {
                return BoxesList[i];
            }
        }
        return null;
    }
    private Vector2 convertIndexToPostion(int indexX, int indexY)
    {
        return new Vector2((float)82.5 + 8 + indexY % 4 * 173, (float)-82.5 - 8 - indexX % 4 * 173);
    }
}

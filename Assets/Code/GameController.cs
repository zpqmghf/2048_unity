using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    BoxManager boxManager;
    enum slideVector { nullVector, up, down, left, right }; //上下左右四个方向
    private float offsetTime = 0.05f;
    private float timer;
    private Vector2 st = Vector2.zero; //手指开始按下的位置  
    private Vector2 end = Vector2.zero;
    private slideVector currentVector = slideVector.nullVector;
    private bool fingerDown = false;
    void Start()
    {
        GameObject gameObj = GameObject.Find("boxex");
        boxManager = gameObj.gameObject.GetComponent<BoxManager>();
        Invoke("findALocationAndAddABox", 0.1f);
        Invoke("findALocationAndAddABox", 0.1f);
    }
    void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown)//判断当前手指是按下事件  
        {
            st = Event.current.mousePosition;//记录开始按下的位置  
            fingerDown = true;
        }
        if (Event.current.type == EventType.MouseDrag)//判断当前手指是拖动事件  

        {
            timer += Time.deltaTime;  //计时器

            if (timer > offsetTime && fingerDown)
            {
                end = Event.current.mousePosition; //记录结束下的位置
                Vector2 slideDirection = end - st;
                float x = slideDirection.x;
                float y = slideDirection.y;

                if (x > 0 && x > Mathf.Abs(y))
                {
                    if (currentVector == slideVector.right) //判断当前方向
                    {
                        return;
                    }
                    boxManager.mergeBoxWithRight();
                    if (boxManager.createNewBoxIsAllowed)
                        Invoke("findALocationAndAddABox", 0.3f);
                    currentVector = slideVector.right; //设置方向
                }
                else if (x < 0 && Mathf.Abs(y) < -x)
                {
                    if (currentVector == slideVector.left)
                    {
                        return;
                    }
                    boxManager.mergeBoxWithLeft();
                    if (boxManager.createNewBoxIsAllowed)
                        Invoke("findALocationAndAddABox", 0.3f);
                    currentVector = slideVector.left;
                }
                else if (y < 0 && Mathf.Abs(x) < -y)
                {
                    if (currentVector == slideVector.up)
                    {
                        return;
                    }
                    boxManager.mergeBoxWithUp();
                    if (boxManager.createNewBoxIsAllowed)
                        Invoke("findALocationAndAddABox", 0.3f);
                    currentVector = slideVector.up;
                }
                else if (y > 0 && y > Mathf.Abs(x))
                {
                    if (currentVector == slideVector.down)
                    {
                        return;
                    }
                    boxManager.mergeBoxWithDown();
                    if (boxManager.createNewBoxIsAllowed)
                        Invoke("findALocationAndAddABox", 0.3f);
                    currentVector = slideVector.down;
                }
                timer = 0;
                st = end;//初始化位置
                fingerDown = false;

            }
        }
        if (Event.current.type == EventType.MouseUp)
        {
            currentVector = slideVector.nullVector;       //初始化方向
            fingerDown = false;
        }
    }
    public void findALocationAndAddABox()
    {
        List<Box> list = boxManager.findAllFreeIndex();
        Box box = list[Random.Range(0, list.Count)];
        boxManager.createABoxAndAddToBoxes(box.IndexX, box.IndexY, Random.Range(0, 2) < 0.5 ? 2 : 4);
    }
}

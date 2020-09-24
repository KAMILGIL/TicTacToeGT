using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldScript : MonoBehaviour
{
    public GameObject boxPrefab;

    public bool PlayerWin = false, BotWin = false;
    public Text label;

    [HideInInspector]
    public GameObject[,,] ar = new GameObject[3, 3, 3];

    private Vector3 delta = Vector3.back + Vector3.down + Vector3.left;

    private Vector3[] p0 = { 
        new Vector3(1, 1, 1) };
    private Vector3[] p1 = { 
        new Vector3(0, 0, 0), new Vector3(0, 0, 2), new Vector3(0, 2, 2), new Vector3(0, 2, 0),
        new Vector3(2, 0, 0), new Vector3(2, 2, 0), new Vector3(2, 0, 2), new Vector3(2, 2, 2) };
    private Vector3[] p2 = { 
        new Vector3(1, 0, 1), new Vector3(1, 2, 1),
        new Vector3(1, 1, 0), new Vector3(1, 1, 2),
        new Vector3(0, 1, 1), new Vector3(2, 1, 1)};
    private Vector3[] p3 = {
        new Vector3(1, 0, 0), new Vector3(1, 2, 0), new Vector3(1, 0, 2), new Vector3(1, 2, 2),
        new Vector3(0, 1, 0), new Vector3(2, 1, 0), new Vector3(2, 1, 2), new Vector3(0, 1, 2),
        new Vector3(0, 0, 1), new Vector3(0, 2, 1), new Vector3(2, 0, 1), new Vector3(2, 2, 1)};

    private Vector3 IndexToCoordinates(int i, int j, int k)
    {
        return new Vector3(i, j, k) + delta;
    }

    public void StartHidingBoxes()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    ar[i, j, k].GetComponent<BoxScript>().StartFading();
                }
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    ar[i, j, k] = Instantiate(boxPrefab, IndexToCoordinates(i, j, k), Quaternion.identity);
                    ar[i, j, k].GetComponent<BoxScript>().SetIndex(i, j, k); 
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RestartGame();
        }

        if (PlayerWin)
        {
            label.text = "You win";
            label.color = Color.green;
        } else if(BotWin)
        {
            label.text = "Bot win";
            label.color = Color.red; 
        } else
        {
            label.text = "Game in progress..";
            label.color = Color.black;
        }
    } 

    private void RestartGame() 
    { 
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    ar[i, j, k].GetComponent<BoxScript>().Clear();
                    PlayerWin = false; BotWin = false;
                }
            }
        }
    }

    public void MakePlayerMove(GameObject box)
    {
        if (PlayerWin || BotWin) { return; } 
        BoxScript boxScript = box.GetComponent<BoxScript>();
        if (boxScript.boxType != BoxType.None) { return; }
        boxScript.MakePlayerMove();

        this.CheckPlayerWin();
        if (PlayerWin || BotWin) { return; }

        this.MakeMove(); 
    }

    private void MakeMove()
    {
        if (CheckBotWin()) return;

        if (CheckBotSave()) return;

        if (CheckP(p0)) return;

        if (CheckP(p1)) return;

        if (CheckP(p2)) return;

        if (CheckP(p3)) return; 
    }

    private bool CheckP(Vector3[] p)
    {
        for (int i = 0; i < p.Length; i++)
        {
            if (CheckBoxNone(p[i]))
            {
                ar[(int)p[i].x, (int)p[i].y, (int)p[i].z].GetComponent<BoxScript>().MakeComputerMove();
                return true; 
            }
        }

        return false; 
    }

    private bool CheckCoordinates(float i, float j, float k)
    {
        if (i < 0 || i > 2 || j < 0 || j > 2 || k < 0 || k > 2)
        {
            return false; 
        }
        return true; 
    }

    private bool CheckBoxRed(Vector3 pos)
    {
        if (ar[(int)pos.x, (int)pos.y, (int)pos.z].GetComponent<BoxScript>().boxType == BoxType.Red) { return true; }
        return false; 
    }

    private bool CheckBoxBlue(Vector3 pos)
    {
        if (ar[(int)pos.x, (int)pos.y, (int)pos.z].GetComponent<BoxScript>().boxType == BoxType.Blue) { return true; }
        return false;
    }
    private bool CheckBoxNone(Vector3 pos)
    {
        if (ar[(int)pos.x, (int)pos.y, (int)pos.z].GetComponent<BoxScript>().boxType == BoxType.None) { return true; }
        return false;
    }


    private bool CheckBoxWinnes(int i, int j, int k)
    {
        Vector3 cur = new Vector3(i, j, k);
        Vector3 temp1 = Vector3.zero, temp2 = Vector3.zero; 
        for (int ix = -1; ix < 2; ix++)
        {
            for (int jx = -1; jx < 2; jx++)
            {
                for (int kx = -1; kx < 2; kx++)
                {
                    temp1 = new Vector3(i + ix, j + jx, k + kx);
                    temp2 = new Vector3(i - ix, j - jx, k - kx);

                    if (!(CheckCoordinates(temp1.x, temp1.y, temp1.z) && CheckCoordinates(temp2.x, temp2.y, temp2.z)))
                    {
                        
                    } 
                    else if (CheckBoxRed(temp1) && CheckBoxRed(temp2) && CheckBoxNone(cur) && !(ix == 0 && jx == 0 && kx == 0))
                    {
                        return true; 
                    }

                    temp1 = new Vector3(i + 2 * ix, j + 2 * jx, k + 2 * kx);
                    temp2 = new Vector3(i + ix, j + jx, k + kx);

                    //Debug.Log("cur: " + cur.ToString());
                    //Debug.Log(temp1); 

                    if (!(CheckCoordinates(temp1.x, temp1.y, temp1.z) && CheckCoordinates(temp2.x, temp2.y, temp2.z)))
                    {
                    }
                    else if (CheckBoxRed(temp1) && CheckBoxRed(temp2) && CheckBoxNone(cur) && !(ix == 0 && jx == 0 && kx == 0))
                    {
                        return true;
                    }
                }
            }
        }

        

        return false; 
    }

    private bool CheckBotWin()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (CheckBoxWinnes(i, j, k))
                    {
                        ar[i, j, k].GetComponent<BoxScript>().MakeComputerMove();
                        BotWin = true;
                        return true;
                    } 
                }
            }
        }
        return false; 
    }

    private bool CheckBoxSavenes(int i, int j, int k)
    {
        Vector3 cur = new Vector3(i, j, k);
        Vector3 temp1 = Vector3.zero, temp2 = Vector3.zero;
        for (int ix = -1; ix <= 1; ix++)
        {
            for (int jx = -1; jx <= 1; jx++)
            {
                for (int kx = -1; kx <= 1; kx++)
                {
                    temp1 = new Vector3(i + ix, j + jx, k + kx);
                    temp2 = new Vector3(i - ix, j - jx, k - kx);

                    if (!(CheckCoordinates(temp1.x, temp1.y, temp1.z) && CheckCoordinates(temp2.x, temp2.y, temp2.z)))
                    {
                    }
                    else if(CheckBoxBlue(temp1) && CheckBoxBlue(temp2) && CheckBoxNone(cur) && !(ix == 0 && jx == 0 && kx == 0))
                    {
                        return true;
                    }

                    temp1 = new Vector3(i + 2 * ix, j + 2 * jx, k + 2 * kx);
                    temp2 = new Vector3(i + ix, j + jx, k + kx);

                    if (!(CheckCoordinates(temp1.x, temp1.y, temp1.z) && CheckCoordinates(temp2.x, temp2.y, temp2.z)))
                    {
                    }
                    else if (CheckBoxBlue(temp1) && CheckBoxBlue(temp2) && CheckBoxNone(cur) && !(ix == 0 && jx == 0 && kx == 0))
                    {
                        return true;
                    }
                }
            }
        }



        return false;
    }

    private bool CheckBotSave()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (CheckBoxSavenes(i, j, k))
                    {
                        ar[i, j, k].GetComponent<BoxScript>().MakeComputerMove();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool CheckBoxWinnesForPlayer(int i, int j, int k)
    {
        Vector3 cur = new Vector3(i, j, k);
        Vector3 temp1 = Vector3.zero, temp2 = Vector3.zero;
        for (int ix = -1; ix <= 1; ix++)
        {
            for (int jx = -1; jx <= 1; jx++)
            {
                for (int kx = -1; kx <= 1; kx++)
                {
                    temp1 = new Vector3(i + ix, j + jx, k + kx);
                    temp2 = new Vector3(i - ix, j - jx, k - kx);

                    if (!(CheckCoordinates(temp1.x, temp1.y, temp1.z) && CheckCoordinates(temp2.x, temp2.y, temp2.z)))
                    {
                    }
                    else if (CheckBoxBlue(temp1) && CheckBoxBlue(temp2) && CheckBoxBlue(cur) && !(ix == 0 && jx == 0 && kx == 0))
                    {
                        return true;
                    }

                    temp1 = new Vector3(i + 2 * ix, j + 2 * jx, k + 2 * kx);
                    temp2 = new Vector3(i + ix, j + jx, k + kx);

                    if (!(CheckCoordinates(temp1.x, temp1.y, temp1.z) && CheckCoordinates(temp2.x, temp2.y, temp2.z)))
                    {
                    }
                    else if (CheckBoxBlue(temp1) && CheckBoxBlue(temp2) && CheckBoxBlue(cur) && !(ix == 0 && jx == 0 && kx == 0))
                    {
                        return true;
                    }
                }
            }
        }



        return false;
    }

    private bool CheckPlayerWin()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (CheckBoxWinnesForPlayer(i, j, k))
                    {
                        PlayerWin = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

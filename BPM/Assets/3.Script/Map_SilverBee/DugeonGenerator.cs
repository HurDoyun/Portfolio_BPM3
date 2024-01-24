using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DugeonGenerator : MonoBehaviour
{
    class Cell //�� �ϳ�
    {
        public bool visited = false;
        public bool[] status = new bool[4];
        
    }

    [System.Serializable]
    class Rule
    {
        public GameObject room;
        public Vector2Int minPos; // Vector2Int : int���� ����ϴ� ���Ͱ�. float�� ������.
        public Vector2Int maxPos;

        public bool obligatory; // oblicatory : (����) �ǹ�����

        public int ProbabailitySpawning(int x, int y)
        {
            /*
             0 - cannot spawn
             1 - can spawn
             2 - has to spawn
             */

            bool spawnRule = x >= minPos.x && x <= maxPos.x && 
                            y >= minPos.y && y <= maxPos.y;

            if (spawnRule)
            {
                return obligatory ? 2 : 1; // ���ǽ� ? ����_��_�� : ������_��_��
            }
            
            return 0;
        }
    }
    private void Start()
    {
        MazeGenerator();
    }
    public Vector2 size;
    public int StartPos = 0;
    List<Cell> board;

    [SerializeField] private Rule[] rooms;
    public Vector2 offset; // �� cell�� ����
    private void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];

                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for(int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabailitySpawning(i, j);

                        if (p == 2) // �ʼ����� �������� ��, ���������� �ʼ����� ���ÿ� �ִ´�.
                        {
                            randomRoom = k;
                            break;
                        }
                        else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }

                        //�� ���� �� shop�� �ϳ� �̻� ������ ������ shop�� �������.
                        
                    }
                    
                    if(randomRoom == -1) // �ʼ����� ���� ���
                    {
                        if(availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }

                    var newRoom =
                    Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y),
                    Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);

                    newRoom.name += " " + i + "-" + j; //�������� ������ �ο��޴� �̸�
                }
            }
        }
    }
    private void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = StartPos;
        Stack<int> path = new Stack<int>();
        int k = 0;

        while (k < 1000)
        {
            k++;
            board[currentCell].visited = true;

            if (currentCell == board.Count -1)
            {
                break;
            }

            //cell�� �̿��� �Ǻ�
            List<int> neighbors = CheckNeighbors(currentCell);

            if(neighbors.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if(newCell > currentCell)
                {
                    //Down or Right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //Up or Left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        GenerateDungeon();
    }
    
    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // ���� �̿� �Ǻ�
        // Mathf.FloorToInt - �Ҽ��� ù°�ڸ��� ����(�Ҽ��� ù°�ڸ� ���ϴ� ����)
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }

        // �Ʒ��� �̿� �Ǻ�
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }

        // ������ �̿� �Ǻ�
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        // ���� �̿� �Ǻ�
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }
    
}

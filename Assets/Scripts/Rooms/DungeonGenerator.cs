using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

public class DungeonGenerator : MonoBehaviour
{
    public enum RoomType
    {
        Undiscovered,
        DNE,
        Basic,
        Fountain,
        Entrance,
        Boss,
        BossKey,
        BossPreRoom,
        Treasure
    }

    [SerializeField] private int columns = 9;
    [SerializeField] private int rows = 5;
    [SerializeField] private RoomType[][] map;
    [SerializeField] int fountainRooms = 2;
    [SerializeField] private int bossKeyRooms = 1;
    [SerializeField] private int treasureRooms = 1;
    [SerializeField] private float xSpacing = 36;
    [SerializeField] private float zSpacing = 23;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private int maxRooms = 18;

    [SerializeField] private int roomsLeft;

    private bool mapDirty;

    void Start()
    {
    }
    
    void Update()
    {
        if (mapDirty)
        {
            GenerateMap();
            mapDirty = false;
        }
    }

    public void ClearRooms()
    {
        while(transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void GenerateRooms()
    {
        if (map == null)
            return;

        // Clear all rooms
        ClearRooms();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = transform.position + new Vector3((j - columns / 2) * xSpacing, 0, (-i + rows - 1) * zSpacing);// Invert i for Unity's coordinate system
                RoomType roomPrefab  = map[i][j];
                
                if (roomPrefab > RoomType.DNE)
                {
                    // var go = PrefabUtility.InstantiatePrefab(rooms[(int)roomPrefab]).GameObject();
                    // go.transform.position = position;
                    var go = Instantiate(rooms[(int)roomPrefab], position, quaternion.identity);
                    go.transform.parent = transform;

                    int pluggedDoors = GetPluggedDoors(i, j);
                    go.GetComponent<RoomManager>().PlugDoors(pluggedDoors);

                }
            }
        }
    }

    int GetPluggedDoors(int row, int col)
    {
        if (map[row][col] == RoomType.Boss) return 0b_1101;
        // 0000 = Right Left Bottom Top 
        return (!ValidRoom(row-1, col) || map[row-1][col] <= RoomType.DNE ? 0b_0001 : 0) +
                        (!ValidRoom(row+1, col) || map[row+1][col] <= RoomType.DNE || map[row+1][col] == RoomType.Boss ? 0b_0010 : 0) +
                        (!ValidRoom(row, col-1) || map[row][col-1] <= RoomType.DNE || map[row][col-1] == RoomType.Boss  ? 0b_0100 : 0) +
                        (!ValidRoom(row, col+1) || map[row][col+1] <= RoomType.DNE || map[row][col+1] == RoomType.Boss  ? 0b_1000 : 0);
    }

    bool ValidRoom(int row, int col)
    {
        return (row >= 0 && row < rows && col >= 0 && col < columns);
    }

    public void GenerateMap()
    {
        map = new RoomType[rows][];
        for (int i = 0; i < rows; i++)
        {
            map[i] = new RoomType[columns];
            for (int j = 0; j < columns; j++)
            {
                map[i][j] = RoomType.Undiscovered;
            }
        }
        
        // Set Entrance Point
        int startRow = rows - 1;
        int startCol = columns / 2;
        map[startRow][startCol] = RoomType.Entrance;
        // Set Boss Point
        map[0][startCol] = RoomType.Boss;
        // Set Boss Pre Room
        map[1][startCol] = RoomType.BossPreRoom;

        roomsLeft = maxRooms; 
        
        // Traverse Rooms
        TraverseRoom(startRow-1,startCol, startRow, startRow);
        TraverseRoom(startRow,startCol+1, startRow, startRow);
        TraverseRoom(startRow+1,startCol, startRow, startRow);
        TraverseRoom(startRow,startCol-1, startRow, startRow);
        
        // Insert Fountain Rooms
        for (int i = 0; i < fountainRooms; i++)
        {
            RandomInsertRoom(RoomType.Fountain);
        }
        
        // Insert Boss Key Rooms
        for (int i = 0; i < bossKeyRooms; i++)
        {
            RandomInsertRoom(RoomType.BossKey);
        }
        
        // Insert Boss Key Rooms
        for (int i = 0; i < treasureRooms; i++)
        {
            RandomInsertRoom(RoomType.Treasure);
        }
        
        print("Finished Dungeon Generation");
        // Update Gizmos
        mapDirty = true;
    }

    void RandomInsertRoom(RoomType newRoomType)
    {
        bool notInserted = true;
        while (notInserted)
        {
            int row = Random.Range(0, rows);
            int col = Random.Range(0, 2) == 0
                ? Random.Range(0, (columns / 2) - 1)
                : Random.Range((columns / 2) + 1, columns);

            if (map[row][col] == RoomType.Basic && !AdjacentIsSameRoom(row,col,newRoomType))
            {
                map[row][col] = newRoomType;
                notInserted = false;
            }
        }
    }

    bool AdjacentIsSameRoom(int row, int col, RoomType roomType)
    {
        return RoomTypeCheck(row + 1, col, roomType) ||
               RoomTypeCheck(row - 1, col, roomType) ||
               RoomTypeCheck(row, col + 1, roomType) ||
               RoomTypeCheck(row, col - 1, roomType);
    }

    bool RoomTypeCheck(int row, int col, RoomType roomType)
    {
        if (!ValidRoom(row,col)) return false;

        return map[row][col] == roomType;
    }

    void TraverseRoom(int row, int col, int prevRow, int prevCol)
    {
        if (roomsLeft <= 0) return;
        // Exit if called from this room
        if (row == prevRow && col == prevCol) return;
        // Exit if room is out of map
        if (!ValidRoom(row,col)) return;
        // Exit if room is discovered
        if (map[row][col] != RoomType.Undiscovered) return;
        
        RoomType type = GetRoomType(row,  col);
        map[row][col] = type;
        print("Traverse map[" + row + "][" + col +"] -> " + type.ToString());
        
        if (type == RoomType.DNE) return;
        --roomsLeft; // Count this room as it exists

        TraverseRoom(row-1,col, row, col);
        TraverseRoom(row,col+1, row, col);
        TraverseRoom(row+1,col, row, col);
        TraverseRoom(row,col-1, row, col);
    }

    RoomType GetRoomType(int row, int col)
    {
        if (col == columns / 2) return RoomType.Basic;

        return Random.Range(0, 3) < 2 ? RoomType.Basic : RoomType.DNE;
    }

    private void OnDrawGizmos()
    {
        if (map == null)
            return;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = transform.position + new Vector3(j, 0, -i) + new Vector3(-columns/2, 0,rows-1); // Invert i for Unity's coordinate system
                RoomType roomType = map[i][j];
                Gizmos.color = GetRoomColor(roomType);
                Gizmos.DrawCube(position, Vector3.one * 0.5f);
            }
        }
    }

    private Color GetRoomColor(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Basic:
                return Color.white;
            case RoomType.DNE:
                return Color.black;
            case RoomType.Fountain:
                return Color.blue;
            case RoomType.Entrance:
                return Color.green;
            case RoomType.Boss:
                return Color.red;
            case RoomType.BossKey:
                return Color.magenta;
            case RoomType.BossPreRoom:
                return Color.magenta;
            case RoomType.Treasure:
                return Color.cyan;;
            default:
                return Color.gray;
        }
    }

    [CustomEditor(typeof(DungeonGenerator))]
    public class DungeonGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DungeonGenerator generator = (DungeonGenerator)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Generate Map"))
            {
                generator.GenerateMap();
            }
            if (GUILayout.Button("Generate Rooms"))
            {
                generator.GenerateRooms();
            }
            if (GUILayout.Button("Clear Rooms"))
            {
                generator.ClearRooms();
            }
        }
    }
}
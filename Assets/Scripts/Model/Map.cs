using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {
    public TileType[,,] map;

    public Map(int size) {
        map = new TileType[size, size, size];
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                for (int z = 0; z < map.GetLength(2); z++) {
                    map[x, y, z] = TileType.Nothing;
                }
            }
        }
    }

    public bool IsTileType(int x, int y, int z, TileType tileType) {
        if (InsideMap(x, y, z)) {
            return map[x, y, z] == tileType;
        }
        return false;
    }

    public bool IsTileType(Vector3Int pos, TileType tileType) {
        return IsTileType(pos.x, pos.y, pos.z, tileType);
    }

    public TileType GetTileType(int x, int y, int z) {
        if (InsideMap(x, y, z)) {
            return map[x, y, z];
        }
        return TileType.Nothing;
    }

    public TileType GetTileType(Vector3Int pos) {
        return GetTileType(pos.x, pos.y, pos.z);
    }

    public void SetTileType(int x, int y, int z, TileType tileType) {
        if (InsideMap(x, y, z)) {
            map[x, y, z] = tileType;
        }
    }

    public void SetTileType(Vector3Int pos, TileType tileType) {
        SetTileType(pos.x, pos.y, pos.z, tileType);
    }

    public bool InsideMap(int x, int y, int z) {
        return x >= 0 && y >= 0 && z >= 0 && x < map.GetLength(0) && y < map.GetLength(1) && z < map.GetLength(2);
    }

    public bool InsideMap(Vector3Int pos) {
        return InsideMap(pos.x, pos.y, pos.z);
    }
}

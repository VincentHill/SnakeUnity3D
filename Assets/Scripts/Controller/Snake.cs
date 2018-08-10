using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Snake {
    public List<Vector3Int> bodyPositions;
    public Direction facingDirection = Direction.Forward;
    public Direction nextDirection = Direction.Forward;

    public Vector3Int nextPosition = new Vector3Int(0, 0, 1);

    public Snake (Vector3Int head) {
        bodyPositions = new List<Vector3Int>() {
            head,
           // head-head
        };
    }

    public void ChangeDirection(Direction dir) {
        switch (facingDirection) {
            case Direction.Forward:
                switch (dir) {
                    case Direction.Forward:
                        nextPosition = new Vector3Int(0, 0, 1);
                        nextDirection = Direction.Forward;
                        break;
                    case Direction.Right:
                        nextPosition = new Vector3Int(1, 0, 0);
                        nextDirection = Direction.Right;
                        break;
                    case Direction.Left:
                        nextPosition = new Vector3Int(-1, 0, 0);
                        nextDirection = Direction.Left;
                        break;
                }
                break;//-----------------------------------------------
            case Direction.Back:
                switch (dir) {
                    case Direction.Forward:
                        nextPosition = new Vector3Int(0, 0, -1);
                        nextDirection = Direction.Back;
                        break;
                    case Direction.Right:
                        nextPosition = new Vector3Int(-1, 0, 0);
                        nextDirection = Direction.Left;
                        break;
                    case Direction.Left:
                        nextPosition = new Vector3Int(1, 0, 0);
                        nextDirection = Direction.Right;
                        break;
                }
                break;//-----------------------------------------------
            case Direction.Right:
                switch (dir) {
                    case Direction.Forward:
                        nextPosition = new Vector3Int(1, 0, 0);
                        nextDirection = Direction.Right;
                        break;
                    case Direction.Right:
                        nextPosition = new Vector3Int(0, 0, -1);
                        nextDirection = Direction.Back;
                        break;
                    case Direction.Left:
                        nextPosition = new Vector3Int(0, 0, 1);
                        nextDirection = Direction.Forward;
                        break;
                }
                break;//-----------------------------------------------
            case Direction.Left:
                switch (dir) {
                    case Direction.Forward:
                        nextPosition = new Vector3Int(-1, 0, 0);
                        nextDirection = Direction.Left;
                        break;
                    case Direction.Right:
                        nextPosition = new Vector3Int(0, 0, 1);
                        nextDirection = Direction.Forward;
                        break;
                    case Direction.Left:
                        nextPosition = new Vector3Int(0, 0, -1);
                        nextDirection = Direction.Back;
                        break;
                }
                break;//-----------------------------------------------
        }


    }
}

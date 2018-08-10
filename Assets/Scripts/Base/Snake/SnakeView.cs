using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class SnakeView : View<SnakeApplication> {

    //UI
    public ViewUI UI { get { return m_UI = Assert<ViewUI>(m_UI); } }
    private ViewUI m_UI;

    public ViewMainMenu mainMenu { get { return m_mainMenu = Assert<ViewMainMenu>(m_mainMenu); } }
    private ViewMainMenu m_mainMenu;

    public ViewSettings settings { get { return m_optionsMenu = Assert<ViewSettings>(m_optionsMenu); } }
    private ViewSettings m_optionsMenu;

    public ViewGameUI gameUI { get { return m_gameUI = Assert<ViewGameUI>(m_gameUI); } }
    private ViewGameUI m_gameUI;

    public ViewGameOver gameOverScreen { get { return m_gameOverScreen = Assert<ViewGameOver>(m_gameOverScreen); } }
    private ViewGameOver m_gameOverScreen;

    //Sound
    public ViewSFX sfx { get { return m_sfx = Assert<ViewSFX>(m_sfx); } }
    private ViewSFX m_sfx;

    public ViewMusic music { get { return m_music = Assert<ViewMusic>(m_music); } }
    private ViewMusic m_music;

    //Cameras
    public View3DCamera _3DCamera { get { return m_3DCamera = Assert<View3DCamera>(m_3DCamera); } }
    private View3DCamera m_3DCamera;

    public View2DCamera _2DCamera { get { return m_2DCamera = Assert<View2DCamera>(m_2DCamera); } }
    private View2DCamera m_2DCamera;

    GameObject entities;
    GameObject snakeParent;
    List<GameObject> bodyParts = new List<GameObject>();
    GameObject[,,] entityMap;

    public void Init() {
        entities = new GameObject("Entities");
        snakeParent = new GameObject("Snake");
        entities.transform.SetParent(app.view.gameObject.transform);
        snakeParent.transform.SetParent(app.view.gameObject.transform);
    }

    public void ClearSnake() {
        for (int i = bodyParts.Count - 1; i >= 0; i--) {
            Destroy(bodyParts[i].gameObject);
        }
        bodyParts = new List<GameObject>();
    }

    public void UpdateWorldVisuals(Map map) {
        if (entityMap == null) {
            entityMap = new GameObject[map.map.GetLength(0), map.map.GetLength(1), map.map.GetLength(2)];
        }
        for (int x = 0; x < map.map.GetLength(0); x++) {
            for (int y = 0; y < map.map.GetLength(1); y++) {
                for (int z = 0; z < map.map.GetLength(2); z++) {
                    if (map.IsTileType(x, y, z, TileType.Nothing) && entityMap[x, y, z] != null) {
                        Destroy(entityMap[x, y, z]);
                    }

                    if (map.IsTileType(x, y, z, TileType.Food) && entityMap[x, y, z] == null) {
                        entityMap[x, y, z] = Instantiate(app.model.food, Offset(x, y, z), Quaternion.identity, entities.transform) as GameObject;

                    }
                }
            }
        }
    }

    public void UpdateSnake(Snake snake) {
        if (snake.bodyPositions.Count > bodyParts.Count) {
            int size = snake.bodyPositions.Count - bodyParts.Count;
            for (int i = 0; i < size; i++) {
                bodyParts.Add(Instantiate(app.model.snakeBody, Vector3.zero, Quaternion.identity, snakeParent.transform));
            }
        }

        for (int i = 0; i < snake.bodyPositions.Count; i++) {
            bodyParts[i].transform.position = Offset(snake.bodyPositions[i]);
        }
    }

    public Vector3 Offset(int x, int y, int z) {
        return new Vector3(x - 15 + 0.5f, y + 1, z - 15 + 0.5f);
    }

    public Vector3 Offset(Vector3Int pos) {
        return Offset(pos.x, pos.y, pos.z);
    }
}

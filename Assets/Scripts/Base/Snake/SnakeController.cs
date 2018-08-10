using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class SnakeController : Controller<SnakeApplication> {
    /// <summary>
    /// Handle notifications from the application.
    /// </summary>
    /// <param name="p_event"></param>
    /// <param name="p_target"></param>
    /// <param name="p_data"></param>
    public override void OnNotification(string p_event, Object p_target, params object[] p_data) {
        switch (p_event) {
            case "scene.start":
                Log("Scene [" + p_data[0] + "][" + p_data[1] + "] loaded");
                app.model.Init();
                app.view.music.Play(app.model.music, true);
                app.view.gameOverScreen.Hide();
                app.view.settings.Hide();
                app.view.gameUI.Hide();
                app.view.mainMenu.Show();
                break;

            case "Play@up":
                if (!app.model.gamePlaying) {
                    app.view.Init();
                    app.view.sfx.PlayOneshot(app.model.UIsfx);
                    app.view.mainMenu.Hide();
                    app.view.gameUI.Show();
                    NewGame();
                }
                else {
                    UnpauseGame();
                }
                break;

            case "OpenOptions@up":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
                app.model.LoadSettings();
                app.view.settings.Show();
                app.view.mainMenu.Hide();
                break;

            case "CloseOptions@up":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
                app.view.settings.Hide();
                app.view.mainMenu.Show();
                break;

            case "MusicVolume@change":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
                app.model.settings.snake.musicVolume = (float)p_data[0];
                app.model.SaveSettings();
                break;

            case "SFXVolume@change":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
                app.model.settings.snake.sfxVolume = (float)p_data[0];
                app.model.SaveSettings();
                break;

            case "Difficulty@change":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
                app.model.settings.snake.difficulty = 1 - (float)p_data[0];
                app.model.SaveSettings();
                break;

            case "Use3D@change":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
                app.model.settings.snake.use3D = (bool)p_data[0];
                app.model.SaveSettings();
                break;

            case "Restart@up":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
                app.view.ClearSnake();
                app.model.cycleCounter = 0;
                NewGame();
                app.view.gameOverScreen.Hide();
                app.view.gameUI.Show();
                break;

            case "Quit@up":
                app.view.sfx.PlayOneshot(app.model.UIsfx);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPaused = false;
                UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
                Application.Quit();
#endif
                break;

            case "GameOver":
                app.model.gamePlaying = false;
                app.view.gameUI.Hide();
                app.view.gameOverScreen.Show();
                app.view.gameOverScreen.score.text = string.Format("Score: {0}", (app.model.player.bodyPositions.Count - 1) * 100);
                break;
        }

    }

    public void MoveSnake() {
        Snake player = app.model.player;
        player.facingDirection = player.nextDirection;
        Vector3Int nextPos = player.bodyPositions[0] + player.nextPosition;
        if (app.model.map.InsideMap(nextPos)) {
            if (app.model.map.IsTileType(nextPos, TileType.Food)) {
                player.bodyPositions.Insert(1, player.bodyPositions[0]);
                app.model.map.SetTileType(nextPos, TileType.Nothing);
                app.model.foodOnMap = false;
            }
            else {
                for (int i = player.bodyPositions.Count - 1; i > 0; i--) {
                    player.bodyPositions[i] = player.bodyPositions[i - 1];
                }
            }

            if (player.bodyPositions.Any(i => i == nextPos)) {
                Notify("GameOver");
            }
            player.bodyPositions[0] = nextPos;
        }
        else {
            Notify("GameOver");
        }
    }

    public void IncrementCycleCounter() {
        app.model.cycleCounter++;
    }

    public void UpdateSnakeVisuals() {
        app.view.UpdateSnake(app.model.player);
    }

    public void UpdateWorldVisuals() {
        app.view.UpdateWorldVisuals(app.model.map);
    }

    public void RandomFoodSpawn() {
        if (!app.model.foodOnMap) {
            if (Random.value < 0.1) {
                Map map = app.model.map;
                int x = Random.Range(0, map.map.GetLength(0));
                int z = Random.Range(0, map.map.GetLength(2));
                Vector3Int pos = new Vector3Int(x, 0, z);
                if (!app.model.player.bodyPositions.Any(i => i == pos)) {
                    map.SetTileType(x, 0, z, TileType.Food);
                    app.model.foodOnMap = true;
                }
            }
        }
    }

    IEnumerator MainLoop() {
        float timer = 0;
        while (app.model.gamePlaying) {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                PauseGame();
            }
            if (!app.model.pause) {
                app.view.gameUI.score.text = string.Format("Score: {0}", (app.model.player.bodyPositions.Count - 1) * 100);
                if (Input.GetKeyDown(KeyCode.W)) {
                    app.model.player.ChangeDirection(Direction.Forward);
                }
                if (Input.GetKeyDown(KeyCode.A)) {
                    app.model.player.ChangeDirection(Direction.Left);
                }
                if (Input.GetKeyDown(KeyCode.D)) {
                    app.model.player.ChangeDirection(Direction.Right);
                }

                if (timer > app.model.cycleTime) {
                    timer = 0;
                    app.model.Cycle();
                }
                timer += Time.deltaTime;
            }
            yield return null;
        }
    }

    public void NewGame() {

        app.model.map = new Map(30);
        app.model.player = new Snake(new Vector3Int(15, 0, 15));
        UpdateSnakeVisuals();
        UpdateWorldVisuals();

        app.model.SubscribeToCycle(MoveSnake);
        app.model.SubscribeToCycle(UpdateSnakeVisuals);
        app.model.SubscribeToCycle(UpdateWorldVisuals);
        app.model.SubscribeToCycle(RandomFoodSpawn);
        app.model.SubscribeToCycle(IncrementCycleCounter);

        app.model.gamePlaying = true;
        app.model.foodOnMap = false;
        StartCoroutine(MainLoop());
    }

    public void PauseGame() {
        app.model.pause = true;
        app.view.mainMenu.Show();
    }

    public void UnpauseGame() {
        app.model.pause = false;
        app.view.mainMenu.Hide();
    }

    public bool[,,] RandomMap(Vector3Int size, int groundSize) {
        bool[,,] map = new bool[size.x, size.y, size.z];
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int z = 0; z < map.GetLength(2); z++) {
                int y = (int)(map.GetLength(1) * Mathf.PerlinNoise((float)x / (float)map.GetLength(0), (float)z / (float)map.GetLength(2)));
                map[x, y, z] = true;
            }
        }
        return map;
    }

}

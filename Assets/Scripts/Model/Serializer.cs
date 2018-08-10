using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serializer {
    public static class Utility {
        public static void DeleteFile(string path) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
        }

        public static string GetMainFilePath(string fileName, string fileExtension, params string[] path) {
            string mainFolderPath = GetMainFolderPath();

            for (int i = 0; i < path.Length; i++) {
                mainFolderPath += (path[i] + "/");
            }

            return mainFolderPath + (fileName + "." + fileExtension);
        }

        public static string GetMainFolderPath(params string[] path) {
            string temp = string.Empty;

#if UNITY_EDITOR

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.LinuxEditor) {
                string folderPath = Application.dataPath + "/";
                temp = folderPath.Substring(0, folderPath.Length - 7);
                for (int i = 0; i < path.Length; i++) {
                    temp += (path[i] + "/");
                }
                return temp;
            }
#endif


            if (!Application.isMobilePlatform) {
                temp = Application.dataPath + "/";
                for (int i = 0; i < path.Length; i++) {
                    temp += (path[i] + "/");
                }
                return temp;
            }
            else {
                temp = Application.persistentDataPath + "/";
                for (int i = 0; i < path.Length; i++) {
                    temp += (path[i] + "/");
                }
                return temp;
            }
        }

        public static bool DoesFileExist(string fullFilePath) {
            return File.Exists(fullFilePath);
        }

        public static void CreateFilePath(params string[] path) {
            if (path.Length > 0) {
                string mainFolderPath = GetMainFolderPath();

                foreach (string seg in path) {
                    mainFolderPath += "/" + path;
                }
                if (!Directory.Exists(mainFolderPath)) {
                    Directory.CreateDirectory(mainFolderPath).Create();
                }
            }
        }
    }
    // Example Use
    // BinarySerializer<Animal>.SerializeBinary(currentDog, "best Dog ever", "binaryfileExtensionOfSomeSort", false, "MyDogs", "Favourites");
    // Animal myFavouriteDog = BinarySerializer<class>.DeserializeBinary("best Dog ever", "binaryfileExtensionOfSomeSort", "MyDogs", "Favourites");
    public static class BinarySerializer<T> where T : class {
        public static void Serialize(T obj, string fileName, string fileExtension, bool copyDuplicate, params string[] path) {
            string mainFolderPath = Utility.GetMainFolderPath();

            for (int i = 0; i < path.Length; i++) {
                mainFolderPath += (path[i] + "/");
            }

            string filePath = mainFolderPath + (fileName + "." + fileExtension);

            if (!Directory.Exists(mainFolderPath)) {
                Directory.CreateDirectory(mainFolderPath).Create();
            }
            else if (File.Exists(filePath) && !copyDuplicate) {
                Utility.DeleteFile(filePath);
            }
            else if (File.Exists(filePath) && copyDuplicate) {
                int i = 0;
                while (true) {
                    filePath = mainFolderPath + (fileName + "_" + i.ToString() + "." + fileExtension);
                    if (!File.Exists(filePath)) {
                        break;
                    }
                    i++;
                }
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Create);
            bf.Serialize(stream, obj);
            stream.Close();
        }

        public static T Deserialize(string fileName, string fileExtension, params string[] path) {
            string filePath = Utility.GetMainFilePath(fileName, fileExtension, path);
            if (File.Exists(filePath) && Directory.Exists(Utility.GetMainFolderPath(path))) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);
                T data = bf.Deserialize(stream) as T;
                stream.Close();
                return data;
            }
            return null;
        }
    }

    public static class ByteSerializer {
        public static void Serialize(Byte[] bytes, string fileName, string fileExtension, bool copyDuplicate, params string[] path) {
            string mainFolderPath = Utility.GetMainFolderPath();

            for (int i = 0; i < path.Length; i++) {
                mainFolderPath += (path[i] + "/");
            }

            string filePath = mainFolderPath + (fileName + "." + fileExtension);

            if (!Directory.Exists(mainFolderPath)) {
                Directory.CreateDirectory(mainFolderPath).Create();
            }
            else if (File.Exists(filePath) && !copyDuplicate) {
                Utility.DeleteFile(filePath);
            }
            else if (File.Exists(filePath) && copyDuplicate) {
                int i = 0;
                while (true) {
                    filePath = mainFolderPath + (fileName + "_" + i.ToString() + "." + fileExtension);
                    if (!File.Exists(filePath)) {
                        break;
                    }
                    i++;
                }
            }
            System.IO.File.WriteAllBytes(filePath, bytes);
        }
    }

    public static class JSONSerializer<T> where T : class {
        public static void Serialize(T obj, string fileName, bool copyDuplicate, params string[] path) {
            string fileExtension = "json";
            string mainFolderPath = Utility.GetMainFolderPath();

            for (int i = 0; i < path.Length; i++) {
                mainFolderPath += (path[i] + "/");
            }

            string filePath = mainFolderPath + (fileName + "." + fileExtension);

            if (!Directory.Exists(mainFolderPath)) {
                Directory.CreateDirectory(mainFolderPath).Create();
            }
            else if (File.Exists(filePath) && !copyDuplicate) {
                Utility.DeleteFile(filePath);
            }
            else if (File.Exists(filePath) && copyDuplicate) {
                int i = 0;
                while (true) {
                    filePath = mainFolderPath + (fileName + "_" + i.ToString() + "." + fileExtension);
                    if (!File.Exists(filePath)) {
                        break;
                    }
                    i++;
                }
            }

            string jsonData = JsonUtility.ToJson(obj, true);
            File.WriteAllText(filePath, jsonData);
        }

        public static T Deserialize(string fileName, params string[] path) {
            string fileExtension = "json";
            string filePath = Utility.GetMainFilePath(fileName, fileExtension, path);
            if (File.Exists(filePath) && Directory.Exists(Utility.GetMainFolderPath(path))) {
                T data = JsonUtility.FromJson<T>(File.ReadAllText(filePath));
                return data;
            }
            return null;
        }
    }

    public static class Screenshot {
        public static void Capture(string name, bool addDate = false, int size = 1) {
            string filename = name;
            if (addDate) {
                filename = System.DateTime.Now.ToString("yyyy-M-d - H-m-s-FF ") + name;
            }
            ScreenCapture.CaptureScreenshot(Utility.GetMainFilePath(filename, "png", "screenshots"), size);
        }

        //		public static void Capture (string name, Camera camera, bool addDate = false, int size = 1)
        //		{
        //			string filename = name;
        //			if (addDate) {
        //				filename = System.DateTime.Now.ToString ("yyyy-M-d - H-m-s-FF") + name;
        //			}
        //
        //			if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.LinuxPlayer) {
        //				int width = Screen.width * size;
        //				int height = Screen.height * size;
        //
        //				RenderTexture renderTexture = new RenderTexture (width, height, 24);
        //				camera.targetTexture = renderTexture;
        //
        //				Texture2D screenshot = new Texture2D (width, height, TextureFormat.RGB24, false);
        //				camera.Render ();
        //				screenshot.ReadPixels (new Rect (0, 0, width, height), 0, 0);
        //
        //				camera.targetTexture = null;
        //				RenderTexture.active = null;
        //				MonoBehaviour.Destroy (renderTexture);
        //				ByteSerializer.Serialize (screenshot.EncodeToPNG (), filename, "png", false, "screenshots");
        //			} else {
        //				CaptureSimple (filename, Mathf.RoundToInt (size));
        //			}
        //		}
    }
}

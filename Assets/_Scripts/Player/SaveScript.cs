using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;

namespace IntoTheMystic.PlayerControl
{

    [Serializable]
    struct SerializableTransform
    {
        public float posX;
        public float posY;
        public float posZ;

        public float rotX;
        public float rotY;
        public float rotZ;
        public float rotW;

        public float scaleX;
        public float scaleY;
        public float scaleZ;

        public SerializableTransform(Transform transform)
        {
            posX = transform.position.x;
            posY = transform.position.y;
            posZ = transform.position.z;

            rotX = transform.rotation.x;
            rotY = transform.rotation.y;
            rotZ = transform.rotation.z;
            rotW = transform.rotation.w;

            scaleX = transform.localScale.x;
            scaleY = transform.localScale.y;
            scaleZ = transform.localScale.z;
        }

        public Transform Apply(Transform transform )
        {
            transform.position = new Vector3(posX, posY, posZ);
            transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

            return transform;
        }
    }



    [Serializable]
    struct SaveGameState
    {
        public string LevelName;
        public SerializableTransform playerTransform;
        public float playerHealth;
        public string LastCheckpointText;
    }

    public class SaveScript : MonoBehaviour
    {
        public void SaveGame(string saveName, int checkpointNum, string lastCheckPoint)
        {
            SaveGameState gameState = new SaveGameState();

            gameState.LevelName = SceneManager.GetActiveScene().name;
            gameState.playerTransform = new SerializableTransform(transform);
            gameState.playerHealth = GetComponent<HealthSystem>().HealthBar;
            gameState.LastCheckpointText = lastCheckPoint;

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Into The Mystic");
            Directory.CreateDirectory(folderPath);

            BinaryFormatter bf = new BinaryFormatter();
            string fileName = string.Format("savegame_{0}.dat", saveName);
            string filePath = Path.Combine(folderPath, fileName);
            FileStream file = File.Create(filePath);
            bf.Serialize(file, gameState);
            file.Close();
        }

        //fake change for update
        //check

    public void AutoSave(int checkpointNum, string lastCheckPoint)
        {
            SaveGameState gameState = new SaveGameState();

            gameState.LevelName = SceneManager.GetActiveScene().name;
            gameState.playerTransform = new SerializableTransform(transform);
            gameState.playerHealth = 100;
            gameState.LastCheckpointText = lastCheckPoint;

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Into The Mystic");
            Directory.CreateDirectory(folderPath);

            BinaryFormatter bf = new BinaryFormatter();
            string fileName = string.Format("savegame_{0}.dat", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            string filePath = Path.Combine(folderPath, fileName);
            FileStream file = File.Create(filePath);
            bf.Serialize(file, gameState);
            file.Close();
        }

        public void LoadGame()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Into The Mystic");
            string[] filePaths = Directory.GetFiles(folderPath, "*.dat");

            if (filePaths.Length == 0)
            {
                Debug.Log("No save files found!");
                return;
            }

            Array.Sort(filePaths);

            string latestFilePath = filePaths[filePaths.Length - 1];

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(latestFilePath, FileMode.Open);
            SaveGameState gameState = (SaveGameState)bf.Deserialize(file);
            file.Close();

            if (!SceneManager.GetActiveScene().name.Equals(gameState.LevelName))
                SceneManager.LoadScene(gameState.LevelName);
            gameObject.transform.position = new Vector3(gameState.playerTransform.posX, gameState.playerTransform.posY, gameState.playerTransform.posZ);
            gameObject.transform.rotation = new Quaternion(gameState.playerTransform.rotX, gameState.playerTransform.rotY, gameState.playerTransform.rotZ, gameState.playerTransform.rotW);
            gameObject.transform.localScale = new Vector3(gameState.playerTransform.scaleX, gameState.playerTransform.scaleY, gameState.playerTransform.scaleZ);
            Physics.SyncTransforms();

           
            GetComponent<HealthSystem>().HealthBar = 100;
            GameObject.Find("FPSPlayerUpdated/Main Camera/Canvas/Slider").GetComponent<UnityEngine.UI.Slider>().value = 100 ;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<SaveScript>().LoadGame();
            }
        }


        public void ContinueGame1()
        {
            Debug.Log("here2");
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Into The Mystic");
            string[] filePaths = Directory.GetFiles(folderPath, "*.dat");

            if (filePaths.Length == 0)
            {
                Debug.Log("No save files found!");
                return;
            }

            Array.Sort(filePaths);

            string latestFilePath = filePaths[filePaths.Length - 1];

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(latestFilePath, FileMode.Open);
            SaveGameState gameState = (SaveGameState)bf.Deserialize(file);
            file.Close();

            if (!SceneManager.GetActiveScene().name.Equals(gameState.LevelName))
                SceneManager.LoadScene(gameState.LevelName);

            Invoke("ContinueGame2", 0.1f);
            
        }
        private void ContinueGame2()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Into The Mystic");
            string[] filePaths = Directory.GetFiles(folderPath, "*.dat");

            if (filePaths.Length == 0)
            {
                Debug.Log("No save files found!");
                return;
            }

            Array.Sort(filePaths);

            string latestFilePath = filePaths[filePaths.Length - 1];

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(latestFilePath, FileMode.Open);
            SaveGameState gameState = (SaveGameState)bf.Deserialize(file);
            file.Close();

            GameObject player = GameObject.Find("FPSPlayerUpdated");
            player.transform.position = new Vector3(gameState.playerTransform.posX, gameState.playerTransform.posY, gameState.playerTransform.posZ);
            player.transform.rotation = new Quaternion(gameState.playerTransform.rotX, gameState.playerTransform.rotY, gameState.playerTransform.rotZ, gameState.playerTransform.rotW);
            player.transform.localScale = new Vector3(gameState.playerTransform.scaleX, gameState.playerTransform.scaleY, gameState.playerTransform.scaleZ);
            Physics.SyncTransforms();
            GameObject taskTrackerOpened = GameObject.Find("FPSPlayerUpdated/Main Camera/Canvas/Task Tracker Opened");
            taskTrackerOpened.transform.Find("Task Tracker BG/TaskText").gameObject.GetComponent<TMP_Text>().text = gameState.LastCheckpointText.Split(',')[1];
            GameObject taskTrackerCollapsed = GameObject.Find("FPSPlayerUpdated/Main Camera/Canvas/Task Tracker Collapsed");
            taskTrackerCollapsed.transform.Find("Task Tracker BG/TaskText").gameObject.GetComponent<TMP_Text>().text = gameState.LastCheckpointText.Split(',')[0];


            player.GetComponent<HealthSystem>().HealthBar = gameState.playerHealth;
            GameObject.Find("FPSPlayerUpdated/Main Camera/Canvas/Slider").GetComponent<UnityEngine.UI.Slider>().value = 100;


        }
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntoTheMystic.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        string NextSceneName;
        string chapterName;
        string DialogueText;
        AsyncOperation asyncLoad;

        bool isMainScene = true;

        typewriterUI dialogueTypeWriterComponent;
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }


        public void LoadChapterLoaderScreen(string data )
        {
            string[] myArray = data.Split(',');

            NextSceneName = myArray[0];
            chapterName= myArray[1];
            DialogueText = myArray[2];

            SceneManager.LoadScene("Chapter Load Screen", LoadSceneMode.Single);
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(NextSceneName, LoadSceneMode.Single);
        }

        private void Update()
        {
            if (!isMainScene)
            {
                if (dialogueTypeWriterComponent.WritingDone)
                {
                    if (Input.anyKeyDown)
                    {
                        LoadNextScene();
                    }
                }
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Chapter Load Screen")
            {
                isMainScene = false;
            }
            else
                isMainScene = true;


            if (!isMainScene)
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                if (canvasObject != null)
                {
                    GameObject name = canvasObject.transform.Find("Chapter Name").gameObject;

                    TMP_Text tx = name.GetComponent<TMP_Text>();
                    if (tx != null)
                        tx.text = chapterName;
                    canvasObject.transform.Find("Dialogue").gameObject.GetComponent<TMP_Text>().text = DialogueText;

                    Transform dialogueTransform = canvasObject.transform.Find("Dialogue");
                    if (dialogueTransform != null)
                    {
                        typewriterUI typewriterUI = dialogueTransform.gameObject.GetComponent<typewriterUI>();
                        if (typewriterUI != null)
                        {
                            dialogueTypeWriterComponent = typewriterUI;
                        }
                    }
                }

                //LoadScene();

            }

        }

    }
}

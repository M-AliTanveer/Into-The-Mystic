using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;


namespace PianoSceneLogic
{
    public class PianoScene : MonoBehaviour
    {
        public GameObject Player;
        public GameObject SpotLights;
        public GameObject ToyPiano;
        public GameObject CubePrefab;
        public Transform FallPosition;
        public GameObject dicePrefab;
        public GameObject Piano;
        public GameObject Theatre;
        public GameObject UIObject;
        public List<GameObject> chilhoodPrefabs;
        public GameObject directionalLight;
        public AudioClip[] childhoodClips;
        public AudioClip[] performanceClips;
        public AudioClip[] teenageClips;


        private bool cubeSpotlightTriggered = false;

        private int currentClipIndex = 0;
        private bool doOnce = false;
        private void Start()
        {
            StartCoroutine(StartSceneAudios(2f));
        }

        IEnumerator StartSceneAudios(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (currentClipIndex < childhoodClips.Length)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = childhoodClips[currentClipIndex];
                audioSource.Play();
                currentClipIndex++;
                StartCoroutine(StartSceneAudios(audioSource.clip.length));

                if(!doOnce)
                {
                    doOnce = true;
                    float totalLength = 0f;
                    for (int i = 0; i < childhoodClips.Length; i++)
                    {
                        totalLength+= childhoodClips[i].length;
                    }
                    totalLength = totalLength / 5;
                    for (int i = 0; i < chilhoodPrefabs.Count; i++)
                    {
                        StartCoroutine(ChildhoodNostalgia(i*totalLength));
                    }
                }
            }
            else
            {
                StartCoroutine(BringUpAudience(1));
                currentClipIndex = 0;
                doOnce = false;
                StartCoroutine(StartPerformanceAudios(4f));
            }
        }

        IEnumerator StartPerformanceAudios(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (currentClipIndex < performanceClips.Length)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = performanceClips[currentClipIndex];
                audioSource.Play();
                currentClipIndex++;
                StartCoroutine(StartPerformanceAudios(audioSource.clip.length));
            }
            else
            {
                StartCoroutine(HideAudience(1));
                currentClipIndex = 0;
                doOnce = false;
                StartCoroutine(StartMistakeAudios(4));
            }
        }

        IEnumerator StartMistakeAudios(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (currentClipIndex < teenageClips.Length)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = teenageClips[currentClipIndex];
                audioSource.Play();
                currentClipIndex++;
                StartCoroutine(StartMistakeAudios(audioSource.clip.length));
            }
            else
            {
                StartCoroutine(ShiftLights(2));
                currentClipIndex = 0;
            }
        }

        IEnumerator ShiftLights(float delay)
        {
            yield return new WaitForSeconds(delay);

            Light spotLight = SpotLights.transform.Find("Spot Light Cube").GetComponent<Light>();
            SpotLights.transform.Find("Spot Light Cube").GetComponent<AudioSource>().Play();
            spotLight.enabled = true;
            spotLight = SpotLights.transform.Find("Spot Light Piano").GetComponent<Light>();
            spotLight.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && cubeSpotlightTriggered)
            {
                BreakGround();               
            }
            
        }

        public void BreakGround()
        {
            GameObject fallPieceParent = GameObject.Find("Fall Pieces");
            
            for (int i = 0; i < fallPieceParent.transform.childCount; i++)
            {
                Animator animController = fallPieceParent.transform.GetChild(i).GetComponent<Animator>();
                animController.SetTrigger("Break");
            }
            fallPieceParent = GameObject.Find("Hanging Pieces");

            StartCoroutine(AttachUI(1));


            for (int i = 0; i < fallPieceParent.transform.childCount; i++)
            {
                Animator animController = fallPieceParent.transform.GetChild(i).GetComponent<Animator>();
                animController.SetTrigger("Break");
            }
            Piano.GetComponent<Animator>().enabled = false;
            ToyPiano.GetComponent<Animator>().enabled = false;
            Physics.gravity = new Vector3(0, 025f, 0);

        }

        IEnumerator AttachUI(float delay)
        {
            yield return new WaitForSeconds(delay);

            UIObject.transform.position = Player.transform.Find("Main Camera").Find("PickedObjectHolder").position;
            UIObject.transform.parent = Player.transform.Find("Main Camera");
            UIObject.transform.localPosition = new Vector3(-22.23f, -6.16f, 1.17f);
            UIObject.transform.localRotation = Quaternion.identity;
            UIObject.SetActive(true);
            directionalLight.GetComponent<Light>().enabled = true;

            StartCoroutine(PlayLogo(1));

        }

        IEnumerator PlayLogo(float delay)
        {
            yield return new WaitForSeconds(delay);
            UIObject.transform.Find("Video Player").gameObject.GetComponent<VideoPlayer>().Play();
        }
        
        public void CubeSpotlightReached()
        {
            if (!cubeSpotlightTriggered)
            {
                GameObject cube = Instantiate(CubePrefab, FallPosition.position, FallPosition.rotation);
                cube.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                cube = Instantiate(CubePrefab, FallPosition.position + new Vector3(0, 10, 5), FallPosition.rotation);
                cube.transform.localScale = new Vector3(2, 2, 2);

                cube = Instantiate(CubePrefab, FallPosition.position + new Vector3(0, -7, 2), FallPosition.rotation);
                int i;
                GameObject lightParent = GameObject.Find("SpotLights");
                for (i = 0; i < lightParent.transform.childCount; i++)
                {
                    GameObject light = lightParent.transform.GetChild(i).gameObject;
                    if (light.name.StartsWith("Spot Light Path"))
                    {
                        StartCoroutine(OpenSpotLight(i, light));
                    }

                }

                StartCoroutine(OpenSpotLight(i, lightParent.transform.Find("Spot Light Piano").gameObject));
                StartCoroutine(BringUpAltar(i));
                cubeSpotlightTriggered = true;
            }

        }

        IEnumerator OpenSpotLight(int delay, GameObject spotlight)
        {
            yield return new WaitForSeconds(delay);

            spotlight.GetComponent<Light>().enabled = true;
            spotlight.GetComponent<AudioSource>().Play();
        }

        IEnumerator BringUpAltar(float delay)
        {
            yield return new WaitForSeconds(delay);
            ToyPiano.GetComponent<Animator>().SetTrigger("Bring UP");

        }

        public IEnumerator BringUpAudience(float delay)
        {
            yield return new WaitForSeconds(delay);
            Animator animator = Piano.GetComponent<Animator>();
            animator.SetTrigger("MoveRight");

            animator = Theatre.GetComponent<Animator>();
            animator.SetTrigger("Bring UP");
        }

        public IEnumerator HideAudience(float delay)
        {
            yield return new WaitForSeconds(delay);
            Animator animator = Piano.GetComponent<Animator>();
            animator.SetTrigger("MoveLeft");

            animator = Theatre.GetComponent<Animator>();
            animator.SetTrigger("Bring Down");
        }


        IEnumerator DestructedCube(int delay, GameObject obj)
        {
            yield return new WaitForSeconds(delay);
            int cubesPerAxis = 5;
            for (int x = 0; x < cubesPerAxis; x++)
            {
                for (int y = 0; y < cubesPerAxis; y++)
                {
                    for (int z = 0; z < cubesPerAxis; z++)
                    {
                        CreateCube(new Vector3(x, y, z), obj);
                    }
                }
            }

        }


        void CreateCube(Vector3 clone_coord, GameObject obj)
        {

            Transform transform = obj.transform.GetChild(0).transform;
            Destroy(obj.transform.GetChild(0).gameObject);
            GameObject cube = Instantiate(dicePrefab, transform.position, transform.rotation);
            cube.transform.localScale = transform.localScale / 5;
            Vector3 firstcube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
            cube.transform.position = firstcube + Vector3.Scale(clone_coord, cube.transform.localScale);
            cube.GetComponent<Rigidbody>().useGravity = true;
            //cube.GetComponent<Rigidbody>().AddExplosionForce(200, transform.position,5);
            cube.GetComponent<Rigidbody>().AddForce(300f * transform.forward);
            Destroy(cube, 2);            
            Destroy(obj, 2);
        }

        IEnumerator ChildhoodNostalgia(float delay)
        {
            yield return new WaitForSeconds(delay);

            Transform playerTransform = Player.transform;

            Vector3 direction = playerTransform.forward;
            int randomIndex = Random.Range(0, chilhoodPrefabs.Count);
            float angle = 40 / 5 * chilhoodPrefabs.Count;
            if(chilhoodPrefabs.Count%2==0)
            {
                angle = angle * -1;
            }

            direction = Quaternion.Euler(0, angle, 0) * direction;

            GameObject prefab = chilhoodPrefabs[randomIndex];
            chilhoodPrefabs.RemoveAt(randomIndex);

            float minHeight = playerTransform.position.y;
            float maxHeight = minHeight + 10;
            float height = Random.Range(minHeight, maxHeight);

            Vector3 position = playerTransform.position + direction * 20;
            position.y = height;

            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            StartCoroutine(DestructedCube(7, obj));
        }
    }
}

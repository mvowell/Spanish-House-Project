using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainControlScript : MonoBehaviour {
    public GameObject[] allObjects;
    public GameObject[] generateUVs;
    public GameObject[] topObjects;
    public GameObject[] atticObjects;
    public GameObject[] thirdlevelObjects;
    public GameObject[] secondlevelObjects;
    public GameObject[] firstlevelObjects;
    public GameObject[] basementObjects;
    public Vector3 defaultRotation;
    public float lerpSpeed;
    public float xRotSpeed;
    public float yRotSpeed;
    public float zoomSpeed;
    public List<Room> rooms;
    public List<GameObject> roomObjects;
    public Vector3 topDefaultPos;
    public Vector3 atticDefaultPos;
    public Vector3 thirdDefaultPos;
    public Vector3 secondDefaultPos;
    public Vector3 firstDefaultPos;
    public Vector3 garageDefaultPos;
    public LayerMask firstFloor;
    public LayerMask secondFloor;
    public LayerMask thirdFloor;
    public string[] defaultName;
    public string[] defaultText;
    public GameObject nametext;
    public GameObject texttext;
    public Camera defaultCamera;
    string currentName;
    string currentText;

    bool isFocused;
    Room roomFocus;
    int currentLevel = 6;
    int index;
    Vector3 preferredPos;
    // Use this for initialization
    void Start () {
        currentName = defaultName[currentLevel];
        currentText = defaultText[currentLevel];
        rooms = new List<Room>();
        roomObjects = new List<GameObject>();
        preferredPos = topDefaultPos;
        this.gameObject.transform.eulerAngles = defaultRotation;
        for (int i = 0;i < generateUVs.Length;i++) {
            Mesh mesh = generateUVs[i].GetComponent<MeshFilter>().mesh;
            Vector2[] randomUVs = new Vector2[mesh.vertexCount];
            for (int j = 0;j < randomUVs.Length;j++) {
                randomUVs[j] = new Vector2(Random.value,Random.value);
            }
            mesh.uv = randomUVs;
        }
	}
	
	// Update is called once per frame
	void Update () {
        int levelPressed = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            levelPressed = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            levelPressed = 2;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            levelPressed = 3;
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            levelPressed = 4;
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            levelPressed = 5;
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            levelPressed = 6;
        }
        if (isFocused) {
            preferredPos = rooms[index].pos;
            currentName = rooms[index].roomName;
            currentText = rooms[index].text;

        } else {
            preferredPos = getLevelPosition();
            currentName = defaultName[currentLevel];
            currentText = defaultText[currentLevel];
        }
        
        if (levelPressed != 0) {
            preferredPos = topDefaultPos;
            currentLevel = 6;
            isFocused = false;
            for (int i = 0;i < allObjects.Length;i++) {
                allObjects[i].SetActive(true);
            }
            if (levelPressed < 6) {
                for (int i = 0;i < topObjects.Length;i++) {
                    topObjects[i].SetActive(false);
                    preferredPos = atticDefaultPos;
                    currentLevel = 5;
                }
            }
            if (levelPressed < 5) {
                for (int i = 0;i < atticObjects.Length;i++) {
                    atticObjects[i].SetActive(false);
                    preferredPos = thirdDefaultPos;
                    currentLevel = 4;
                }
            }
            if (levelPressed < 4) {
                for (int i = 0;i < thirdlevelObjects.Length;i++) {
                    thirdlevelObjects[i].SetActive(false);
                    preferredPos = secondDefaultPos;
                    currentLevel = 3;
                }
            }
            if (levelPressed < 3) {
                for (int i = 0;i < secondlevelObjects.Length;i++) {
                    secondlevelObjects[i].SetActive(false);
                    preferredPos = firstDefaultPos;
                    currentLevel = 2;
                }
            }
            if (levelPressed < 2) {
                for (int i = 0;i < firstlevelObjects.Length;i++) {
                    firstlevelObjects[i].SetActive(false);
                    preferredPos = garageDefaultPos;
                    currentLevel = 1;
                }
            }
            Debug.Log(currentLevel);
        }
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position,preferredPos,lerpSpeed * Time.deltaTime);
        float zoom = Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
        defaultCamera.transform.localPosition = defaultCamera.transform.localPosition.z - zoom > 0 ? defaultCamera.transform.localPosition + new Vector3(0,0,-zoom) : new Vector3(defaultCamera.transform.localPosition.x,defaultCamera.transform.localPosition.y,0);
        //defaultCamera.transform.position = defaultCamera.transform.position - (defaultCamera.transform.position - this.gameObject.transform.position).normalized * (Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed);
        if (Input.GetMouseButton(1) || (defaultCamera.transform.localPosition.z == 0))
        this.gameObject.transform.eulerAngles = this.gameObject.transform.eulerAngles + new Vector3(Input.GetAxis("Mouse Y") * yRotSpeed * Time.deltaTime,Input.GetAxis("Mouse X") * xRotSpeed * Time.deltaTime,0);
        if (Input.GetMouseButton(0)) {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(defaultCamera.ScreenPointToRay(Input.mousePosition),out hit,float.MaxValue,getLevel())) {
                Debug.Log("Hit");
                if (roomObjects.Contains(hit.collider.gameObject)) {
                    Debug.Log("Hit room");
                    if (rooms[roomObjects.IndexOf(hit.collider.gameObject)].level == currentLevel) {
                        Debug.Log("Hit room on current level");
                        index = roomObjects.IndexOf(hit.collider.gameObject);
                        isFocused = true;
                    } else
                        isFocused = false;
                } else
                    isFocused = false;
            } else
                isFocused = false;
        }
        nametext.GetComponent<Text>().text = currentName;
        texttext.GetComponent<Text>().text = currentText;
        if (defaultCamera.transform.localPosition.z == 0) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetAxis("Cancel") > 0)
            Application.Quit();

	}
    Vector3 getLevelPosition() {
        switch (currentLevel) {
            case 1:
            return garageDefaultPos;
            case 2:
            return firstDefaultPos;
            case 3:
            return secondDefaultPos;
            case 4:
            return thirdDefaultPos;
            case 5:
            return atticDefaultPos;
            case 6:
            return topDefaultPos;
            default:
            return topDefaultPos;

        }
    }
    LayerMask getLevel() {
        if (currentLevel == 2) {
            return firstFloor;
        } else if (currentLevel == 3) {
            return secondFloor;
        } else if (currentLevel == 4) {
            return thirdFloor;
        } else {
            return new LayerMask();
        }
    }
}

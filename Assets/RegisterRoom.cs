using UnityEngine;
using System.Collections;

public class RegisterRoom : MonoBehaviour{
    GameObject controller;
    public string roomName;
    public string text;
    Vector3 pos;
    GameObject obj;
    public int level;
	void Start () {
        controller = GameObject.Find("GameManager");
        obj = this.gameObject;
        pos = findMidPoint(this.gameObject.GetComponent<MeshFilter>().mesh.vertices);
        controller.GetComponent<MainControlScript>().rooms.Add(new Room(roomName,text,obj,pos,level));
        controller.GetComponent<MainControlScript>().roomObjects.Add(obj);
    }
    Vector3 findMidPoint(Vector3[] verteces) {
        float x = 0;
        float y = 0;
        float z = 0;
        for (int i = 0;i < verteces.Length;i++) {
            x += verteces[i].x;
            y += verteces[i].y;
            z += verteces[i].z;
        }
        x /= verteces.Length;
        y /= verteces.Length;
        z /= verteces.Length;
        return new Vector3(x,y,z);
    }
}

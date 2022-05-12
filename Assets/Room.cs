using UnityEngine;
public class Room{
    public string roomName;
    public string text;
    public GameObject roomObj;
    public Vector3 pos;
    public int level;
    public Room(string name, string text, GameObject roomObj, Vector3 pos, int level) {
        this.roomName = name;
        this.text = text;
        this.roomObj = roomObj;
        this.pos = pos;
        this.level = level;
    }
}

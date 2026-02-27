using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class PlayerMoves
{
    public string[] moves = new string[4];
}

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    public float speed = 5f;
    private string serverURL = "http://10.7.22.234:3000/getMoves";

    void Start()
    {
        StartCoroutine(PollServer());
    }

    IEnumerator PollServer()
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(serverURL);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                string[] moves = JsonHelper.FromJson<string>(json);

                for(int i = 0; i < players.Length; i++)
                {
                    if(string.IsNullOrEmpty(moves[i])) continue;

                    Vector3 move = Vector3.zero;
                    switch(moves[i])
                    {
                        case "up": move = Vector3.up; break;
                        case "down": move = Vector3.down; break;
                        case "left": move = Vector3.left; break;
                        case "right": move = Vector3.right; break;
                    }
                    players[i].transform.position += move * speed * Time.deltaTime * 20f;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        json = "{\"moves\":" + json + "}";
        PlayerMoves wrapper = JsonUtility.FromJson<PlayerMoves>(json);
        return (T[])(object)wrapper.moves;
    }
}
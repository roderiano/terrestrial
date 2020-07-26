using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string scene;
    public float[] position;
    public int memories;
    

    public PlayerData(GameObject player)
    {
        scene = SceneManager.GetActiveScene().name;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        PlayerController playerController = player.GetComponent<PlayerController>();
        memories = playerController.GetMemoriesAmount();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{

    private Transform player;
    [SerializeField]private GameObject commandSaveGameUI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) < 5f) 
        {
            commandSaveGameUI.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                SaveGame();
            }
        }   
        else
        {
            commandSaveGameUI.SetActive(false);
        }
    }

    private void SaveGame() 
    {
        PlayerController playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController; 
        playerController.SavePlayer();  

        SkillTreeManager skillTree = FindObjectOfType(typeof(SkillTreeManager)) as SkillTreeManager; 
        skillTree.SaveSkillTree();  
    }
}

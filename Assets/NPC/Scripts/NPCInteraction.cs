using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField]private List<string> dialogTokens = new List<string>();
    [SerializeField]private string npcName = "";
    [SerializeField]private GameObject dialogPanel = null;
    [SerializeField]private float distanceToEnableDialog = 10;
    [SerializeField]private NPCInteractionOption interactionOption = NPCInteractionOption.None;
    [SerializeField]private GameObject commandEnableDialogUI;

    private int dialogCount;
    private bool dialogActived;
    private Text dialogText;
    private Text npcNameText;
    private Transform player;
    private I18NManager tranlationManager;
    
    private PlayerController playerController;
    

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tranlationManager = (I18NManager)FindObjectOfType(typeof(I18NManager));    
        dialogText = dialogPanel.transform.Find("DialogText").GetComponent<Text>();
        npcNameText = dialogPanel.transform.Find("NpcNameText").GetComponent<Text>();
        playerController = (PlayerController)FindObjectOfType(typeof(PlayerController));
    }
    
    private void Update() 
    {
        DialogTrigger();
        StartCoroutine(DialogController());
    }

    void DialogTrigger() 
    {
        float distanceBetweenPlayerAndNPC = Vector2.Distance(transform.position, player.position);
        

        if(distanceBetweenPlayerAndNPC < distanceToEnableDialog && !dialogActived)
        {
            commandEnableDialogUI.SetActive(true);
            if(Input.GetKeyUp(KeyCode.E))
            {
                EnableDialog();
            }
        }
        else
            commandEnableDialogUI.SetActive(false);
    }

    IEnumerator DialogController()
    {
        if(dialogActived)
        {
            dialogText.text = tranlationManager.GetTranslation(dialogTokens[dialogCount]);

            if(Input.GetKeyUp(KeyCode.Space))
            {
                if(dialogCount < dialogTokens.Count - 1)
                {
                    dialogCount++;
                } 
                else
                {   
                    dialogPanel.SetActive(false);

                    // Verify if enable something
                    if(interactionOption != NPCInteractionOption.None)
                    {
                        switch(interactionOption) {
                            case NPCInteractionOption.SkillTree:
                               SkillTreeManager skillTreeManager = (SkillTreeManager)FindObjectOfType(typeof(SkillTreeManager));
                               yield return skillTreeManager.EnableSkillTree();
                            break;
                        }
                    } 

                    DisableDialog();
                }
            }
                
        }
    }

    void EnableDialog() 
    {
        dialogCount = 0;
        dialogActived = true;
        dialogPanel.SetActive(true);
        npcNameText.text = npcName.ToUpper();  
        playerController.SetStatus(PlayerStatus.Dialoguing);
    }

    void DisableDialog() 
    {
        dialogActived = false;
        playerController.SetStatus(PlayerStatus.Moving);
    }
}

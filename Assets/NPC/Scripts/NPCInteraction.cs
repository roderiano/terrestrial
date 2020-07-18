using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField]private List<string> dialogTokens = new List<string>();
    [SerializeField]private string npcName = "";
    [SerializeField]private GameObject dialogPanel = null;
    [SerializeField]private GameObject commandsGroup = null;
    [SerializeField]private float distanceToEnableDialog = 10;
    [SerializeField]private NPCInteractionOption interactionOption = NPCInteractionOption.None;

    private int dialogCount;
    private bool dialogActived;
    private Text dialogText;
    private Text npcNameText;
    private Transform player;
    private I18NManager tranlationManager;
    private GameObject commandNextDialogUI;
    private GameObject commandEnableDialogUI;
    

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tranlationManager = (I18NManager)FindObjectOfType(typeof(I18NManager));    
        dialogText = dialogPanel.transform.Find("dialogText").GetComponent<Text>();
        npcNameText = dialogPanel.transform.Find("npcNameText").GetComponent<Text>();
        commandNextDialogUI = commandsGroup.transform.Find("commandNextDialog").gameObject;
        commandEnableDialogUI = commandsGroup.transform.Find("commandEnableDialog").gameObject;
    }
    
    private void Update() 
    {
        DialogController();
        DialogTrigger();
    }

    void DialogTrigger() 
    {
        float distanceBetweenPlayerAndNPC = Vector2.Distance(transform.position, player.position);
        

        if(distanceBetweenPlayerAndNPC < distanceToEnableDialog && !dialogActived)
        {
            commandEnableDialogUI.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E))
            {
                EnableDialog();
            }
        }
        else
            commandEnableDialogUI.SetActive(false);
    }

    void DialogController()
    {
        if(dialogActived)
        {
            dialogText.text = tranlationManager.GetTranslation(dialogTokens[dialogCount]);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(dialogCount < dialogTokens.Count - 1)
                {
                    dialogCount++;
                } 
                else
                {
                    DisableDialog();
                    
                    // Verify if enable something
                    if(interactionOption != NPCInteractionOption.None)
                    {
                        switch(interactionOption) {
                            case NPCInteractionOption.SkillTree:
                               SkillTreeManager skillTreeManager = (SkillTreeManager)FindObjectOfType(typeof(SkillTreeManager));
                               skillTreeManager.EnableSkillTree();
                            break;
                        }
                    }
                    
                }
            }
                
        }
    }

    void EnableDialog() 
    {
        dialogCount = 0;
        dialogActived = true;
        dialogPanel.SetActive(true);
        npcNameText.text = npcName;  
        commandNextDialogUI.SetActive(true);
    }

    void DisableDialog() 
    {
        dialogCount = 0;
        dialogActived = false;
        dialogPanel.SetActive(false);
        commandNextDialogUI.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AskAIProblem : MonoBehaviour
{
    public bool Active = true;
    public Button AskBt;
    public GameObject ChatUI;
    
    // Start is called before the first frame update
    void Start()
    {
       
        if (AskBt != null)
        {
            AskBt.onClick.AddListener(AskProblem);
        }
    }

    private void AskProblem()
    {
        ChatUI.SetActive(true);
        PreInput.instance.Show();
        //PlayerMouseLock.instance.UnlockCursor();

    }
}

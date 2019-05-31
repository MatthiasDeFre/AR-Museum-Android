using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public MessageScannedGameObject MessageScannedGameObject;
    public MessageLockedGameObject  MessageLockedGameObject;

    public Dictionary<MessageType, string> Test;
    // Start is called before the first frame update
    void Start()
    {
        MessageScannedGameObject.Init();
        MessageLockedGameObject.Init();
    }
    public void ShowMessage(MessageType type, string[] messageInfo = null)
    {
        switch(type)
        {
            case MessageType.Scanned:
                {
                    MessageScannedGameObject.ShowMessage(messageInfo[0], messageInfo[1]);
                    break;
                }
            case MessageType.Locked:
                {
                    MessageLockedGameObject.ShowMessage();
                    break;
                }
        }
    }
}

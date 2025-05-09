using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum EPopupType
{
    UI_OptionPopup,
    UI_CreditPopup,
}
public class UI_PopupManager : BehaviourSingleton<UI_PopupManager>
{

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    public List<UI_Popup> PopupList;
    public void Open(EPopupType type)
    {
        Open(type.ToString());
    }

    private void Open(string TypeString)
    {
        UI_Popup popup = PopupList.Find(x => x.name == TypeString);
        if(popup != null)
        {
            popup.Open();
            Add(popup);
        }
    }

    public void Close()
    {
        Remove();
    }

    public bool IsEmpty()
    {
        return _popupStack.Count == 0;
    }

    private void Add(UI_Popup popup)
    {
        _popupStack.Push(popup);
    }

    private void Remove()
    {

        UI_Popup recentPopup = null;
        while(recentPopup == null && _popupStack.Count > 0)
        {
            UI_Popup popup = _popupStack.Pop();
            if(popup.gameObject.activeSelf == true)
            {
                recentPopup = popup;
                break;
            }
        }


        recentPopup?.Close();
    }
}

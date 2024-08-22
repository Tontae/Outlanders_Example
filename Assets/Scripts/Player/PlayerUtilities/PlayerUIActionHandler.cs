using System;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.UI
{
    public class PlayerUIActionHandler : MonoBehaviour
    {
        // public event Action OnUIInventory;
        // public event Action OnUIController;
        public event Action OnUIUnstuck;
        public event Action OnUILeftItem;
        public event Action OnUIRightItem;
        public event Action OnUISwapSkill;
        public event Action OnUISkill;
        // public event Action OnUIYesButton;
        // public event Action OnUINoButton;

        // public void OnPlayerUIInventory()
        // {
        //     OnUIInventory?.Invoke();
        // }

        // public void OnPlayerUIController()
        // {
        //     OnUIController?.Invoke();
        // }

        //done
        public void OnPlayerUIUnstuck()
        {
            OnUIUnstuck?.Invoke();
        }

        //done
        public void OnPlayerUILeftItem()
        {
            OnUILeftItem?.Invoke();
        }

        //done
        public void OnPlayerUIRightItem()
        {
            OnUIRightItem?.Invoke();
        }

        //done
        public void OnPlayerUISwapSkill()
        {
            OnUISwapSkill?.Invoke();
        }

        public void OnPlayerUISkill()
        {
            OnUISkill?.Invoke();
        }

        // public void OnPlayerUIYesButton()
        // {
        //     OnUIYesButton?.Invoke();
        // }

        // public void OnPlayerUINoButton()
        // {
        //     OnUINoButton?.Invoke();
        // }
    }

}

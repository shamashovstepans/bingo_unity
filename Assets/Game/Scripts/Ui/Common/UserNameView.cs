using BingoGame.Modules.User;
using TMPro;
using UnityEngine;
using Zenject;

namespace BingoGame.Ui.Common
{
    internal class UserNameView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _userNameText;
        
        [Inject] private readonly IUserNameProvider _userNameProvider;

        private void OnEnable()
        {
            _userNameText.text = _userNameProvider.UserName.Value;
            _userNameProvider.UserName.ValueChanged += OnUserNameChanged;
        }

        private void OnDisable()
        {
            _userNameProvider.UserName.ValueChanged -= OnUserNameChanged;
        }
        
        private void OnUserNameChanged(string newName)
        {
            _userNameText.text = newName;
        }
    }
}
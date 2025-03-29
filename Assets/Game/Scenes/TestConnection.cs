using System.Linq;
using System.Threading;
using BingoGame.Module;
using Game.Scenes;
using Game.Scenes.HttpClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class TestConnection : MonoBehaviour
{
    [SerializeField] private Button _testButton;
    [SerializeField] private TextMeshProUGUI _testText;

    [Inject] private IHttpClient _httpClient;
    [Inject] private IBingoModule _bingoModule;

    private void OnEnable()
    {
        _testButton.onClick.AddListener(OnTestButtonClicked);
    }

    private void OnDisable()
    {
        _testButton.onClick.RemoveListener(OnTestButtonClicked);
    }

    private async void OnTestButtonClicked()
    {
        // var url = "https://bingo-server-production-4652.up.railway.app/api/episodes";
        var url = "http://localhost:3000/api/login";
        var udid = SystemInfo.deviceUniqueIdentifier;
        await _bingoModule.LoginAsync(CancellationToken.None);


        // _testText.text = string.Join(',', result.data.user_name);
    }
}
using UnityEngine;
using Newtonsoft.Json;
using Zork.Common;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI CurrentLocationText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    

    void Awake()
    {
        TextAsset gameJsonAsset = Resources.Load<TextAsset>(ZorkGameFileAssetName);
        _game = JsonConvert.DeserializeObject<Game>(gameJsonAsset.text);
    }

    void Start()
    {
        _game.Player.LocationChanged += (sender, newLocation) => CurrentLocationText.text = newLocation.ToString();

        _game.Start(InputService, OutputService);
    }

    private void Update()
    {

        //if (_game.IsRunning == false)
        //{
        //    Application.Quit();
        //}
    }

    [SerializeField]
    private string ZorkGameFileAssetName = "Zork";

    private Game _game;
}

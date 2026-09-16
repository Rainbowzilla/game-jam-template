using PurrNet;
using UnityEngine;
using TMPro;

public class PlayerIdentifier : NetworkBehaviour
{
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private TextMeshProUGUI playerNumber;

    private Color[] playerColors =
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow
    };

    private SyncVar<int> playerIndex = new(0);
    private static int playerCount = 0;

    protected override void OnSpawned(bool asServer)
    {
        base.OnSpawned(asServer);

        if (asServer)
        {
            // Temporary/simple: assign based on spawn order later
            playerIndex.value = playerCount;

            playerCount = (playerCount + 1) % playerColors.Length;
        }

        ApplyColor(playerIndex.value);

        playerNumber.text = "PLAYER " + (playerIndex.value + 1);
    }

    private void ApplyColor(int index)
    {
        playerRenderer.material.color = playerColors[index];
    }
}

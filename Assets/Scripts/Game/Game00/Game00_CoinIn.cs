using UnityEngine;
using UnityEngine.UI;


public class Game00_CoinIn : MonoBehaviour
{
    public Num num_Coins;
    public Num num_StartCoins;
    public Image image_Coins;

    int playerId = 0;
    int coins;

    public Text coin_Text;
    public void Init(int no)
    {
        playerId = no;
        coins = FjData.g_Fj[playerId].Coins;
        Update_CoinIn();
    }
    void Update()
    {
        if (coins != FjData.g_Fj[playerId].Coins)
        {
            coins = FjData.g_Fj[playerId].Coins;

            Update_CoinIn();
        }
    }

    void Update_CoinIn()
    {
        if (num_Coins != null && num_StartCoins != null)
        {
            num_Coins.UpdateShow(coins);
            num_StartCoins.UpdateShow(Set.setVal.StartCoins);
        }
        if (coin_Text != null)
        {
            coin_Text.text = FjData.g_Fj[playerId].Coins.ToString("D2") + "/" + Set.setVal.StartCoins.ToString("D2");
        }
    }
}

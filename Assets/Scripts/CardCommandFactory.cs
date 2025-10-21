using UnityEngine;

public static class CardCommandFactory
{
    public static ICardCommand Create(CardElement element)
    {
        return element switch
        {
            CardElement.Fire => new FireCommand(),
            CardElement.Water => new WaterCommand(),
            CardElement.Air => new AirCommand(),
            CardElement.Light => new LightCommand(),
            CardElement.Dark => new DarkCommand(),
            CardElement.Chaos => new ChaosCommand(),
            _ => null
        };
    }
}

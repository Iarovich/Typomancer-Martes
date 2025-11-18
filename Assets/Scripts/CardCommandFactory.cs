using UnityEngine;

//Factory - centraliza la creación de comandos según el elemento
public static class CardCommandFactory
{
    //Factory - metodo que crea instancias de comandos
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

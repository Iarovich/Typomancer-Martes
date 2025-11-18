using UnityEngine;

//Command - implementaciones concretas que encapsulan acciones de cartas
public class FireCommand : ICardCommand
{
    public void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context)
    {
        enemy?.GetComponent<Health>()?.TakeDamage(data.power * 2);
        context.SpawnEffect(data);
    }
}

public class WaterCommand : ICardCommand
{
    public void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context)
    {
        enemy?.GetComponent<Health>()?.TakeDamage(data.power);
        enemy?.Slow(2f, 3f);
        context.SpawnEffect(data);
    }
}

public class AirCommand : ICardCommand
{
    public void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context)
    {
        handManager?.DrawToHand(); 
        context.SpawnEffect(data);
    }
}

public class LightCommand : ICardCommand
{
    public void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context)
    {
        playerHealth?.Heal(data.power);
        context.SpawnEffect(data);
    }
}

public class DarkCommand : ICardCommand
{
    public void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context)
    {
        enemy?.GetComponent<Health>()?.TakeDamage(data.power * 4);
        context.SpawnEffect(data);
    }
}

public class ChaosCommand : ICardCommand
{
    public void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context)
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: enemy?.GetComponent<Health>()?.TakeDamage(data.power * 2); break;
            case 1: enemy?.Slow(2f, 2f); break;
            case 2: handManager?.DrawToHand(); break;
            case 3: playerHealth?.Heal(data.power); break;
        }
        context.SpawnEffect(data);
    }
}

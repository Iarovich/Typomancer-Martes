using UnityEngine;

public interface ICardCommand
{
    void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context);
}

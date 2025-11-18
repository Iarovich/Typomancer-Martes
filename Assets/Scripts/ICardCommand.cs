using UnityEngine;

// Command - interfaz que define el contrato para encapsular acciones
public interface ICardCommand
{
    void Execute(Enemy enemy, Health playerHealth, HandManager handManager, CardData data, CardEffectHandler context);
}
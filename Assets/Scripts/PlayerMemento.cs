using UnityEngine;

public class PlayerMemento
{
    public int Health { get; private set; }
    public PlayerMemento(int health) 
    {
        this.Health = health;
    }
}

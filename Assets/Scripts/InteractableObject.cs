using UnityEngine;

public interface InteractableObject
{
    public abstract void beginInteraction(Player player);
    public abstract void endInteraction(Player player);
}

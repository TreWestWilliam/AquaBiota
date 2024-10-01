using UnityEngine;

public interface InteractableObject
{
    public abstract string getInteractText();
    public abstract Vector3 getPosition();
    public abstract void beginInteraction(Player player);
    public abstract void endInteraction(Player player, bool confirmed);
}

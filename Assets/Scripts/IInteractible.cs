public interface IInteractible
{
    bool CanInteract();
    void Interact(PlayerController player);
    string GetActionText(PlayerController player);
}

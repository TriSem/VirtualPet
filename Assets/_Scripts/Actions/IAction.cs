public interface IAction
{
    ActionStatus Status { get; }
    void UseAction(PetAgent agent);
    void Cancel();
    bool CanDoBoth(IAction other);
}
public interface IBehaviour
{
    BehaviorState Status { get; }
    void Use(PetAgent agent);
    void Cancel();
}
﻿public interface IBehaviour
{
    ActionStatus Status { get; }
    void Use(PetAgent agent);
    void Cancel();
}
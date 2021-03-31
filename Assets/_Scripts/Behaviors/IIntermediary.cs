using System.Collections.Generic;

public interface IIntermediary
{
    HashSet<InternalState> GetPredictedChanges();
}
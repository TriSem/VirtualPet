public static class IdGeneration
{
    // String hashing algorithm by Daniel J. Bernstein.
    public static uint Djb2(string str)
    {
        uint hash = 5381;
        foreach (char ch in str)
        {
            uint c = (byte)ch;
            hash = ((hash << 5) + hash) + c;
        }
        return hash;
    }
}

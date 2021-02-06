public static class IdGeneration
{
    // String hashing algorithm by Daniel J. Bernstein.
    public static ulong Djb2(string str)
    {
        ulong hash = 5381;
        foreach (char ch in str)
        {
            ulong c = (byte)ch;
            hash = ((hash << 5) + hash) + c;
        }
        return hash;
    }
}

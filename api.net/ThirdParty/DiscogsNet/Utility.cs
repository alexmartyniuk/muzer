using System.Collections.Generic;
using System.Text;

namespace DiscogsNet
{
    class Utility
    {
        public static int GetCombinedHashCode(params object[] objects)
        {
            int combinedHash = 271;
            for (int i = 0; i < objects.Length; ++i)
            {
                combinedHash *= 31;
                if (objects[i] != null)
                {
                    combinedHash += objects[i].GetHashCode();
                }
                else
                {
                    combinedHash += 271;
                }
            }
            return combinedHash;
        }
    }
}

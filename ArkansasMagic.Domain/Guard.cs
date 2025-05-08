using System;

namespace ArkansasMagic.Domain
{
    public static class Guard
    {
        public static void NotNull<T>(T item, string itemName)
            where T : class
        {
            if (item == null)
                throw new ArgumentNullException(itemName);
        }
    }
}

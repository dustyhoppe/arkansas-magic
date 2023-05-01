using System;

namespace ArkansasMagic.Domain.Clock
{
    public interface ISystemClock
    {
        DateTime UtcNow();
        long EpochNow();
        long UtcToEpochTime(DateTime dateTime);
    }
}

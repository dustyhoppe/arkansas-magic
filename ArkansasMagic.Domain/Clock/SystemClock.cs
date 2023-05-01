using System;

namespace ArkansasMagic.Domain.Clock
{
    public class SystemClock : ISystemClock
    {
        private readonly DateTime _epochStart = new DateTime(1970, 1, 1);

        public long EpochNow() => UtcToEpochTime(UtcNow());

        public DateTime UtcNow() => DateTime.UtcNow;

        public long UtcToEpochTime(DateTime dateTime) => (long)(dateTime - _epochStart).TotalSeconds;
    }
}

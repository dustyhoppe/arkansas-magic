namespace ArkansasMagic.Infrastructure.Data
{
    public class MySqlConstants
    {
        public class DataType
        {
            public const string Currency = "DECIMAL(13, 4)";
            public const string Coordinate = "DECIMAL(12, 6)";
            public const string Timestamp = "timestamp";
            public const string ConcurrencyToken = "BINARY(16)";
        }

        public class Defaults
        {
            public const string CurrentTimstamp = "CURRENT_TIMESTAMP";
        }
    }
}

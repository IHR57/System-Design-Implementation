using Microsoft.Extensions.Configuration;

namespace UrlShortener.Core.ServiceContracts
{
    public class SnowflakeIdGenerator : IUniqueIdGenerator
    {
        private static long lastTimestamp = 0L;
        private static readonly object syncLock = new object();
        private static long sequence = 0L;
        private const long workerIdBits = 5L;
        private const long datacenterIdBits = 5L;
        private const long maxWorkerId = -1L ^ (-1L << (int)workerIdBits);
        private const long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits);
        private const long sequenceBits = 12L;
        private const long workerIdShift = sequenceBits;
        private const long datacenterIdShift = sequenceBits + workerIdBits;
        private const long timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;
        private const long epoch = 1288834974657L; // Twitter Snowflake epoch

        private readonly long workerId;
        private readonly long datacenterId;

        public SnowflakeIdGenerator(IConfiguration configuration)
        {
            workerId = long.Parse(configuration["MachineId"]);
            datacenterId = long.Parse(configuration["DataCenterId"]);

            if (workerId > maxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"worker Id can't be greater than {maxWorkerId} or less than 0");
            }
            if (datacenterId > maxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id can't be greater than {maxDatacenterId} or less than 0");
            }
        }

        public long NextId()
        {
            lock (syncLock)
            {
                long timestamp = UnixTimestamp();
                if (timestamp < lastTimestamp)
                {
                    throw new Exception($"Clock moved backwards. Refusing to generate id for {lastTimestamp - timestamp} milliseconds");
                }
                if (timestamp == lastTimestamp)
                {
                    sequence = (sequence + 1) & ((1 << (int)sequenceBits) - 1);
                    if (sequence == 0)
                    {
                        timestamp = NextMillis(lastTimestamp);
                    }
                }
                else
                {
                    sequence = 0L;
                }
                lastTimestamp = timestamp;
                long id = ((timestamp - epoch) << (int)timestampLeftShift) |
                          (datacenterId << (int)datacenterIdShift) |
                          (workerId << (int)workerIdShift) |
                          sequence;
                return id;
            }
        }

        private long NextMillis(long lastTimestamp)
        {
            long timestamp = UnixTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = UnixTimestamp();
            }
            return timestamp;
        }

        private static long UnixTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}

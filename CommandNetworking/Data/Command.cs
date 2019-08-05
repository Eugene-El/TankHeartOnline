using CommandNetworking.Enums;
using System;

namespace CommandNetworking.Data
{
    [Serializable]
    public class Command
    {
        public OperationType OperationType { get; set; }
        public byte[] Data { get; set; }
    }
}

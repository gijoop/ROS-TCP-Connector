//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Std
{
    public class MInt8 : Message
    {
        public const string k_RosMessageName = "std_msgs/Int8";
        public override string RosMessageName => k_RosMessageName;

        public sbyte data;

        public MInt8()
        {
            this.data = 0;
        }

        public MInt8(sbyte data)
        {
            this.data = data;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            listOfSerializations.Add(new[]{(byte)this.data});

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            this.data = (sbyte)data[offset];;
            offset += 1;

            return offset;
        }

        public override string ToString()
        {
            return "MInt8: " +
            "\ndata: " + data.ToString();
        }
    }
}

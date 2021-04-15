//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Nav
{
    public class MGetMapResponse : Message
    {
        public const string k_RosMessageName = "nav_msgs/GetMap";
        public override string RosMessageName => k_RosMessageName;

        public MOccupancyGrid map;

        public MGetMapResponse()
        {
            this.map = new MOccupancyGrid();
        }

        public MGetMapResponse(MOccupancyGrid map)
        {
            this.map = map;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            listOfSerializations.AddRange(map.SerializationStatements());

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            offset = this.map.Deserialize(data, offset);

            return offset;
        }

        public override string ToString()
        {
            return "MGetMapResponse: " +
            "\nmap: " + map.ToString();
        }
    }
}

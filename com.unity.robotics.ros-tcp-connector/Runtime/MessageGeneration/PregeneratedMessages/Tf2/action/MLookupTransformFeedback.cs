//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Tf2
{
    public class MLookupTransformFeedback : Message
    {
        public const string k_RosMessageName = "tf2_msgs/LookupTransform";
        public override string RosMessageName => k_RosMessageName;


        public MLookupTransformFeedback()
        {
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {

            return offset;
        }

        public override string ToString()
        {
            return "MLookupTransformFeedback: ";
        }
    }
}

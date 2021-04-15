//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.ObjectRecognition
{
    public class MGetObjectInformationResponse : Message
    {
        public const string k_RosMessageName = "object_recognition_msgs-master/GetObjectInformation";
        public override string RosMessageName => k_RosMessageName;

        //  Extra object info 
        public MObjectInformation information;

        public MGetObjectInformationResponse()
        {
            this.information = new MObjectInformation();
        }

        public MGetObjectInformationResponse(MObjectInformation information)
        {
            this.information = information;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            listOfSerializations.AddRange(information.SerializationStatements());

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            offset = this.information.Deserialize(data, offset);

            return offset;
        }

        public override string ToString()
        {
            return "MGetObjectInformationResponse: " +
            "\ninformation: " + information.ToString();
        }
    }
}

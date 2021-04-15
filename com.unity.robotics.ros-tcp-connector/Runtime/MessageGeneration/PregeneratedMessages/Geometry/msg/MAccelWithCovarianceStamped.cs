//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;

namespace RosMessageTypes.Geometry
{
    public class MAccelWithCovarianceStamped : Message
    {
        public const string k_RosMessageName = "geometry_msgs/AccelWithCovarianceStamped";
        public override string RosMessageName => k_RosMessageName;

        //  This represents an estimated accel with reference coordinate frame and timestamp.
        public MHeader header;
        public MAccelWithCovariance accel;

        public MAccelWithCovarianceStamped()
        {
            this.header = new MHeader();
            this.accel = new MAccelWithCovariance();
        }

        public MAccelWithCovarianceStamped(MHeader header, MAccelWithCovariance accel)
        {
            this.header = header;
            this.accel = accel;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            listOfSerializations.AddRange(header.SerializationStatements());
            listOfSerializations.AddRange(accel.SerializationStatements());

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            offset = this.header.Deserialize(data, offset);
            offset = this.accel.Deserialize(data, offset);

            return offset;
        }

        public override string ToString()
        {
            return "MAccelWithCovarianceStamped: " +
            "\nheader: " + header.ToString() +
            "\naccel: " + accel.ToString();
        }
    }
}

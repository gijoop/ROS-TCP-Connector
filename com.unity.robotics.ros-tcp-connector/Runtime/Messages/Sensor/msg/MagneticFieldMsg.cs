//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Sensor
{
    [Serializable]
    public class MagneticFieldMsg : Message
    {
        public const string k_RosMessageName = "sensor_msgs/MagneticField";

        //  Measurement of the Magnetic Field vector at a specific location.
        // 
        //  If the covariance of the measurement is known, it should be filled in.
        //  If all you know is the variance of each measurement, e.g. from the datasheet,
        //  just put those along the diagonal.
        //  A covariance matrix of all zeros will be interpreted as "covariance unknown",
        //  and to use the data a covariance will have to be assumed or gotten from some
        //  other source.
        public Std.HeaderMsg header;
        //  timestamp is the time the
        //  field was measured
        //  frame_id is the location and orientation
        //  of the field measurement
        public Geometry.Vector3Msg magnetic_field;
        //  x, y, and z components of the
        //  field vector in Tesla
        //  If your sensor does not output 3 axes,
        //  put NaNs in the components not reported.
        public double[] magnetic_field_covariance;
        //  Row major about x, y, z axes
        //  0 is interpreted as variance unknown

        public MagneticFieldMsg()
        {
            this.header = new Std.HeaderMsg();
            this.magnetic_field = new Geometry.Vector3Msg();
            this.magnetic_field_covariance = new double[9];
        }

        public MagneticFieldMsg(Std.HeaderMsg header, Geometry.Vector3Msg magnetic_field, double[] magnetic_field_covariance)
        {
            this.header = header;
            this.magnetic_field = magnetic_field;
            this.magnetic_field_covariance = magnetic_field_covariance;
        }

        public static MagneticFieldMsg Deserialize(MessageDeserializer deserializer) => new MagneticFieldMsg(deserializer);

        private MagneticFieldMsg(MessageDeserializer deserializer)
        {
            this.header = Std.HeaderMsg.Deserialize(deserializer);
            this.magnetic_field = Geometry.Vector3Msg.Deserialize(deserializer);
            deserializer.Read(out this.magnetic_field_covariance, sizeof(double), 9);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.magnetic_field);
            serializer.Write(this.magnetic_field_covariance);
        }

        public override string ToString()
        {
            return "MagneticFieldMsg: " +
            "\nheader: " + header.ToString() +
            "\nmagnetic_field: " + magnetic_field.ToString() +
            "\nmagnetic_field_covariance: " + System.String.Join(", ", magnetic_field_covariance.ToList());
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}

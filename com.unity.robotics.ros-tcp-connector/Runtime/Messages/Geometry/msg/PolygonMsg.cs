//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Geometry
{
    [Serializable]
    public class PolygonMsg : Message
    {
        public const string k_RosMessageName = "geometry_msgs/Polygon";

        //  A specification of a polygon where the first and last points are assumed to be connected
        public Point32Msg[] points;

        public PolygonMsg()
        {
            this.points = new Point32Msg[0];
        }

        public PolygonMsg(Point32Msg[] points)
        {
            this.points = points;
        }

        public static PolygonMsg Deserialize(MessageDeserializer deserializer) => new PolygonMsg(deserializer);

        private PolygonMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.points, Point32Msg.Deserialize, deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.WriteLength(this.points);
            serializer.Write(this.points);
        }

        public override string ToString()
        {
            return "PolygonMsg: " +
            "\npoints: " + System.String.Join(", ", points.ToList());
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

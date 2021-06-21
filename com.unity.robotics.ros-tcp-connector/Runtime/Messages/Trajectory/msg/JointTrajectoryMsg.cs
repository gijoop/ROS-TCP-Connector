//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Trajectory
{
    [Serializable]
    public class JointTrajectoryMsg : Message
    {
        public const string k_RosMessageName = "trajectory_msgs/JointTrajectory";

        //  The header is used to specify the coordinate frame and the reference time for
        //  the trajectory durations
        public Std.HeaderMsg header;
        //  The names of the active joints in each trajectory point. These names are
        //  ordered and must correspond to the values in each trajectory point.
        public string[] joint_names;
        //  Array of trajectory points, which describe the positions, velocities,
        //  accelerations and/or efforts of the joints at each time point.
        public JointTrajectoryPointMsg[] points;

        public JointTrajectoryMsg()
        {
            this.header = new Std.HeaderMsg();
            this.joint_names = new string[0];
            this.points = new JointTrajectoryPointMsg[0];
        }

        public JointTrajectoryMsg(Std.HeaderMsg header, string[] joint_names, JointTrajectoryPointMsg[] points)
        {
            this.header = header;
            this.joint_names = joint_names;
            this.points = points;
        }

        public static JointTrajectoryMsg Deserialize(MessageDeserializer deserializer) => new JointTrajectoryMsg(deserializer);

        private JointTrajectoryMsg(MessageDeserializer deserializer)
        {
            this.header = Std.HeaderMsg.Deserialize(deserializer);
            deserializer.Read(out this.joint_names, deserializer.ReadLength());
            deserializer.Read(out this.points, JointTrajectoryPointMsg.Deserialize, deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.WriteLength(this.joint_names);
            serializer.Write(this.joint_names);
            serializer.WriteLength(this.points);
            serializer.Write(this.points);
        }

        public override string ToString()
        {
            return "JointTrajectoryMsg: " +
            "\nheader: " + header.ToString() +
            "\njoint_names: " + System.String.Join(", ", joint_names.ToList()) +
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

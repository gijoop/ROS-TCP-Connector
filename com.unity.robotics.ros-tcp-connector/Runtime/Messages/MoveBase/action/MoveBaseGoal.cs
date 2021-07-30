//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.MoveBase
{
    [Serializable]
    public class MoveBaseGoal : Message
    {
        public const string k_RosMessageName = "move_base_msgs/MoveBase";
        public override string RosMessageName => k_RosMessageName;

        public Geometry.PoseStampedMsg target_pose;

        public MoveBaseGoal()
        {
            this.target_pose = new Geometry.PoseStampedMsg();
        }

        public MoveBaseGoal(Geometry.PoseStampedMsg target_pose)
        {
            this.target_pose = target_pose;
        }

        public static MoveBaseGoal Deserialize(MessageDeserializer deserializer) => new MoveBaseGoal(deserializer);

        private MoveBaseGoal(MessageDeserializer deserializer)
        {
            this.target_pose = Geometry.PoseStampedMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.target_pose);
        }

        public override string ToString()
        {
            return "MoveBaseGoal: " +
            "\ntarget_pose: " + target_pose.ToString();
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

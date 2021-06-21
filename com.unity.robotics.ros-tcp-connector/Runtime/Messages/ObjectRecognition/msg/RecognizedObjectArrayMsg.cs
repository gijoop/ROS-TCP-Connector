//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;

namespace RosMessageTypes.ObjectRecognition
{
    [Serializable]
    public class RecognizedObjectArrayMsg : Message
    {
        public const string k_RosMessageName = "object_recognition_msgs/RecognizedObjectArray";

        // #################################################### HEADER ###########################################################
        public HeaderMsg header;
        //  This message type describes a potential scene configuration: a set of objects that can explain the scene
        public RecognizedObjectMsg[] objects;
        // #################################################### SEARCH ###########################################################
        //  The co-occurrence matrix between the recognized objects
        public float[] cooccurrence;

        public RecognizedObjectArrayMsg()
        {
            this.header = new HeaderMsg();
            this.objects = new RecognizedObjectMsg[0];
            this.cooccurrence = new float[0];
        }

        public RecognizedObjectArrayMsg(HeaderMsg header, RecognizedObjectMsg[] objects, float[] cooccurrence)
        {
            this.header = header;
            this.objects = objects;
            this.cooccurrence = cooccurrence;
        }

        public static RecognizedObjectArrayMsg Deserialize(MessageDeserializer deserializer) => new RecognizedObjectArrayMsg(deserializer);

        private RecognizedObjectArrayMsg(MessageDeserializer deserializer)
        {
            this.header = HeaderMsg.Deserialize(deserializer);
            deserializer.Read(out this.objects, RecognizedObjectMsg.Deserialize, deserializer.ReadLength());
            deserializer.Read(out this.cooccurrence, sizeof(float), deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.WriteLength(this.objects);
            serializer.Write(this.objects);
            serializer.WriteLength(this.cooccurrence);
            serializer.Write(this.cooccurrence);
        }

        public override string ToString()
        {
            return "RecognizedObjectArrayMsg: " +
            "\nheader: " + header.ToString() +
            "\nobjects: " + System.String.Join(", ", objects.ToList()) +
            "\ncooccurrence: " + System.String.Join(", ", cooccurrence.ToList());
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

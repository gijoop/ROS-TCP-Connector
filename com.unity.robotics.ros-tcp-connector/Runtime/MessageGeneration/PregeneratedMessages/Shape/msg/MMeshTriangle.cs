//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Shape
{
    public class MMeshTriangle : Message
    {
        public const string k_RosMessageName = "shape_msgs/MeshTriangle";
        public override string RosMessageName => k_RosMessageName;

        //  Definition of a triangle's vertices
        public uint[] vertex_indices;

        public MMeshTriangle()
        {
            this.vertex_indices = new uint[3];
        }

        public MMeshTriangle(uint[] vertex_indices)
        {
            this.vertex_indices = vertex_indices;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            
            Array.Resize(ref vertex_indices, 3);
            foreach(var entry in vertex_indices)
                listOfSerializations.Add(BitConverter.GetBytes(entry));

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            
            this.vertex_indices= new uint[3];
            for(var i = 0; i < 3; i++)
            {
                this.vertex_indices[i] = BitConverter.ToUInt32(data, offset);
                offset += 4;
            }

            return offset;
        }

        public override string ToString()
        {
            return "MMeshTriangle: " +
            "\nvertex_indices: " + System.String.Join(", ", vertex_indices.ToList());
        }
    }
}

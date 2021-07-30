using System;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

namespace Unity.Robotics.MessageVisualizers
{
    public class DefaultVisualizerTwistStamped : StampedDrawingVisualFactory<TwistStampedMsg>
    {
        public float thickness = 0.01f;
        public float lengthScale = 1.0f;
        public float sphereRadius = 1.0f;
        public GameObject origin;
        [SerializeField]
        Color m_Color;

        public override void Draw(BasicDrawing drawing, TwistStampedMsg message, MessageMetadata meta)
        {
            drawing.SetTFTrackingType(m_TFTrackingType, message.header);
            var orig = origin == null ? Vector3.zero : origin.transform.position;
            message.twist.Draw<FLU>(drawing, SelectColor(m_Color, meta), orig, lengthScale, sphereRadius, thickness);
        }

        public override Action CreateGUI(TwistStampedMsg message, MessageMetadata meta) => () =>
        {
            message.header.GUI();
            message.twist.GUI();
        };
    }
}

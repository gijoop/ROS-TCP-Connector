﻿using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Robotics.ROSTCPConnector.MessageGeneration
{
    public enum BatteryStateStatusConstants
    {
        POWER_SUPPLY_STATUS_UNKNOWN = 0,
        POWER_SUPPLY_STATUS_CHARGING = 1,
        POWER_SUPPLY_STATUS_DISCHARGING = 2,
        POWER_SUPPLY_STATUS_NOT_CHARGING = 3,
        POWER_SUPPLY_STATUS_FULL = 4
    };

    public enum BatteryStateHealthConstants
    {
        POWER_SUPPLY_HEALTH_UNKNOWN = 0,
        POWER_SUPPLY_HEALTH_GOOD = 1,
        POWER_SUPPLY_HEALTH_OVERHEAT = 2,
        POWER_SUPPLY_HEALTH_DEAD = 3,
        POWER_SUPPLY_HEALTH_OVERVOLTAGE = 4,
        POWER_SUPPLY_HEALTH_UNSPEC_FAILURE = 5,
        POWER_SUPPLY_HEALTH_COLD = 6,
        POWER_SUPPLY_HEALTH_WATCHDOG_TIMER_EXPIRE = 7,
        POWER_SUPPLY_HEALTH_SAFETY_TIMER_EXPIRE = 8
    };

    public enum BatteryStateTechnologyConstants
    {
        POWER_SUPPLY_TECHNOLOGY_UNKNOWN = 0,
        POWER_SUPPLY_TECHNOLOGY_NIMH = 1,
        POWER_SUPPLY_TECHNOLOGY_LION = 2,
        POWER_SUPPLY_TECHNOLOGY_LIPO = 3,
        POWER_SUPPLY_TECHNOLOGY_LIFE = 4,
        POWER_SUPPLY_TECHNOLOGY_NICD = 5,
        POWER_SUPPLY_TECHNOLOGY_LIMN = 6
    };

    public enum JoyFeedbackTypes
    {
        TYPE_LED    = 0,
        TYPE_RUMBLE = 1,
        TYPE_BUZZER = 2,
    };

    public enum JoyLayout
    {
        DS4 = 0,
        XB360Windows = 1,
        XB360Linux = 2,
        XB360Wired = 3,
        F710 = 4
    };
    
    public enum JoyRegion 
    {
        BSouth = 0,
        BEast = 1,
        BWest = 2,
        BNorth = 3,
        LB = 4,
        RB = 5,
        Back = 6,
        Start = 7,
        Power = 8,
        LPress = 9,
        RPress = 10, 
        LStick, RStick, LT, RT, DPad, lAxisX, lAxisY, 
        rAxisX, rAxisY, ltAxis, rtAxis, dAxisX, dAxisY
    };

    public enum NavSatStatuses
    {
        STATUS_NO_FIX =  -1,        // unable to fix position
        STATUS_FIX =      0,        // unaugmented fix
        STATUS_SBAS_FIX = 1,        // with satellite-based augmentation
        STATUS_GBAS_FIX = 2         // with ground-based augmentation
    };

    public enum NavSatStatusServices
    {
        SERVICE_GPS =     1,
        SERVICE_GLONASS = 2,
        SERVICE_COMPASS = 4,      // includes BeiDou.
        SERVICE_GALILEO = 8
    };

    public enum NavSatFixCovariance
    {
        COVARIANCE_TYPE_UNKNOWN = 0,
        COVARIANCE_TYPE_APPROXIMATED = 1,
        COVARIANCE_TYPE_DIAGONAL_KNOWN = 2,
        COVARIANCE_TYPE_KNOWN = 3
    };

    public enum RangeRadiationTypes
    {
        ULTRASOUND = 0,
        INFRARED = 1
    };

    public enum PointFieldFormat
    {
        INT8    = 1,
        UINT8   = 2,
        INT16   = 3,
        UINT16  = 4,
        INT32   = 5,
        UINT32  = 6,
        FLOAT32 = 7,
        FLOAT64 = 8
    }

    // Convenience functions for built-in message types
    public static class MessageExtensions
    {
        public static string ToTimestampString(this MTime message)
        {
            // G: format using short date and long time
            return message.ToDateTime().ToString("G") + $"(+{message.nsecs})";
        }

        public static DateTime ToDateTime(this MTime message)
        {
            DateTime time = new DateTime(message.secs);
            time = time.AddMilliseconds(message.nsecs / 1E6);
            return time;
        }

        public static MTime ToMTime(this DateTime dateTime, uint nsecs = 0)
        {
            return new MTime { secs = (uint)dateTime.Ticks, nsecs = (uint)(dateTime.Millisecond * 1E6) };
        }

        public static Color ToUnityColor(this MColorRGBA message)
        {
            return new Color(message.r, message.g, message.b, message.a);
        }

        public static MColorRGBA ToMColorRGBA(this Color color)
        {
            return new MColorRGBA(color.r, color.g, color.b, color.a);
        }

        static Dictionary<int, int> channelConversion = new Dictionary<int, int>() 
        {
            { 0, 2 }, // B -> R 
            { 1, 1 }, // G -> G
            { 2, 0 }, // R -> B
            { 3, 3 }  // A -> A  
        };

        /// <summary>
        /// Converts a byte array from BGR to RGB.
        /// </summary>
        public static byte[] EncodingConversion(byte[] toConvert, string from, int width, int height, bool convert, bool flipY)
        {
            // No modifications necessary; return original array
            if (!convert && !flipY)
                return toConvert;

            // Set number of channels to calculate conversion offsets
            int channels = 3;

            if (from[from.Length - 1] == '1' || from.Contains("mono") || from.Contains("bayer"))
            {
                channels = 1;
            }
            else if (from[from.Length - 1] == '4' || from.Contains("a"))
            {
                channels = 4;
            }

            int idx = 0;
            int pixel = 0;
            int flipIdx;
            int fromIdx;
            int tmpR;
            int tmpC;
            int tmpH = height - 1;
            int tmpW = width * channels;

            byte[] converted = new byte[toConvert.Length];

            // Bit shift BGR->RGB and flip across X axis
            for (int i = 0; i < toConvert.Length; i++)
            {
                pixel = i / channels;
                tmpR = tmpH - (i / tmpW);
                tmpC = i % tmpW;
                flipIdx = (flipY) ? ((tmpR * tmpW) + tmpC) : i;
                if (channels > 1)
                    fromIdx = (convert) ? pixel * channels + channelConversion[idx] : pixel * channels + idx;
                else
                    fromIdx = pixel * channels;

                converted[flipIdx] = toConvert[fromIdx];
                idx = (idx + 1) % channels;
            }

            return converted;
        }

        public static TextureFormat EncodingToTextureFormat(this string encoding)
        {
            switch (encoding)
            {
                case "8UC1":
                    return TextureFormat.R8;
                case "8UC2":
                    return TextureFormat.RG16;
                case "8UC3":
                    return TextureFormat.RGB24;
                case "8UC4":
                    return TextureFormat.RGBA32;
                case "8SC1":
                    return TextureFormat.R8;
                case "8SC2":
                    return TextureFormat.RG16;
                case "8SC3":
                    return TextureFormat.RGB24;
                case "8SC4":
                    return TextureFormat.RGBA32;
                case "16UC1":
                    return TextureFormat.R16;
                case "16UC2":
                    return TextureFormat.RG32;
                case "16UC3":
                    return TextureFormat.RGB48;
                case "16UC4":
                    return TextureFormat.RGBA64;
                case "16SC1":
                    return TextureFormat.R16;
                case "16SC2":
                    return TextureFormat.RG32;
                case "16SC3":
                    return TextureFormat.RGB48;
                case "16SC4":
                    return TextureFormat.RGBA64;
                case "32SC1":
                    throw new NotImplementedException();
                case "32SC2":
                    throw new NotImplementedException();
                case "32SC3":
                    throw new NotImplementedException();
                case "32SC4":
                    throw new NotImplementedException();
                case "32FC1":
                    return TextureFormat.RFloat;
                case "32FC2":
                    return TextureFormat.RGFloat;
                case "32FC3":
                    throw new NotImplementedException();
                case "32FC4":
                    return TextureFormat.RGBAFloat;
                case "64FC1":
                    return TextureFormat.RGB24;
                case "64FC2":
                    return TextureFormat.RGB24;
                case "64FC3":
                    return TextureFormat.RGB24;
                case "64FC4":
                    return TextureFormat.RGB24;
                case "mono8":
                    return TextureFormat.R8;
                case "mono16":
                    return TextureFormat.R16;
                case "bgr8":
                    return TextureFormat.RGB24;
                case "rgb8":
                    return TextureFormat.RGB24;
                case "bgra8":
                    return TextureFormat.RGBA32;
                case "rgba8":
                    return TextureFormat.RGBA32;
                case "bayer_rggb8":
                    return TextureFormat.R8;
                case "bayer_bggr8":
                    return TextureFormat.R8;
                case "bayer_gbrg8":
                    return TextureFormat.R8;
                case "bayer_grbg8":
                    return TextureFormat.R8;
                case "bayer_rggb16":
                    return TextureFormat.R16;
                case "bayer_bggr16":
                    return TextureFormat.R16;
                case "bayer_gbrg16":
                    return TextureFormat.R16;
                case "bayer_grbg16":
                    return TextureFormat.R16;
            }
            return TextureFormat.RGB24;
        }

        public static Texture2D ToTexture2D(this MCompressedImage message)
        {
            var tex = new Texture2D(1, 1);
            tex.LoadImage(message.data);
            return tex;
        }

        public static Texture2D ToTexture2D(this MImage message, bool convert, bool flipY)
        {
            var tex = new Texture2D((int)message.width, (int)message.height, message.encoding.EncodingToTextureFormat(), false);
            var data = EncodingConversion(message.data, message.encoding, (int)message.width, (int)message.height, convert, flipY);
            tex.LoadRawTextureData(data);
            tex.Apply();
            return tex;
        }

        public static MCompressedImage ToMCompressedImage(this Texture2D tex, string format="jpeg")
        {
            var data = tex.GetRawTextureData();
            return new MCompressedImage(new MHeader(), format, data);
        }

        public static MImage ToMImage(this Texture2D tex, string encoding="RGBA", byte isBigEndian=0, uint step=4)
        {
            var data = tex.GetRawTextureData();
            return new MImage(new MHeader(), (uint)tex.width, (uint)tex.height, encoding, isBigEndian, step, data);
        }

        static Dictionary<JoyRegion, int> joyDS4 = new Dictionary<JoyRegion, int>() 
        {
            { JoyRegion.BSouth, 0 }, { JoyRegion.BEast, 1 },
            { JoyRegion.BWest, 3 }, { JoyRegion.BNorth, 2 },
            { JoyRegion.LB, (int)JoyRegion.LB },
            { JoyRegion.RB, (int)JoyRegion.RB },
            { JoyRegion.Back, 8 }, { JoyRegion.Start, 9 },
            { JoyRegion.Power, 10 }, { JoyRegion.LPress, 11 },
            { JoyRegion.RPress, 12 },
            { JoyRegion.lAxisX, 0 }, { JoyRegion.lAxisY, 1 },
            { JoyRegion.rAxisX, 3 }, { JoyRegion.rAxisY, 4 },
            { JoyRegion.ltAxis, 2 }, { JoyRegion.rtAxis, 5 },
            { JoyRegion.dAxisX, 6 }, { JoyRegion.dAxisY, 7 },
        };

        static Dictionary<JoyRegion, int> joyXB360Windows = new Dictionary<JoyRegion, int>() 
        {
            { JoyRegion.BSouth, (int)JoyRegion.BSouth },
            { JoyRegion.BEast, (int)JoyRegion.BEast },
            { JoyRegion.BWest, (int)JoyRegion.BWest },
            { JoyRegion.BNorth, (int)JoyRegion.BNorth },
            { JoyRegion.LB, (int)JoyRegion.LB },
            { JoyRegion.RB, (int)JoyRegion.RB },
            { JoyRegion.Back, 14 },
            { JoyRegion.Start, (int)JoyRegion.Start },
            { JoyRegion.Power, (int)JoyRegion.Power },
            { JoyRegion.LPress, (int)JoyRegion.LPress },
            { JoyRegion.RPress, (int)JoyRegion.RPress },
            { JoyRegion.lAxisX, 0 }, { JoyRegion.lAxisY, 1 },
            { JoyRegion.rAxisX, 2 }, { JoyRegion.rAxisY, 3 },
            { JoyRegion.ltAxis, 4 }, { JoyRegion.rtAxis, 5 },
            { JoyRegion.dAxisX, 6 }, { JoyRegion.dAxisY, 7 },
        };

        static Dictionary<JoyRegion, int> joyXB360Linux = new Dictionary<JoyRegion, int>() 
        {
            { JoyRegion.BSouth, (int)JoyRegion.BSouth },
            { JoyRegion.BEast, (int)JoyRegion.BEast },
            { JoyRegion.BWest, (int)JoyRegion.BWest },
            { JoyRegion.BNorth, (int)JoyRegion.BNorth },
            { JoyRegion.LB, (int)JoyRegion.LB },
            { JoyRegion.RB, (int)JoyRegion.RB },
            { JoyRegion.Back, (int)JoyRegion.Back },
            { JoyRegion.Start, (int)JoyRegion.Start },
            { JoyRegion.Power, (int)JoyRegion.Power },
            { JoyRegion.LPress, (int)JoyRegion.LPress },
            { JoyRegion.RPress, (int)JoyRegion.RPress },
            { JoyRegion.lAxisX, 0 }, { JoyRegion.lAxisY, 1 },
            { JoyRegion.rAxisX, 2 }, { JoyRegion.rAxisY, 3 },
            { JoyRegion.ltAxis, 4 }, { JoyRegion.rtAxis, 5 },
            { JoyRegion.dAxisX, 6 }, { JoyRegion.dAxisY, 7 },
        };

        static Dictionary<JoyRegion, int> joyXB360Wired = new Dictionary<JoyRegion, int>() 
        {
            { JoyRegion.BSouth, (int)JoyRegion.BSouth },
            { JoyRegion.BEast, (int)JoyRegion.BEast },
            { JoyRegion.BWest, (int)JoyRegion.BWest },
            { JoyRegion.BNorth, (int)JoyRegion.BNorth },
            { JoyRegion.LB, (int)JoyRegion.LB },
            { JoyRegion.RB, (int)JoyRegion.RB },
            { JoyRegion.Back, (int)JoyRegion.Back },
            { JoyRegion.Start, (int)JoyRegion.Start },
            { JoyRegion.Power, (int)JoyRegion.Power },
            { JoyRegion.LPress, (int)JoyRegion.LPress },
            { JoyRegion.RPress, (int)JoyRegion.RPress },
            { JoyRegion.lAxisX, 0 }, { JoyRegion.lAxisY, 1 },
            { JoyRegion.rAxisX, 3 }, { JoyRegion.rAxisY, 4 },
            { JoyRegion.ltAxis, 2 }, { JoyRegion.rtAxis, 5 },
            { JoyRegion.dAxisX, 6 }, { JoyRegion.dAxisY, 7 },
        };

        static Dictionary<JoyRegion, int> joyF710 = new Dictionary<JoyRegion, int>() 
        {
            { JoyRegion.BSouth, 1 }, { JoyRegion.BEast, 2 },
            { JoyRegion.BWest, 0 }, { JoyRegion.BNorth, 3 },
            { JoyRegion.LB, (int)JoyRegion.LB },
            { JoyRegion.RB, (int)JoyRegion.RB },
            { JoyRegion.Back, 8 }, { JoyRegion.Start, 9 },
            { JoyRegion.Power, (int)JoyRegion.Power },
            { JoyRegion.LPress, 10 }, { JoyRegion.RPress, 11 },
            { JoyRegion.lAxisX, 0 }, { JoyRegion.lAxisY, 1 },
            { JoyRegion.rAxisX, 2 }, { JoyRegion.rAxisY, 3 },
            { JoyRegion.ltAxis, 6 }, { JoyRegion.rtAxis, 7 },
            { JoyRegion.dAxisX, 6 }, { JoyRegion.dAxisY, 7 },
        };

        static Dictionary<JoyLayout, Dictionary<JoyRegion, int>> layoutToMap = new Dictionary<JoyLayout, Dictionary<JoyRegion, int>>()
        {
            { JoyLayout.DS4, joyDS4 },
            { JoyLayout.XB360Windows, joyXB360Windows },
            { JoyLayout.XB360Linux, joyXB360Linux },
            { JoyLayout.XB360Wired, joyXB360Wired },
            { JoyLayout.F710, joyF710 }
        };

        /// <summary>
        /// Creates a new Texture2D that displays the current input values of a region of the MJoy message
        /// </summary>
        /// <returns>Texture2D corresponding to this region and layout</returns>
        public static Texture2D TextureFromJoy(this MJoy message, JoyRegion region, int layout=0) 
        {
            Color[] colorHighlight = new Color[100];
            for (int i = 0; i < colorHighlight.Length; i++)
            {
                colorHighlight[i] = Color.red;
            }

            Color[] colorPress = new Color[2500];
            for (int i = 0; i < colorPress.Length; i++)
            {
                colorPress[i] = Color.blue;
            }

            Texture2D tex;
            int x = 0;
            int y = 0;
            var layoutMap = layoutToMap[(JoyLayout)layout];

            if ((int)region <= 10) 
            {
                // Define small button context
                tex = new Texture2D(10, 10);
                if (message.buttons[layoutMap[region]] == 1)
                    tex.SetPixels(0, 0, 10, 10, colorHighlight);
            }
            else {
                // Axes
                switch (region)
                {
                    case JoyRegion.LStick:
                        tex = new Texture2D(50, 50);
                        x = (Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.lAxisX]] * -20) + tex.width / 2);
                        y = (Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.lAxisY]] * 20) + tex.height / 2);
                        if (message.buttons[layoutMap[JoyRegion.LPress]] == 1)
                            tex.SetPixels(0, 0, 50, 50, colorPress);
                        tex.SetPixels(x - 5, y - 5, 10, 10, colorHighlight);
                        break;
                    case JoyRegion.RStick:
                        tex = new Texture2D(50, 50);
                        x = (Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.rAxisX]] * -20) + tex.width / 2);
                        y = (Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.rAxisY]] * 20) + tex.height / 2);
                        if (message.buttons[layoutMap[JoyRegion.RPress]] == 1)
                            tex.SetPixels(0, 0, 50, 50, colorPress);
                        tex.SetPixels(x - 5, y - 5, 10, 10, colorHighlight);
                        break;
                    case JoyRegion.DPad:
                        tex = new Texture2D(50, 50);
                        x = (Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.dAxisX]] * -20) + tex.width / 2);
                        y = (Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.dAxisY]] * 20) + tex.height / 2);
                        tex.SetPixels(x - 5, y - 5, 10, 10, colorHighlight);
                        break;
                    case JoyRegion.LT:
                        tex = new Texture2D(25, 50);
                        y = Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.ltAxis]] * 20) + tex.height / 2;
                        tex.SetPixels(0, y - 2, 25, 4, colorHighlight);
                        break;
                    case JoyRegion.RT:
                        tex = new Texture2D(25, 50);
                        y = Mathf.FloorToInt(message.axes[layoutMap[JoyRegion.rtAxis]] * 20) + tex.height / 2;
                        tex.SetPixels(0, y - 2, 25, 4, colorHighlight);
                        break;
                    default:
                        tex = new Texture2D(1,1);
                        break;
                }
            }
            tex.Apply();
            return tex;
        }

        /// <summary>
        /// Creates a new Texture2D that grabs a region of interest of a given texture, if applicable. Otherwise, an approximated empty texture with a highlighted region is returned. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tex"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Texture2D RegionOfInterestTexture(this MRegionOfInterest message, Texture2D tex, int height, int width)
        {
            int x_off = (int)message.x_offset;
            int y_off = (int)message.y_offset;
            int mWidth = (int)message.width;
            int mHeight = (int)message.height;

            Texture2D overlay;
            if (tex == null)
            {
                // No texture provided, just return approximation
                if (width == 0 || height == 0)
                    overlay = new Texture2D(x_off + mWidth + 10, y_off + mHeight + 10);
                else 
                    overlay = new Texture2D(width, height);

                // Initialize ROI color block 
                Color[] colors = new Color[mHeight * mWidth];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = Color.red;
                }

                overlay.SetPixels(x_off, y_off, mWidth, mHeight, colors);
            }
            else 
            {
                // Crop out ROI from input texture
                overlay = new Texture2D(mWidth, mHeight, tex.format, true);
                overlay.SetPixels(0, 0, mWidth, mHeight, tex.GetPixels(x_off, y_off, mWidth, mHeight));
            }
            overlay.Apply();
            return overlay;
        }

        public static string ToLatLongString(this MNavSatFix message)
        {
            string lat = (message.latitude > 0) ? "ºN" : "ºS";
            string lon = (message.longitude > 0) ? "ºE" : "ºW";
            return $"{message.latitude}{lat} {message.longitude}{lon}";
        }
    }
}
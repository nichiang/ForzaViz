using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ForzaViz
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Listening on port 51384");
            
            UdpClient receivingUdpClient = new UdpClient(51384);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            
            bool recordRace = true;
            bool raceStarted = false;

            StreamWriter file = new StreamWriter(@"C:\Users\Nick Chiang\OneDrive\Projects\ForzaViz\Data\output.csv", false);

            try {
                while(recordRace) {
                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint); 

                    int index = 0;

                    Int32 IsRaceOn = BitConverter.ToInt32(receiveBytes, index); // = 1 when race is on. = 0 when in menus/race stopped …

                    UInt32 TimestampMS = BitConverter.ToUInt32(receiveBytes, index += 4); //Can overflow to 0 eventually

                    Single EngineMaxRpm = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single EngineIdleRpm = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single CurrentEngineRpm = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single AccelerationX = BitConverter.ToSingle(receiveBytes, index += 4); //In the car's local space; X = right, Y = up, Z = forward
                    Single AccelerationY = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single AccelerationZ = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single VelocityX = BitConverter.ToSingle(receiveBytes, index += 4); //In the car's local space; X = right, Y = up, Z = forward
                    Single VelocityY = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single VelocityZ = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single AngularVelocityX = BitConverter.ToSingle(receiveBytes, index += 4); //In the car's local space; X = pitch, Y = yaw, Z = roll
                    Single AngularVelocityY = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single AngularVelocityZ = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single Yaw = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single Pitch = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single Roll = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single NormalizedSuspensionTravelFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // Suspension travel normalized: 0.0f = max stretch; 1.0 = max compression
                    Single NormalizedSuspensionTravelFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single NormalizedSuspensionTravelRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single NormalizedSuspensionTravelRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single TireSlipRatioFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // Tire normalized slip ratio, = 0 means 100% grip and |ratio| > 1.0 means loss of grip.
                    Single TireSlipRatioFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single TireSlipRatioRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single TireSlipRatioRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single WheelRotationSpeedFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // Wheel rotation speed radians/sec. 
                    Single WheelRotationSpeedFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single WheelRotationSpeedRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single WheelRotationSpeedRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Int32 WheelOnRumbleStripFrontLeft = BitConverter.ToInt32(receiveBytes, index += 4); // = 1 when wheel is on rumble strip, = 0 when off.
                    Int32 WheelOnRumbleStripFrontRight = BitConverter.ToInt32(receiveBytes, index += 4);
                    Int32 WheelOnRumbleStripRearLeft = BitConverter.ToInt32(receiveBytes, index += 4);
                    Int32 WheelOnRumbleStripRearRight = BitConverter.ToInt32(receiveBytes, index += 4);

                    Single WheelInPuddleDepthFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // = from 0 to 1, where 1 is the deepest puddle
                    Single WheelInPuddleDepthFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single WheelInPuddleDepthRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single WheelInPuddleDepthRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single SurfaceRumbleFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // Non-dimensional surface rumble values passed to controller force feedback
                    Single SurfaceRumbleFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single SurfaceRumbleRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single SurfaceRumbleRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single TireSlipAngleFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // Tire normalized slip angle, = 0 means 100% grip and |angle| > 1.0 means loss of grip.
                    Single TireSlipAngleFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single TireSlipAngleRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single TireSlipAngleRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single TireCombinedSlipFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // Tire normalized combined slip, = 0 means 100% grip and |slip| > 1.0 means loss of grip.
                    Single TireCombinedSlipFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single TireCombinedSlipRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single TireCombinedSlipRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Single SuspensionTravelMetersFrontLeft = BitConverter.ToSingle(receiveBytes, index += 4); // Actual suspension travel in meters
                    Single SuspensionTravelMetersFrontRight = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single SuspensionTravelMetersRearLeft = BitConverter.ToSingle(receiveBytes, index += 4);
                    Single SuspensionTravelMetersRearRight = BitConverter.ToSingle(receiveBytes, index += 4);

                    Int32 CarOrdinal = BitConverter.ToInt32(receiveBytes, index += 4); //Unique ID of the car make/model
                    Int32 CarClass = BitConverter.ToInt32(receiveBytes, index += 4); //Between 0 (D -- worst cars) and 7 (X class -- best cars) inclusive 
                    Int32 CarPerformanceIndex = BitConverter.ToInt32(receiveBytes, index += 4); //Between 100 (slowest car) and 999 (fastest car) inclusive
                    Int32 DrivetrainType = BitConverter.ToInt32(receiveBytes, index += 4); //Corresponds to EDrivetrainType; 0 = FWD, 1 = RWD, 2 = AWD
                    Int32 NumCylinders = BitConverter.ToInt32(receiveBytes, index += 4); //Number of cylinders in the engine

                    if (IsRaceOn == 1) {
                        raceStarted = true;

                        file.WriteLine(
                            IsRaceOn + "," +
                            TimestampMS + "," +
                            EngineMaxRpm + "," +
                            EngineIdleRpm + "," +
                            CurrentEngineRpm + "," +
                            AccelerationX + "," +
                            AccelerationY + "," +
                            AccelerationZ + "," +
                            VelocityX + "," +
                            VelocityY + "," +
                            VelocityZ + "," +
                            AngularVelocityX + "," +
                            AngularVelocityY + "," +
                            AngularVelocityZ + "," +
                            Yaw + "," +
                            Pitch + "," +
                            Roll + "," +
                            NormalizedSuspensionTravelFrontLeft + "," +
                            NormalizedSuspensionTravelFrontRight + "," +
                            NormalizedSuspensionTravelRearLeft + "," +
                            NormalizedSuspensionTravelRearRight + "," +
                            TireSlipRatioFrontLeft + "," +
                            TireSlipRatioFrontRight + "," +
                            TireSlipRatioRearLeft + "," +
                            TireSlipRatioRearRight + "," +
                            WheelRotationSpeedFrontLeft + "," +
                            WheelRotationSpeedFrontRight + "," +
                            WheelRotationSpeedRearLeft + "," +
                            WheelRotationSpeedRearRight + "," +
                            WheelOnRumbleStripFrontLeft + "," +
                            WheelOnRumbleStripFrontRight + "," +
                            WheelOnRumbleStripRearLeft + "," +
                            WheelOnRumbleStripRearRight + "," +
                            WheelInPuddleDepthFrontLeft + "," +
                            WheelInPuddleDepthFrontRight + "," +
                            WheelInPuddleDepthRearLeft + "," +
                            WheelInPuddleDepthRearRight + "," +
                            SurfaceRumbleFrontLeft + "," +
                            SurfaceRumbleFrontRight + "," +
                            SurfaceRumbleRearLeft + "," +
                            SurfaceRumbleRearRight + "," +
                            TireSlipAngleFrontLeft + "," +
                            TireSlipAngleFrontRight + "," +
                            TireSlipAngleRearLeft + "," +
                            TireSlipAngleRearRight + "," +
                            TireCombinedSlipFrontLeft + "," +
                            TireCombinedSlipFrontRight + "," +
                            TireCombinedSlipRearLeft + "," +
                            TireCombinedSlipRearRight + "," +
                            SuspensionTravelMetersFrontLeft + "," +
                            SuspensionTravelMetersFrontRight + "," +
                            SuspensionTravelMetersRearLeft + "," +
                            SuspensionTravelMetersRearRight + "," +
                            CarOrdinal + "," +
                            CarClass + "," +
                            CarPerformanceIndex + "," +
                            DrivetrainType + "," +
                            NumCylinders
                        );
                    }

                    if (raceStarted && IsRaceOn == 0) {
                        recordRace = false; // Stop recording after one race

                        file.Close();
                    }
                }
            } catch ( Exception e ){
                Console.WriteLine(e.ToString()); 
                file.Close();
            }
        }
    }
}

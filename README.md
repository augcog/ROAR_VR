# IRG-RACING-VR

## Setup
Refer to the PC part of [ROARVR](https://github.com/augcog/ROARVR/tree/dev#pc)

## Build and Run
Double click `Assets/Scenes/SampleScene.unity` to open the Scene. You may customize any settings and click on play to run.

## Manual
In `Assets/Scenes/SampleScene.unity`, the `Utility` object acts as the manager for the scene. It has two components `DataReader.cs`
and `GStreamer.cs`.

`DataReader.cs` is a script automatically runs and fetches throttle and steering control signals.

`GStreamer.cs` is a script that fetches video streams through GStreamer framework. Port 1 is for the front view camera, and port 2 is for the rear view camera.

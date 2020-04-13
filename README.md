# ROAR_VR

## Setup
First and foremost, you have to clone [ROAR_Jetson](https://github.com/augcog/ROAR_Jetson) repo and setup on your Jetson. Then have your Jetson and PC connected to the same local network and make sure network is in good condition to get smooth and jitter-free video streaming. 

Then setup VR device. Install [Oculus software](https://www.oculus.com/setup/) and setup your VR device according to guidance. And you need to have **Unity** installed. 
To develop and build the unity project, make sure you intstall [GStreamer](https://gstreamer.freedesktop.org/documentation/installing/on-windows.html?gi-language=c) first and then [build OpenCV with GStreamer](https://cv-tricks.com/how-to/installation-of-opencv-4-1-0-in-windows-10-from-source/). 

For reference, we tested on [unity 2019.3.4](https://unity.cn/releases), [GStreamer 1.16.2 devel-msvc-x86_64](https://gstreamer.freedesktop.org/data/pkg/windows/1.16.2/gstreamer-1.0-devel-msvc-x86_64-1.16.2.msi),[GStreamer 1.16.2 devel-msvc-x86_64](https://gstreamer.freedesktop.org/data/pkg/windows/1.16.2/gstreamer-1.0-msvc-x86_64-1.16.2.msi), and [opencv 4.1.0](https://opencv.org/releases/page/2/).
## Build and Run
Double click `Assets/Scenes/SampleScene.unity` to open the Scene. You may customize any settings and click on play to run.

## Manual
In `Assets/Scenes/SampleScene.unity`, the `Utility` object acts as the manager for the scene. It has two components `DataReader.cs`
and `GStreamer.cs`.

`DataReader.cs` is a script automatically runs and fetches throttle and steering control signals.

`GStreamer.cs` is a script that fetches video streams through GStreamer framework. Port 1 is for the front view camera, and port 2 is for the rear view camera.

# ROAR_VR

## Setup
First and foremost, you have to clone [ROAR_Jetson](https://github.com/augcog/ROAR_Jetson) repo and setup on your Jetson. Then have your Jetson and PC connected to the same local network and make sure network is in good condition to get smooth and jitter-free video streaming. 

Then setup VR device. Install [Oculus software](https://www.oculus.com/setup/) and setup your VR device according to guidance. And you need to have **Unity** installed. 
To develop and build the unity project, make sure you intstall [GStreamer](https://gstreamer.freedesktop.org/documentation/installing/on-windows.html?gi-language=c) first and then [build OpenCV with GStreamer](https://cv-tricks.com/how-to/installation-of-opencv-4-1-0-in-windows-10-from-source/). 

For reference, we tested on [unity 2019.3.4](https://unity.cn/releases), [GStreamer 1.16.2 devel-msvc-x86_64](https://gstreamer.freedesktop.org/data/pkg/windows/1.16.2/gstreamer-1.0-devel-msvc-x86_64-1.16.2.msi),[GStreamer 1.16.2-msvc-x86_64](https://gstreamer.freedesktop.org/data/pkg/windows/1.16.2/gstreamer-1.0-msvc-x86_64-1.16.2.msi), and [opencv 4.1.0](https://opencv.org/releases/page/2/).
## Build and Run

### Testing without launching a car :

Here are the commands streaming test videos to port 5000 & 5001 on your host computer that could be used as video sources for Unity. Once you successfully Installed gstreamer, you will be able to run these commands in the terminal.

Sender:

gst-launch-1.0 videotestsrc ! video/x-raw,width=1280,height=720 ! x264enc ! h264parse ! rtph264pay ! udpsink host=127.0.0.1 port=5000

gst-launch-1.0 videotestsrc ! video/x-raw,width=1280,height=720 ! x264enc ! h264parse ! rtph264pay ! udpsink host=127.0.0.1 port=5001

You may replace videotestsrc to autovideosrc or some other sources for different videos.

You can run the following command in terminal for testing.

Command line Reciever:

gst-launch-1.0 udpsrc port=5000 ! application/x-rtp, encoding-name=H264,payload=96 ! rtph264depay ! h264parse ! avdec_h264 ! autovideosink

if you see a video shows in a pop-up window, it means that you have successfully streamed the video, you can now terminate the command line reciever and start working on Unity.Good Luck!

### Unity
Double click `Assets/Scenes/SampleScene.unity` to open the Scene. 

First you need to make sure your receiving resolution is the same as your sending resolution. First click on object `Utility` under `SampleScene`, and in the `Inspector` tab on the right side, you can see two public variables `width` and `height`. Set these two values the same as `IMAGE_W` and `IMAGE_H` in ROAR_Jetson/myconfig.py. 

Here is another variable called 'fisheye',  You may activate it to undistort the fisheye rear camera. We tested it on our MIPI camera with resolution 1280*720. IF you use different camera, you can deactivate it or modify the intrinsics.txt and dis_coeff.txt to fit your own camera.

Besides, you may want to customize the rendering resolution for either front-view window or rear-view mirror. These settings can be found in the `Inspector` tab of `Canvas/FrontView` and `backmirror/Canvas/Image`.

You may customize any settings and click on play to run.

When you want to stop everything, there is a bug here. Never stop program running on Jetson before stopping the game playing in **Unity**, otherwise **Unity** may hang.


## Manual
In `Assets/Scenes/SampleScene.unity`, the `Utility` object acts as the manager for the scene. It has two components `DataReader.cs`
and `GStreamer.cs`.

`DataReader.cs` is a script automatically runs and fetches throttle and steering control signals.

`GStreamer.cs` is a script that fetches video streams through GStreamer framework. Port 1 is for the front view camera, and port 2 is for the rear view camera.

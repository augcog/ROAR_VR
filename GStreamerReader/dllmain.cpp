#include "pch.h"
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/video.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/calib3d/calib3d.hpp>
#include <fstream>

using namespace cv;
using namespace std;
VideoCapture cap1, cap2;

Mat frame;
Mat mapx, mapy;

extern "C" _declspec(dllexport) bool GR_Init(int port1, int port2) {
	char pattern1[1000], pattern2[1000];
	sprintf(pattern1, "udpsrc port=%d ! application/x-rtp,encoding-name=H264,payload=96 ! rtph264depay ! h264parse ! avdec_h264 ! videoflip method=vertical-flip ! videoconvert ! appsink", port1);
	sprintf(pattern2, "udpsrc port=%d ! application/x-rtp,encoding-name=H264,payload=96 ! rtph264depay ! h264parse ! avdec_h264 ! videoflip method=vertical-flip ! videoconvert ! appsink", port2);
	cap1 = VideoCapture(pattern1, cv::CAP_GSTREAMER);
	cap2 = VideoCapture(pattern2, cv::CAP_GSTREAMER);
	Matx33d intrinsics;
	Vec4d distortion_coeff;
	
	ifstream intrinsicfile("intrinsics.txt");
	ifstream disfile("dis_coeff.txt");
	
	Size corrected_size(1280, 720);
	Mat corrected;
	/*
	intrinsics= Matx33d(671.026, 0, 650.266,
						0, 670.748, 365.647,
						0, 0, 1);
	distortion_coeff = Vec4d(-0.00679943, -0.0550859, 0.0894707, -0.045548);
	*/

	
	for (int i = 0; i < 3; ++i)
	{
		for (int j = 0; j < 3; ++j)
		{
			intrinsicfile >> intrinsics(i, j);
		}
	}
	for (int i = 0; i < 4; ++i)
	{
		disfile >> distortion_coeff(i);
	}
	intrinsicfile.close();
	disfile.close();
	
	fisheye::initUndistortRectifyMap(intrinsics, distortion_coeff, cv::Matx33d::eye(), intrinsics, corrected_size, CV_16SC2, mapx, mapy);

	return cap1.isOpened() && cap2.isOpened();
}

extern "C" _declspec(dllexport) uchar* GR_GetFrame(int flag, bool useFisheye,int &size) {
	//VideoCapture& cap = (flag == 1 ? cap1 : cap2);
	
	if (flag == 1)
		cap1.read(frame);
	else
		cap2.read(frame);
	
	//cap.read(frame);
	if (flag == 1 && useFisheye) {
		Mat corrected;
		remap(frame, corrected, mapx, mapy, INTER_LINEAR, BORDER_CONSTANT);
		frame = corrected;
	}
	
	//flip(frame, frame, +1);
	cvtColor(frame, frame, COLOR_BGR2RGB);
	size = frame.cols * frame.rows * frame.channels();
	//return new Mat(frame);
	//length = frame.total() * frame.elemSize();
	return frame.data;
}


extern "C" _declspec(dllexport) void GR_Release() {
	cap1.release();
	cap2.release();
}
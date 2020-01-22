#pragma once
#include <set>
#include <cmath>
#include <string>
#include <sstream>
#include "ws2tcpip.h"
#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "Iphlpapi.lib")

#define BROADCAST_PORT 9090
#define TCP_PORT 7788
#define UNITY_PORT 4567
#define PREAMBLE "Logitech control"

typedef UINT8 COMMAND_TYPE;
#define COMMAND_THROTTLE 0
#define COMMAND_STEERING 1
#define NONE 10000000

class CommandSender {
public:
	CommandSender();
	~CommandSender();

	bool Match();
	void sendCommand(COMMAND_TYPE type, int value, int oriValue = NONE);

private:
	std::string GetBroadcastAddress();
	std::string CombineIPMask(std::string IP, UINT8 prefixLength, int family);
	static DWORD WINAPI StaticListenThread(void *param);
	DWORD ListenThread();

	std::string broadcastAddr;
	SOCKET serverSock, tcpSock;
	struct sockaddr_in remoteAddr;
	bool connected;
};


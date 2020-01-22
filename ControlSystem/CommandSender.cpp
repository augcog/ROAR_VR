#include "stdafx.h"
#include "CommandSender.h"


CommandSender::CommandSender() : connected(false) {
	WORD wVersionRequested = MAKEWORD(2, 2);
	WSADATA wsaData;
	WSAStartup(wVersionRequested, &wsaData);

	broadcastAddr = GetBroadcastAddress();
	Match();
}


CommandSender::~CommandSender() {
	WSACleanup();
}


DWORD WINAPI CommandSender::StaticListenThread(void *param) {
	return ((CommandSender *)param)->ListenThread();
}


DWORD CommandSender::ListenThread() {
	listen(serverSock, 1);
	int addrLen = sizeof(remoteAddr);
	tcpSock = accept(serverSock, (sockaddr *)&remoteAddr, &addrLen);
	connected = true;
	return 0L;
}


bool CommandSender::Match() {
	SOCKET bc_sock = socket(AF_INET, SOCK_DGRAM, 0);
	char broadcast = '1';
    if(setsockopt(bc_sock, SOL_SOCKET, SO_BROADCAST, &broadcast, sizeof(broadcast)) < 0) {
        closesocket(bc_sock);
        return false;
    }

	char msg[] = PREAMBLE;
	struct sockaddr_in recv_addr, sender_addr;
	recv_addr.sin_family = AF_INET;
	recv_addr.sin_port = htons(BROADCAST_PORT);
	recv_addr.sin_addr.s_addr = inet_addr(broadcastAddr.c_str());

	serverSock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	struct sockaddr_in my_addr;
	my_addr.sin_family = AF_INET;
	my_addr.sin_port = htons(TCP_PORT);
	my_addr.sin_addr.s_addr = INADDR_ANY;

	bind(serverSock, (sockaddr *)&my_addr, sizeof(my_addr));

	DWORD threadID;
	HANDLE listenThread = CreateThread(NULL, 0, StaticListenThread, (void *)this, 0, &threadID);

	while (!connected) {
		sendto(bc_sock, msg, strlen(msg) + 1, 0, (sockaddr *)&recv_addr, sizeof(recv_addr));
		Sleep(300);
	}
	CloseHandle(listenThread);

	struct sockaddr_in peer_addr;
	int l = sizeof(peer_addr);
	getpeername(serverSock, (struct sockaddr *)&peer_addr, &l);
	char ipstr[INET6_ADDRSTRLEN];
	inet_ntop(AF_INET, &(peer_addr.sin_addr), ipstr, sizeof ipstr);
	TRACE("Peer IP: %s", ipstr);

	closesocket(bc_sock);
	return true;
}


std::string CommandSender::GetBroadcastAddress() {
	char local[255] = { 0 };
	gethostname(local, sizeof(local));
	hostent* ph = gethostbyname(local);
	if (ph == NULL) return "";

	in_addr addr;
	std::set<std::string> addrSet;
	std::string localIP;
	for (int i = 0; ph->h_addr_list[i]; ++i) {
		memcpy(&addr, ph->h_addr_list[i], sizeof(in_addr));
		localIP.assign(inet_ntoa(addr));
		addrSet.insert(localIP);
	}
	localIP = "";
	int prefLen = 0, family;

	DWORD rv, size;
	PIP_ADAPTER_ADDRESSES adapter_addresses, aa;
	PIP_ADAPTER_UNICAST_ADDRESS ua;

	rv = GetAdaptersAddresses(AF_UNSPEC, GAA_FLAG_INCLUDE_PREFIX, NULL, NULL, &size);
	adapter_addresses = (PIP_ADAPTER_ADDRESSES)malloc(size);

	rv = GetAdaptersAddresses(AF_UNSPEC, GAA_FLAG_INCLUDE_PREFIX, NULL, adapter_addresses, &size);
	char buf[BUFSIZ];

	for (aa = adapter_addresses; aa != NULL; aa = aa->Next) {
		if (aa->IfType != IF_TYPE_IEEE80211) continue;
		for (ua = aa->FirstUnicastAddress; ua != NULL; ua = ua->Next) {
			memset(buf, 0, sizeof(buf));
			getnameinfo(ua->Address.lpSockaddr, ua->Address.iSockaddrLength, buf, sizeof(buf), NULL, 0, NI_NUMERICHOST);
			std::set<std::string>::iterator it = addrSet.find(std::string(buf));
			if (it != addrSet.end()) {
				localIP = *it;
				prefLen = ua->OnLinkPrefixLength;
				family = ua->Address.lpSockaddr->sa_family;
				break;
			}
		}
		if (localIP != "") break;
	}

	free(adapter_addresses);
	return CombineIPMask(localIP, prefLen, family);
}


std::string CommandSender::CombineIPMask(std::string IP, UINT8 prefixLength, int family) {
	struct in_addr *inp;
	const char *s = IP.c_str();
	UINT8 ip[16];
	inet_pton(family, IP.c_str(), ip);
	UINT8 mask[16];
	if (family == AF_INET6) {
		memset(mask, 0, sizeof(mask));
		for (int i = 0; i < 4 && prefixLength > 0; ++i) {
			ConvertLengthToIpv4Mask(min(prefixLength, 32), ((PULONG)mask) + i);
			prefixLength -= 32;
		}
	} else {
		ConvertLengthToIpv4Mask(prefixLength, (PULONG)mask);
	}
	UINT8 dst[16];
	for (int i = 0; i < (family == AF_INET6 ? 4 : 1); ++i) {
		*((PULONG)dst + i) = ((*((PULONG)ip + i)) | ~(*((PULONG)mask + i)));
	}
	char res[50];
	inet_ntop(family, dst, res, sizeof(res));
	return std::string(res);
}


void CommandSender::sendCommand(COMMAND_TYPE type, int value, int oriValue) {
	if (type == COMMAND_STEERING) {
		char bf[100];
		sprintf(bf, "%d %d\n", value, oriValue);
		TRACE(bf);
	}
	int buf[2];
	buf[0] = htonl(type);
	buf[1] = htonl(value);
	send(tcpSock, (char *)buf, sizeof(buf), 0);

	// Send to local port for Unity
	if (oriValue != NONE) {
		buf[1] = htonl(oriValue);
	}
	SOCKET sock = socket(AF_INET, SOCK_DGRAM, 0);
	struct sockaddr_in recv_addr;
	recv_addr.sin_family = AF_INET;
	recv_addr.sin_port = htons(UNITY_PORT);
	recv_addr.sin_addr.s_addr = inet_addr("127.0.0.1");
	sendto(sock, (char *)buf, sizeof(buf), 0, (sockaddr *)&recv_addr, sizeof(recv_addr));
}

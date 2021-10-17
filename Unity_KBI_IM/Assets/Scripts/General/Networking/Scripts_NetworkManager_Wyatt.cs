using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public enum MultiplayerType { QUICKPLAY, MATCHMAKING, CUSTOM_LOBBY }

[DefaultExecutionOrder(-60)]
public class Scripts_NetworkManager_Wyatt : MonoBehaviourPunCallbacks {
	public static Scripts_NetworkManager_Wyatt Instance { get; private set; }

	// subscribe to these to interface with our networking system
	#region NETWORKING EVENTS
	public static event Action Connected;
	public static event Action ConnectedToMaster;
	public static event Action CreatedRoom;
	public static event Action<short, string> CreateRoomFailed;
	public static event Action<string> CustomAuthenticationFailed;
	public static event Action<Dictionary<string, object>> CustomAuthenticationResponse;
	public static event Action<DisconnectCause> Disconnected;
	public static event Action<ErrorInfo> ErrorInfo;
	public static event Action<List<FriendInfo>> FriendListUpdate;
	public static event Action JoinedLobby;
	public static event Action JoinedRoom;
	public static event Action<short, string> JoinRandomFailed;
	public static event Action<short, string> JoinRoomFailed;
	public static event Action LeftLobby;
	public static event Action LeftRoom;
	public static event Action<List<TypedLobbyInfo>> LobbyStatisticsUpdate;
	public static event Action<Player> MasterClientSwitched;
	public static event Action<Player> PlayerEnteredRoom;
	public static event Action<Player> PlayerLeftRoom;
	public static event Action<Player, Hashtable> PlayerPropertiesUpdate;
	public static event Action<RegionHandler> RegionListReceived;
	public static event Action<List<RoomInfo>> RoomListUpdate;
	public static event Action<Hashtable> RoomPropertiesUpdate;
	public static event Action<OperationResponse> WebRpcResponse;
	#endregion

	#region LOBBY TYPES
	public static MultiplayerType multiplayerType;
	public static TypedLobby quickplayLobby = new TypedLobby("quickplay", LobbyType.Default);
	public static TypedLobby matchmadeLobby = new TypedLobby("matchmade", LobbyType.Default);
	public static TypedLobby customLobby = new TypedLobby("custom", LobbyType.Default);
	#endregion

	static string currentRoomName;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.AutomaticallySyncScene = true;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	#region PHOTON CALLBACKS
	public override void OnConnected()															{ Connected?.Invoke();											}
	public override void OnConnectedToMaster()													{ ConnectedToMaster?.Invoke();									}
	public override void OnCreatedRoom()														{ CreatedRoom?.Invoke();										}
	public override void OnCreateRoomFailed(short returnCode, string message)					{ CreateRoomFailed?.Invoke(returnCode, message);				}
	public override void OnCustomAuthenticationFailed(string debugMessage)						{ CustomAuthenticationFailed?.Invoke(debugMessage);				}
	public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)		{ CustomAuthenticationResponse?.Invoke(data);					}
	public override void OnDisconnected(DisconnectCause cause)									{ Disconnected?.Invoke(cause);									}
	public override void OnErrorInfo(ErrorInfo errorInfo)										{ ErrorInfo?.Invoke(errorInfo);									}
	public override void OnFriendListUpdate(List<FriendInfo> friendList)						{ FriendListUpdate?.Invoke(friendList);							}
	public override void OnJoinedLobby()														{ JoinedLobby?.Invoke();										}
	public override void OnJoinedRoom()															{ JoinedRoom?.Invoke();											}
	public override void OnJoinRandomFailed(short returnCode, string message)					{ JoinRandomFailed?.Invoke(returnCode, message);				}
	public override void OnJoinRoomFailed(short returnCode, string message)						{ JoinRoomFailed?.Invoke(returnCode, message);					}
	public override void OnLeftLobby()															{ LeftLobby?.Invoke();											}
	public override void OnLeftRoom()															{ LeftRoom?.Invoke();											} 
	public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)			{ LobbyStatisticsUpdate?.Invoke(lobbyStatistics);				}
	public override void OnMasterClientSwitched(Player newMasterClient)							{ MasterClientSwitched?.Invoke(newMasterClient);				}
	public override void OnPlayerEnteredRoom(Player newPlayer)									{ PlayerEnteredRoom?.Invoke(newPlayer);							}
	public override void OnPlayerLeftRoom(Player otherPlayer)									{ PlayerLeftRoom?.Invoke(otherPlayer);							}
	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)	{ PlayerPropertiesUpdate?.Invoke(targetPlayer, changedProps);	}
	public override void OnRegionListReceived(RegionHandler regionHandler)						{ RegionListReceived?.Invoke(regionHandler);					}
	public override void OnRoomListUpdate(List<RoomInfo> roomList)								{ RoomListUpdate?.Invoke(roomList);								}
	public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)				{ RoomPropertiesUpdate?.Invoke(propertiesThatChanged);			}
	public override void OnWebRpcResponse(OperationResponse response)							{ WebRpcResponse?.Invoke(response);								}
	#endregion

	public static void SetRoomName(string name) => currentRoomName = name;
	public static string GetRoomName() => currentRoomName;
}

using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Collections;

public class MinecraftClient : MonoBehaviour
{
	#region Attributes
    #region Public Attributes
    public bool                     connection = false;
    #endregion
    #region Private Attributes
    private ButtonManager           buttonManager;
    private	static MinecraftClient	m_singleton = null;
	private	Socket					m_socket = null;
	private	string					m_IPAdress = "127.0.0.1";
	private	const int				m_port = 25565;
	#endregion
	#endregion

	#region Private Methods
    void Start()
    {
        buttonManager = GameObject.FindGameObjectWithTag("ButtonManager").GetComponent<ButtonManager>();
    }

	void Cancel()
	{
		Debug.Log("Cancel");
		if (m_socket != null)
			m_socket.Close();
        buttonManager.DirectConnection();
	}

	void Connect()
	{
		Debug.Log("Connect");
        System.Net.IPAddress	remoteIPAddress = System.Net.IPAddress.Parse(m_IPAdress);
        System.Net.IPEndPoint	remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, m_port);

		m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_singleton = this;
        m_socket.Connect(remoteEndPoint);
	}
	#endregion

	#region Unity CallBacks Methods
	void OnGUI()
	{
        if (!connection)
            return;

		GUILayout.BeginArea(new Rect(Screen.width / 3, Screen.height / 4, Screen.width / 3 + 1.0f, Screen.height - Screen.height / 4));
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
					m_IPAdress = GUILayout.TextField(m_IPAdress, GUILayout.Width(Screen.width / 3));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					if (GUILayout.Button("Cancel", GUILayout.Height(20)) == true)
						Cancel();
					if (GUILayout.Button("Connect", GUILayout.Height(20)) == true)
						Connect();
				GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	#endregion
}

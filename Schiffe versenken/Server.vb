
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Threading

Public Class Server
    ReadOnly conn As Connection
    Dim connected As Boolean
    Public ServerIP As String
    Public ServerPort As Integer = 60_005
    Dim Listener As TcpListener
    Dim ListenerThread As Thread
    Public user As User
    Dim shift As Integer

    Public Sub New(ip As String, UserExists As User, conn As Connection)
        InitializeComponent()

        ServerIP = ip
        Me.conn = conn
        Dim Load = LoadingForm.getForm()

        While (PortInUse(ServerPort))
            ServerPort += 1
        End While

        Listener = New TcpListener(Net.IPAddress.Parse(ServerIP), ServerPort)
        ListenerThread = New Thread(New ThreadStart(AddressOf Listener.Start))
        ListenerThread.Start()

        Load.lblP.Text = (ip & ":" & ServerPort)

        If Not UserExists Is Nothing Then
            user = UserExists
            shift = user.shiftNumber
            connected = True
        End If
    End Sub

    Private Function PortInUse(port As Integer) As Boolean
        Dim IPs As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
        Dim ActiveListenersIPs As Net.IPEndPoint() = IPs.GetActiveTcpListeners()

        For Each ActiveListener In ActiveListenersIPs
            If ActiveListener.Port = port Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Async Function GetUserWhenReady() As Task(Of User)
        While (user Is Nothing)
            Await Task.Delay(100)
        End While
        Return user
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Listener.Pending Then
            Dim Message = ""
            Dim ServerClient As TcpClient = Listener.AcceptTcpClient()

            Dim Reader As New StreamReader(ServerClient.GetStream())

            Try
                While Reader.Peek > -1
                    Message += Convert.ToChar(Reader.Read()).ToString
                End While
            Catch
                conn.LeaverBuster()
                conn.CloseGame()
            End Try

            If Message = "" Then
                Return
            End If

            Message = Decode(Message)

            If connected = False Then                                                     'Nachrichten verarbeiten
                    If Message.StartsWith("CONNECT TO ") Then
                        Dim ConnectIP = Message.Substring(11)
                        Dim stringarray As String() = ConnectIP.Split(":")
                    user = New User(stringarray(0), stringarray(1), False, conn)
                    shift = user.shiftNumber
                    connected = True
                    conn.ShowForm()
                    Return
                    End If
                End If

                If Message.StartsWith("SHOOT") Then
                    Dim stringArray As String() = Message.Split(";")
                    Dim Spieler As Integer = stringArray(1)
                    Dim x As Integer = stringArray(2)
                    Dim y As Integer = stringArray(3)
                    conn.HitAt(Spieler, x, y)

                End If

                If Message.StartsWith("HIT") Then
                    Dim stringArray As String() = Message.Split(";")
                    Dim Spieler As Integer = stringArray(1)
                    Dim x As Integer = stringArray(2)
                    Dim y As Integer = stringArray(3)
                    conn.MarkPanelHit(Spieler, x, y)

                ElseIf Message.StartsWith("NOHIT") Then
                    Dim stringArray As String() = Message.Split(";")
                    Dim Spieler As Integer = stringArray(1)
                    Dim x As Integer = stringArray(2)
                    Dim y As Integer = stringArray(3)
                    conn.MarkPanelNoHit(Spieler, x, y)

                End If

                If Message.StartsWith("WON") Then
                    Dim stringArray As String() = Message.Split(";")
                    Dim Spieler As Integer = stringArray(1)
                    conn.MessageWon(Spieler)

                End If

                If Message.StartsWith("READY") Then

                    conn.PlayerReady()

                End If

            End If

    End Sub

    Private Function Decode(input As String) As String
        Return Verschlüsselung.Decode_Caesar(input, shift)
    End Function
End Class

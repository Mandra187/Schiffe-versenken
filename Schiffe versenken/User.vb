Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class User
    Dim ServerIP As String
    Dim ServerPort As Integer
    Dim Server As Server
    Dim Shift As Integer


    Public Sub New(IP As String, Port As String, createServer As Boolean, conn As Connection)
        ServerIP = IP
        ServerPort = Port
        InitializeComponent()
        Shift = shiftNumber()

        If createServer Then
            Dim strHostName As String
            Dim strIPAddress As String
            strHostName = Net.Dns.GetHostName()
            strIPAddress = Net.Dns.GetHostEntry(strHostName).AddressList(0).ToString()
            Server = New Server(strIPAddress, Me, conn)
            Dim UserClient As New TcpClient(ServerIP, ServerPort)
            Dim Writer As New StreamWriter(UserClient.GetStream())
            Writer.Write("CONNECT TO " & strIPAddress & ":" & Server.ServerPort)
            Writer.Flush()
            Writer.Close()
            UserClient.Close()

            'SendMessage("CONNECT TO " & strIPAddress & ":" & Server.ServerPort)
        End If
    End Sub

    Public Sub SendMessage(message As String)
        If message = "" Then
            Return
        End If
        Dim UserClient As New TcpClient(ServerIP, ServerPort)
        Dim Writer As New StreamWriter(UserClient.GetStream())
        Writer.Write(Encode(message))
        Writer.Flush()
    End Sub

    Private Function Encode(input As String) As String
        Return Verschlüsselung.Encode_Caesar(input, Shift)
    End Function

    Public Function shiftNumber() As Integer
#Region "Verschlüsselung aus IPs"
        '10.132.100.76 
        'Dim strHostName As String
        'Dim strIPAddress As String
        'strHostName = Net.Dns.GetHostName()
        'strIPAddress = Net.Dns.GetHostEntry(strHostName).AddressList(0).ToString()

        'Dim myIP As String = strIPAddress
        'Dim secondIP As String = ServerIP

        'Dim secretNums() = strIPAddress.Split(".")
        'Dim secretNums2() = ServerIP.Split(".")
        'Dim superSecretNum As Integer
        'For i As Integer = 0 To 3
        '    superSecretNum += secretNums(i) + secretNums2(i)
        'Next
        'superSecretNum = superSecretNum Mod 25
        'superSecretNum += 1

        'Return superSecretNum
#End Region






    End Function

End Class
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class Connection
    Dim Main As MainForm
    Dim user As User
    Dim loadingForm As LoadingForm = LoadingForm.getForm()
    Public Sub New(spielerZahl As Integer, Main As MainForm)
        If spielerZahl = 1 Then
            hostServer()
        ElseIf spielerZahl = 2 Then
            connectServer()
        End If
        Me.Main = Main
    End Sub

    Public Sub Leaver(yourplayerid As Integer)
        Dim msg As String = ("LEFT;" & yourplayerid & ";")
        user.SendMessage(msg)
    End Sub

    Public Sub LeaverBuster()
        Main.LeaverMessage()
    End Sub

    Public Sub CloseGame()
        Main.GameClose()
    End Sub
    Public Sub ShootAt(yourplayerid As Integer, x As Integer, y As Integer)
        'Returns if Shot has hit Ship on other Players Field
        Dim msg As String = ("SHOOT;" & yourplayerid & ";" & x & ";" & y)
        user.SendMessage(msg)
    End Sub

    Public Function HitAt(yourplayerid As Integer, x As Integer, y As Integer)
        'color on own field
        If Main.CheckHit(x, y) Then
            Main.Modus = MainForm.Modi.getroffen
            Main.MarkCurrentPosition(x, y)
            user.SendMessage("HIT;" & yourplayerid & ";" & x & ";" & y)
        Else
            Main.Modus = MainForm.Modi.Verfehlt
            Main.MarkCurrentPosition(x, y)
            user.SendMessage("NOHIT;" & yourplayerid & ";" & x & ";" & y)
        End If
    End Function

    Public Sub MarkPanelHit(Spieler As Integer, x As Integer, y As Integer)
        Main.Modus = MainForm.Modi.selfMarkHit
        Main.MarkCurrentPosition(x, y)
    End Sub

    Public Sub MarkPanelNoHit(Spieler As Integer, x As Integer, y As Integer)
        Main.Modus = MainForm.Modi.selfMark
        Main.MarkCurrentPosition(x, y)
    End Sub

    Public Sub SendWon(yourplayerid As Integer)
        user.SendMessage("WON;" & yourplayerid)
    End Sub

    Public Sub MessageWon(yourplayerid As Integer)
        Main.playerEndGame(yourplayerid)
    End Sub

    Public Sub SendReady(yourplayerid As Integer)
        user.SendMessage("READY; " & yourplayerid)
    End Sub

    Public Sub PlayerReady()
        Main.Ready()
    End Sub

    Public Sub ShowForm()
        loadingForm.HideForm()
        Main.Show()
    End Sub

    '///////////////////////////////////SERVER/////////////////////////////////////////////
    Private Async Sub hostServer()
        Dim strHostName As String
        Dim strIPAddress As String
        strHostName = Net.Dns.GetHostName()
        strIPAddress = Net.Dns.GetHostEntry(strHostName).AddressList(0).ToString()
        Dim server As New Server(strIPAddress, Nothing, Me)
        user = Await server.GetUserWhenReady()
    End Sub

    Private Sub connectServer()
        Dim stringarray As String() = FormStart.IP.Split(":")
        Dim IP As String
        IP = stringarray(0)
        Dim Port As String
        Port = stringarray(1)
        user = New User(IP, Port, True, Me)
    End Sub

End Class

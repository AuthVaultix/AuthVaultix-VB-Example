Imports System.IO
Imports Client.AuthVaultix

Public Class Form2

    Private channel As String = "test"

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Drag.MakeDraggable(Me)
        timer1.Interval = 5000
        timer1.Start()

        ' VIP check
        Dim level As String = Form1.Client.GetVar("level")

        If level = "vip" Then
            MessageBox.Show("⚠ Your update support has expired." & vbCrLf & "Please renew your subscription.", "Support Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        ' User Data
        Dim user = Form1.Client.CurrentUser

        userDataField.Items.Add("Username: " & user.username)
        userDataField.Items.Add("License: " & user.subscriptions(0).key)
        userDataField.Items.Add("Expires: " & user.subscriptions(0).ExpiryFormatted)
        userDataField.Items.Add("Subscription: " & user.subscriptions(0).subscription)
        userDataField.Items.Add("IP: " & user.ip)
        userDataField.Items.Add("HWID: " & user.hwid)
        userDataField.Items.Add("Creation Date: " & user.CreationDateFormatted)
        userDataField.Items.Add("Last Login: " & user.LastLoginFormatted)
        userDataField.Items.Add("Time Left: " & user.subscriptions(0).TimeLeft_String)

        ' Online users
        Try
            Dim users As List(Of OnlineUser) = Nothing
            Dim msg As String = ""

            If Not Form1.Client.FetchOnline(users, msg) Then
                MessageBox.Show(msg)
                Return
            End If

            onlineUsersField.Items.Clear()
            For Each u In users
                onlineUsersField.Items.Add(u.credential)
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub closeBtn_Click(sender As Object, e As EventArgs) Handles closeBtn.Click
        Environment.Exit(0)
    End Sub

    Private Sub minBtn_Click(sender As Object, e As EventArgs) Handles minBtn.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    ' ================= CHAT SEND =================
    Private Sub sendMsgBtn_Click(sender As Object, e As EventArgs) Handles sendMsgBtn.Click
        Dim msg As String = ""

        If Form1.Client.ChatSend(chatMsgField.Text, channel, msg) Then
            chatroomGrid.Rows.Insert(0,
                Form1.Client.CurrentUser.username,
                chatMsgField.Text,
                DateTime.Now)
            chatMsgField.Clear()
        Else
            MessageBox.Show(Form1.Client.LastResponseMessage)
        End If
    End Sub

    ' ================= USER VAR =================
    Private Sub fetchUserVarBtn_Click(sender As Object, e As EventArgs) Handles fetchUserVarBtn.Click
        Dim val = Form1.Client.GetVar(varField.Text)

        If val Is Nothing Then
            MessageBox.Show(Form1.Client.RisponceCollection)
            Return
        End If

        MessageBox.Show(val)
    End Sub

    Private Sub setUserVarBtn_Click(sender As Object, e As EventArgs) Handles setUserVarBtn.Click
        If Not Form1.Client.SetVar(varField.Text, varDataField.Text) Then
            MessageBox.Show(Form1.Client.RisponceCollection)
            Return
        End If

        MessageBox.Show("Updated!")
    End Sub

    ' ================= DOWNLOAD =================
    Private Sub downloadFileBtn_Click(sender As Object, e As EventArgs) Handles downloadFileBtn.Click
        Dim bytes() As Byte = Nothing
        Dim msg As String = ""

        If Not Form1.Client.Download("823785F2", bytes, msg) Then
            MessageBox.Show(msg)
            Return
        End If

        Try
            Dim fullPath = Path.Combine(filePathField.Text, fileExtensionField.Text)
            File.WriteAllBytes(fullPath, bytes)
            MessageBox.Show("Downloaded!")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ' ================= GLOBAL VAR =================
    Private Sub fetchGlobalVariableBtn_Click(sender As Object, e As EventArgs) Handles fetchGlobalVariableBtn.Click
        Dim val = Form1.Client.GetGlobalVar(globalVariableField.Text)

        If val Is Nothing Then
            MessageBox.Show(Form1.Client.RisponceCollection)
            Return
        End If

        MessageBox.Show("Global var value: " & val)
    End Sub
    ' ================= LOG =================
    Private Sub sendLogDataBtn_Click(sender As Object, e As EventArgs) Handles sendLogDataBtn.Click
        Dim msg As String = ""
        If Not Form1.Client.Log(logDataField.Text, msg) Then
            MessageBox.Show(msg)
        Else
            MessageBox.Show(msg)
        End If
    End Sub

    ' ================= BAN =================
    Private Sub banBtn_Click(sender As Object, e As EventArgs) Handles banBtn.Click
        Dim msg As String = ""
        If Form1.Client.Ban("Cheating detected", msg) Then
            MessageBox.Show(msg, "Banned")
            Application.Exit()
        Else
            MessageBox.Show(msg)
        End If
    End Sub


    ' ================= CHECK SESSION =================
    Private Sub checkSessionBtn_Click(sender As Object, e As EventArgs) Handles checkSessionBtn.Click
        If Form1.Client.Check() Then
            MessageBox.Show(Form1.Client.RisponceCollection)
        End If
    End Sub

    ' ================= BLACKLIST =================
    Private Sub CheackBlacklistBtn_Click(sender As Object, e As EventArgs) Handles CheackBlacklistBtn.Click
        Dim msg As String = ""
        If Not Form1.Client.CheckBlacklist(msg) Then
            MessageBox.Show(msg)
        Else
            MessageBox.Show(msg)
        End If
    End Sub

    ' ================= CHAT FETCH =================
    Private Async Sub Timer1_Tick(sender As Object, e As EventArgs) Handles timer1.Tick
        timer1.Interval = 15000

        Try
            Dim messages = Await Form1.Client.ChatFetch(channel)

            chatroomGrid.Rows.Clear()

            If messages Is Nothing OrElse messages.Count = 0 Then
                chatroomGrid.Rows.Insert(0, "AuthVaultix", "No messages", DateTime.Now)
                Return
            End If

            For Each m In messages
                chatroomGrid.Rows.Insert(0,
                    m.author,
                    m.message,
                    DateTimeOffset.FromUnixTimeSeconds(m.timestamp).DateTime)
            Next

        Catch ex As Exception
            timer1.Stop()
            MessageBox.Show("Chat error: " & ex.Message)
        End Try
    End Sub
End Class
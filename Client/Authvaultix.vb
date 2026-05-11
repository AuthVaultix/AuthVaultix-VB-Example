Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Net.Security
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Principal
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Newtonsoft.Json

Namespace AuthVaultix
    Public Class AuthVaultixClient
        Private ReadOnly _core As AuthVaultixCore

        Public Sub New(appName As String, ownerId As String, secret As String, version As String)
            _core = New AuthVaultixCore(appName, ownerId, secret, version)
        End Sub

        Public ReadOnly Property RisponceCollection As String
            Get
                Return _core.RisponceCollection
            End Get
        End Property
        Public ReadOnly Property LastMessage1 As String
            Get
                Return _core.LastMessage1
            End Get
        End Property
        Public ReadOnly Property LastMessage As String
            Get
                Return _core.LastMessage
            End Get
        End Property
        Public ReadOnly Property LastResponseMessage As String
            Get
                Return _core.LastResponseMessage
            End Get
        End Property
        Public ReadOnly Property CurrentUser As UserInfo
            Get
                Return _core.CurrentUser
            End Get
        End Property
        Public ReadOnly Property UserData As UserInfo
            Get
                Return _core.UserData
            End Get
        End Property
        Public ReadOnly Property UseFullKey As Boolean
            Get
                Return _core.UseFullKey
            End Get
        End Property
        Public ReadOnly Property SessionId As String
            Get
                Return _core.SessionId
            End Get
        End Property
        Public ReadOnly Property Initialized As Boolean
            Get
                Return _core.Initialized
            End Get
        End Property

        Public Function Init() As Boolean
            Return _core.InitializeContext()
        End Function
        Public Function Login(username As String, password As String) As Boolean
            Return _core.AuthenticateUser(username, password)
        End Function
        Public Function Check() As Boolean
            Return _core.ValidateSession()
        End Function
        Public Function Register(username As String, password As String, licenseKey As String, Optional email As String = "") As Boolean
            Return _core.RegisterAccount(username, password, licenseKey, email)
        End Function
        Public Function LicenseLogin(licenseKey As String) As Boolean
            Return _core.LicenseAccess(licenseKey)
        End Function
        Public Function Log(message As String, ByRef serverMessage As String) As Boolean
            Return _core.SendLog(message, serverMessage)
        End Function
        Public Function Download(fileId As String, ByRef fileBytes As Byte(), ByRef serverMessage As String) As Boolean
            Return _core.RetrieveFile(fileId, fileBytes, serverMessage)
        End Function
        Public Function FetchOnline(ByRef users As List(Of OnlineUser), ByRef serverMessage As String) As Boolean
            Return _core.GetOnlineClients(users, serverMessage)
        End Function
        Public Function Ban(reason As String, ByRef serverMessage As String) As Boolean
            Return _core.EnforceBan(reason, serverMessage)
        End Function
        Public Sub Logout()
            _core.TerminateSession()
        End Sub
        Public Sub ChangeUsername(newUsername As String)
            _core.UpdateUsername(newUsername)
        End Sub
        Public Function CheckBlacklist(ByRef serverMessage As String) As Boolean
            Return _core.VerifyBlacklist(serverMessage)
        End Function
        Public Function ForgotPassword(username As String, email As String) As Boolean
            Return _core.TriggerPasswordReset(username, email)
        End Function
        Public Function Upgrade(username As String, licenseKey As String) As Boolean
            Return _core.ApplyUpgrade(username, licenseKey)
        End Function
        Public Function GetGlobalVar(varKey As String) As String
            Return _core.FetchGlobalVariable(varKey)
        End Function
        Public Function GetVar(varName As String) As String
            Return _core.FetchUserVariable(varName)
        End Function
        Public Function SetVar(varName As String, value As String) As Boolean
            Return _core.UpdateUserVariable(varName, value)
        End Function
        Public Function ChatSend(message As String, channel As String, ByRef serverMessage As String) As Boolean
            Return _core.TransmitChatMessage(message, channel, serverMessage)
        End Function
        Public Function ChatFetch(channel As String) As Task(Of List(Of ChatMessage))
            Return _core.RetrieveChatHistory(channel)
        End Function
    End Class

    Public Class OnlineUser
        Public Property credential As String
    End Class

    Public Class UserInfo
        Public Property username As String
        Public Property ip As String
        Public Property hwid As String
        Public Property createdate As String
        Public Property lastlogin As String
        Public Property subscriptions As Subscription()

        Private Function ParseUnix(value As String) As DateTime?
            Dim ts As Long
            If Long.TryParse(value, ts) Then
                Return DateTimeOffset.FromUnixTimeSeconds(ts).LocalDateTime
            End If
            Return Nothing
        End Function

        Public ReadOnly Property CreationDate As DateTime?
            Get
                Return ParseUnix(createdate)
            End Get
        End Property
        Public ReadOnly Property LastLoginDate As DateTime?
            Get
                Return ParseUnix(lastlogin)
            End Get
        End Property
        Public ReadOnly Property CreationDateFormatted As String
            Get
                Return If(CreationDate IsNot Nothing, CreationDate.Value.ToString("dd/MM/yyyy hh:mm tt"), "Invalid date")
            End Get
        End Property
        Public ReadOnly Property LastLoginFormatted As String
            Get
                Return If(LastLoginDate IsNot Nothing, LastLoginDate.Value.ToString("dd/MM/yyyy hh:mm tt"), "Invalid date")
            End Get
        End Property
    End Class

    Public Class Subscription
        Public Property subscription As String
        Public Property key As String
        Public Property expiry As String
        Public Property timeleft_seconds As Long

        Private ReadOnly Property ExpiryTimestamp As Long?
            Get
                Dim ts As Long
                Return If(Long.TryParse(expiry, ts), ts, CType(Nothing, Long?))
            End Get
        End Property
        Public ReadOnly Property ExpiryDate As DateTime?
            Get
                Return If(ExpiryTimestamp.HasValue, DateTimeOffset.FromUnixTimeSeconds(ExpiryTimestamp.Value).LocalDateTime, CType(Nothing, DateTime?))
            End Get
        End Property
        Public ReadOnly Property ExpiryFormatted As String
            Get
                Return If(ExpiryDate IsNot Nothing, ExpiryDate.Value.ToString("dd/MM/yyyy hh:mm tt"), "Invalid date")
            End Get
        End Property

        Public ReadOnly Property TimeLeft As String
            Get
                If ExpiryDate Is Nothing Then Return "N/A"
                Dim diff = ExpiryDate.Value - DateTime.Now
                If diff.TotalSeconds <= 0 Then Return "Expired"
                Return $"{diff.Days}d {diff.Hours}h {diff.Minutes}m {diff.Seconds}s"
            End Get
        End Property
    End Class

    Public Class ChatMessage
        Public Property author As String
        Public Property role As String
        Public Property message As String
        Public Property timestamp As Long
    End Class

    Public Class AppInfo
        Public Property version As String
        Public Property customerPanelLink As String
    End Class

    Public Class UpgradeUser
        Public Property name As String
    End Class

    Friend Class AuthVaultixCore
        Private ReadOnly _appName As String
        Private ReadOnly _ownerId As String
        Private ReadOnly _secret As String
        Private ReadOnly _version As String
        Private ReadOnly _apiUrl As String = "https://authvaultix.com/api/1.0/"

        Public Property RisponceCollection As String = ""
        Public Property LastMessage1 As String
        Public Property LastMessage As String
        Public Property LastResponseMessage As String
        Public Property CurrentUser As UserInfo
        Public Property UserData As UserInfo
        Public Property UseFullKey As Boolean
        Public Property SessionId As String
        Public Property Initialized As Boolean

        Private _encryptionKey As String

        Public Sub New(appName As String, ownerId As String, secret As String, version As String)
            If String.IsNullOrWhiteSpace(appName) OrElse String.IsNullOrWhiteSpace(ownerId) OrElse String.IsNullOrWhiteSpace(secret) OrElse String.IsNullOrWhiteSpace(version) Then
                Process.Start("https://youtu.be/rJ1x1fYiZoU?si=GffkGIAGupPHWa0x")
                Process.Start("https://authvaultix.com/win/app/")
                Thread.Sleep(2000)
                Diagnostics.Crash("Application not setup correctly." & vbLf & "Please watch the YouTube video for setup.")
            End If
            _appName = appName
            _ownerId = ownerId
            _secret = secret
            _version = version
        End Sub

        Private Sub EnsureReady()
            If Not Initialized Then Diagnostics.Crash("SDK not initialized." & vbLf & "Call Client.Init() before using any API.")
        End Sub

        Public Function InitializeContext() As Boolean
            RisponceCollection = "Initialization failed1"
            If Initialized Then Return True

            Dim iv As String = Guid.NewGuid().ToString("N").Substring(0, 16)
            _encryptionKey = iv & "-" & _secret

            Dim exePath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim hash As String = VaultixCrypto.FileHash(exePath)

            Dim payload = New PayloadBuilder("init") _
                .WithValue("ver", _version) _
                .WithValue("enckey", iv) _
                .WithValue("hash", hash) _
                .WithValue("name", _appName) _
                .WithValue("ownerid", _ownerId) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "init", sig)
            If resp = "Authvaultix_Invalid" Then Diagnostics.Crash("App not found")

            Dim dto = JsonConvert.DeserializeObject(Of DtoInit)(resp)
            If dto Is Nothing Then Diagnostics.Crash("Invalid JSON")
            If Not dto.Success Then Diagnostics.Crash(dto.Msg)

            SessionId = dto.SessId
            Initialized = True
            Console.WriteLine("Session Initialized: " & SessionId)
            Return True
        End Function

        Public Function AuthenticateUser(username As String, password As String) As Boolean
            RisponceCollection = Nothing
            EnsureReady()

            Dim payload = New PayloadBuilder("login") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("username", username) _
                .WithValue("pass", password) _
                .WithValue("hwid", HardwareIdentifier.Fetch()) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "login", sig)
            Dim dto = JsonConvert.DeserializeObject(Of DtoAuth)(resp)

            If dto Is Nothing OrElse Not dto.Success Then
                RisponceCollection = If(dto?.Msg, "Login failed")
                Return False
            End If

            CurrentUser = dto.Profile
            If Not String.IsNullOrWhiteSpace(dto.SessId) Then SessionId = dto.SessId
            Return True
        End Function

        Public Function ValidateSession() As Boolean
            RisponceCollection = Nothing
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then Diagnostics.Crash("Session missing")

            Dim payload = New PayloadBuilder("check") _
                .WithContext(_appName, _ownerId, SessionId) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "check", sig)
            If resp Is Nothing Then Diagnostics.Crash("Connection failed")
            If String.IsNullOrWhiteSpace(resp) OrElse resp(0) <> "{"c Then Diagnostics.Crash("Invalid response format")

            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing Then Diagnostics.Crash("Invalid JSON")
            If Not dto.Success Then Diagnostics.Crash(If(dto.Msg, "Session check failed"))

            RisponceCollection = dto.Msg
            LastMessage = RisponceCollection
            LastMessage1 = RisponceCollection
            Return True
        End Function

        Public Function RegisterAccount(username As String, password As String, licenseKey As String, email As String) As Boolean
            RisponceCollection = Nothing
            EnsureReady()

            Dim payload = New PayloadBuilder("register") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("username", username) _
                .WithValue("pass", password) _
                .WithValue("key", licenseKey) _
                .WithValue("email", email) _
                .WithValue("hwid", HardwareIdentifier.Fetch()) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "register", sig)
            Dim dto = JsonConvert.DeserializeObject(Of DtoAuth)(resp)

            If dto Is Nothing OrElse Not dto.Success Then
                RisponceCollection = dto?.Msg
                Return False
            End If

            CurrentUser = dto.Profile
            If Not String.IsNullOrWhiteSpace(dto.SessId) Then SessionId = dto.SessId
            Return True
        End Function

        Public Function LicenseAccess(licenseKey As String) As Boolean
            EnsureReady()
            Dim payload = New PayloadBuilder("license") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("key", licenseKey) _
                .WithValue("hwid", HardwareIdentifier.Fetch()) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "license", sig)
            Dim dto = JsonConvert.DeserializeObject(Of DtoAuth)(resp)

            If dto Is Nothing OrElse Not dto.Success Then
                RisponceCollection = dto?.Msg
                Return False
            End If

            CurrentUser = dto.Profile
            If Not String.IsNullOrWhiteSpace(dto.SessId) Then SessionId = dto.SessId
            Return True
        End Function

        Public Function SendLog(message As String, ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            Dim payload = New PayloadBuilder("log") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("message", message) _
                .WithValue("pcuser", Environment.UserName) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "log", sig)
            If String.IsNullOrWhiteSpace(resp) Then
                serverMessage = "Log request failed (no response)."
                Return False
            End If
            If resp(0) <> "{"c Then
                serverMessage = resp.Trim()
                Return False
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If
            If Not dto.Success Then
                serverMessage = If(dto.Msg, "Log failed")
                Return False
            End If
            LastMessage = dto.Msg
            serverMessage = dto.Msg
            Return True
        End Function

        Public Function RetrieveFile(fileId As String, ByRef fileBytes As Byte(), ByRef serverMessage As String) As Boolean
            fileBytes = Nothing
            serverMessage = Nothing
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If
            If String.IsNullOrWhiteSpace(fileId) Then
                serverMessage = "Invalid file id."
                Return False
            End If

            Dim payload = New PayloadBuilder("file") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("fileid", fileId) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "file", sig)
            If String.IsNullOrWhiteSpace(resp) Then
                serverMessage = "Download request failed (no response)."
                Return False
            End If
            If resp(0) <> "{"c Then
                serverMessage = resp.Trim()
                Return False
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoData)(resp)
            If dto Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If

            LastMessage = dto.Msg
            LastMessage1 = dto.Msg

            If Not dto.Success Then
                serverMessage = If(dto.Msg, "Download failed")
                Return False
            End If
            If String.IsNullOrWhiteSpace(dto.B64Data) Then
                serverMessage = "File content missing"
                Return False
            End If

            Try
                fileBytes = Convert.FromBase64String(dto.B64Data)
                serverMessage = If(dto.Msg, "Download successful")
                Return True
            Catch ex As FormatException
                serverMessage = "Invalid file encoding (base64)"
                Return False
            End Try
        End Function

        Public Function GetOnlineClients(ByRef users As List(Of OnlineUser), ByRef serverMessage As String) As Boolean
            users = Nothing
            serverMessage = Nothing
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            Dim payload = New PayloadBuilder("fetchonline") _
                .WithContext(_appName, _ownerId, SessionId) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "fetchonline", sig)
            If String.IsNullOrWhiteSpace(resp) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If
            If resp(0) <> "{"c Then
                serverMessage = resp.Trim()
                Return False
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoOnline)(resp)
            If dto Is Nothing Then
                serverMessage = "Invalid server response."
                Return False
            End If
            If Not dto.Success Then
                serverMessage = If(dto.Msg, "Failed to fetch online users.")
                Return False
            End If

            users = If(dto.UserList, New List(Of OnlineUser)())
            serverMessage = If(dto.Msg, "OK")
            Return True
        End Function

        Public Function EnforceBan(reason As String, ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If
            If String.IsNullOrWhiteSpace(reason) Then reason = "No reason provided"

            Dim payload = New PayloadBuilder("ban") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("reason", reason) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "ban", sig)
            If String.IsNullOrWhiteSpace(resp) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If
            If resp(0) <> "{"c Then
                serverMessage = resp.Trim()
                Return False
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If

            LastMessage = dto.Msg
            LastMessage1 = dto.Msg

            If Not dto.Success Then
                serverMessage = If(dto.Msg, "Ban failed")
                Return False
            End If

            serverMessage = If(dto.Msg, "Banned")
            Return True
        End Function

        Public Sub TerminateSession()
            EnsureReady()
            Dim payload = New PayloadBuilder("logout") _
                .WithContext(_appName, _ownerId, SessionId) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "logout", sig)
            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing OrElse Not dto.Success Then Throw New Exception(If(dto?.Msg, "Logout Error"))

            SessionId = Nothing
            Initialized = False
            Console.WriteLine("Logged out successfully")
        End Sub

        Public Sub UpdateUsername(newUsername As String)
            EnsureReady()
            If String.IsNullOrWhiteSpace(newUsername) Then Throw New Exception("New username cannot be empty")

            Dim payload = New PayloadBuilder("changeusername") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("newUsername", newUsername) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "changeusername", sig)
            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing OrElse Not dto.Success Then Throw New Exception(If(dto?.Msg, "Change username Error"))

            SessionId = Nothing
            Initialized = False
            Console.WriteLine("Username changed successfully, user logged out.")
        End Sub

        Public Function VerifyBlacklist(ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            Dim payload = New PayloadBuilder("checkblacklist") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("hwid", HardwareIdentifier.Fetch()) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "checkblacklist", sig)
            If String.IsNullOrWhiteSpace(resp) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If
            If resp(0) <> "{"c Then
                serverMessage = resp.Trim()
                Return False
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If

            LastMessage = dto.Msg
            LastMessage1 = dto.Msg

            If Not dto.Success Then
                serverMessage = If(dto.Msg, "Client is blacklisted")
                Return False
            End If

            serverMessage = If(dto.Msg, "Client is not blacklisted")
            Return True
        End Function

        Public Function TriggerPasswordReset(username As String, email As String) As Boolean
            EnsureReady()
            Dim payload = New PayloadBuilder("forgot") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("username", username) _
                .WithValue("email", email) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "forgot", sig)
            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)

            If dto Is Nothing OrElse Not dto.Success Then
                RisponceCollection = If(dto?.Msg, "Failed")
                Return False
            End If

            Console.WriteLine("Reset email sent successfully")
            Return True
        End Function

        Public Function ApplyUpgrade(username As String, licenseKey As String) As Boolean
            EnsureReady()
            Dim payload = New PayloadBuilder("upgrade") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("username", username) _
                .WithValue("key", licenseKey) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "upgrade", sig)
            Dim dto = JsonConvert.DeserializeObject(Of DtoUpgrade)(resp)

            If dto Is Nothing OrElse Not dto.Success Then
                RisponceCollection = If(dto?.Msg, "Upgrade Error")
                Return False
            End If

            Console.WriteLine("Upgrade successful: " & If(dto.Upgraded IsNot Nothing AndAlso dto.Upgraded.Count > 0, dto.Upgraded(0).name, "Unknown"))
            Return True
        End Function

        Public Function FetchGlobalVariable(varKey As String) As String
            RisponceCollection = ""
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                RisponceCollection = "Session missing. Please login again."
                Return Nothing
            End If
            If String.IsNullOrWhiteSpace(varKey) Then
                RisponceCollection = "Invalid variable key."
                Return Nothing
            End If

            Dim payload = New PayloadBuilder("var") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("varid", varKey) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "var", sig)
            If String.IsNullOrWhiteSpace(resp) OrElse resp(0) <> "{"c Then
                RisponceCollection = "Invalid server response."
                Return Nothing
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing OrElse Not dto.Success Then
                RisponceCollection = If(dto?.Msg, "Failed to fetch variable.")
                Return Nothing
            End If

            RisponceCollection = "OK"
            Return dto.Msg
        End Function

        Public Function FetchUserVariable(varName As String) As String
            RisponceCollection = ""
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                RisponceCollection = "Session missing. Please login again."
                Return Nothing
            End If
            If String.IsNullOrWhiteSpace(varName) Then
                RisponceCollection = "Invalid variable name."
                Return Nothing
            End If

            Dim payload = New PayloadBuilder("getvar") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("var", varName) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "getvar", sig)
            If String.IsNullOrWhiteSpace(resp) OrElse resp(0) <> "{"c Then
                RisponceCollection = If(resp?.Trim(), "Request failed.")
                Return Nothing
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoVar)(resp)
            If dto Is Nothing OrElse Not dto.Success Then
                RisponceCollection = If(dto?.Msg, "Failed to get variable.")
                Return Nothing
            End If

            RisponceCollection = If(dto.Msg, "OK")
            Return dto.VarData
        End Function

        Public Function UpdateUserVariable(varName As String, value As String) As Boolean
            RisponceCollection = ""
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                RisponceCollection = "Session missing. Please login again."
                Return False
            End If
            If String.IsNullOrWhiteSpace(varName) Then
                RisponceCollection = "Invalid variable name."
                Return False
            End If

            Dim payload = New PayloadBuilder("setvar") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("var", varName) _
                .WithValue("data", If(value, String.Empty)) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "setvar", sig)
            If String.IsNullOrWhiteSpace(resp) OrElse resp(0) <> "{"c Then
                RisponceCollection = If(resp?.Trim(), "Request failed.")
                Return False
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoBasic)(resp)
            If dto Is Nothing Then
                RisponceCollection = "Invalid server response."
                Return False
            End If

            RisponceCollection = If(dto.Msg, If(dto.Success, "OK", "Failed"))
            LastMessage = RisponceCollection
            LastMessage1 = RisponceCollection

            Return dto.Success
        End Function

        Public Function TransmitChatMessage(message As String, channel As String, ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            EnsureReady()
            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If
            If String.IsNullOrWhiteSpace(message) Then
                serverMessage = "Message cannot be empty."
                Return False
            End If
            If String.IsNullOrWhiteSpace(channel) Then
                serverMessage = "Invalid channel."
                Return False
            End If

            Dim payload = New PayloadBuilder("chatsend") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("message", message) _
                .WithValue("channel", channel) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "chatsend", sig)
            If String.IsNullOrWhiteSpace(resp) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If
            If resp(0) <> "{"c Then
                serverMessage = resp.Trim()
                LastResponseMessage = serverMessage
                Return False
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoChat)(resp)
            If dto Is Nothing Then
                serverMessage = "Invalid server response."
                Return False
            End If

            LastResponseMessage = dto.Msg

            If Not dto.Success Then
                If dto.ErrCode = 403 AndAlso dto.RemainingSec > 0 Then
                    serverMessage = $"Muted till {dto.MutedTime} (wait {dto.MutedHuman})"
                    LastResponseMessage = serverMessage
                    Return False
                End If
                serverMessage = If(dto.Msg, "Failed to send message.")
                Return False
            End If

            serverMessage = If(dto.Msg, "Message sent.")
            Return True
        End Function

        Public Function RetrieveChatHistory(channel As String) As Task(Of List(Of ChatMessage))
            EnsureReady()
            LastResponseMessage = Nothing
            If String.IsNullOrWhiteSpace(SessionId) Then
                LastResponseMessage = "Session missing. Please login again."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If
            If String.IsNullOrWhiteSpace(channel) Then
                LastResponseMessage = "Invalid channel."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            Dim payload = New PayloadBuilder("chatfetch") _
                .WithContext(_appName, _ownerId, SessionId) _
                .WithValue("channel", channel) _
                .Compile()

            Dim sig As String = Nothing
            Dim resp As String = NetworkAgent.Post(_apiUrl, payload, _encryptionKey, "chatfetch", sig)
            If String.IsNullOrWhiteSpace(resp) Then
                LastResponseMessage = "Request failed. Please try again."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If
            If resp(0) <> "{"c Then
                LastResponseMessage = resp.Trim()
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            Dim dto = JsonConvert.DeserializeObject(Of DtoChatHistory)(resp)
            If dto Is Nothing Then
                LastResponseMessage = "Invalid server response."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            If Not dto.Success Then
                LastResponseMessage = If(dto.Msg, "Failed to fetch chat messages.")
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            LastResponseMessage = If(dto.Msg, "OK")
            Return Task.FromResult(If(dto.Log, New List(Of ChatMessage)()))
        End Function
    End Class

    Friend Class PayloadBuilder
        Private ReadOnly _nvc As NameValueCollection

        Public Sub New(actionType As String)
            _nvc = New NameValueCollection From {{"type", actionType}}
        End Sub

        Public Function WithContext(appName As String, ownerId As String, sessionId As String) As PayloadBuilder
            _nvc("name") = appName
            _nvc("ownerid") = ownerId
            If Not String.IsNullOrEmpty(sessionId) Then _nvc("sessionid") = sessionId
            Return Me
        End Function

        Public Function WithValue(key As String, value As String) As PayloadBuilder
            If value IsNot Nothing Then _nvc(key) = value
            Return Me
        End Function

        Public Function Compile() As NameValueCollection
            Return _nvc
        End Function
    End Class

    Friend Class NetworkAgent
        Public Shared Function Post(url As String, payload As NameValueCollection, encKey As String, actionType As String, ByRef signature As String) As String
            signature = String.Empty
            Try
                Using client As New WebClient() With {.Proxy = Nothing}
                    client.Headers.Add("User-Agent", "AuthVaultixClient/1.0")
                    ServicePointManager.ServerCertificateValidationCallback = AddressOf SecureSslValidation
                    Dim responseBytes As Byte() = client.UploadValues(url, payload)
                    Dim rawResponse As String = Encoding.UTF8.GetString(responseBytes)
                    signature = client.ResponseHeaders("signature")

                    ServicePointManager.ServerCertificateValidationCallback = Function(sender, cert, chain, errors) True

                    If Not VaultixCrypto.Verify(rawResponse, signature, actionType, encKey) Then
                        Diagnostics.Crash("Signature verification failed. Request tampered")
                        Return Nothing
                    End If
                    Return rawResponse
                End Using
            Catch wex As WebException
                Dim resp As HttpWebResponse = TryCast(wex.Response, HttpWebResponse)
                If resp IsNot Nothing AndAlso resp.StatusCode = CType(429, HttpStatusCode) Then
                    Diagnostics.Crash("You're connecting too fast, slow down.")
                Else
                    Diagnostics.Crash("Connection failure or network error.")
                End If
                Return Nothing
            End Try
        End Function

        Private Shared Function SecureSslValidation(sender As Object, cert As X509Certificate, chain As X509Chain, errors As SslPolicyErrors) As Boolean
            If (Not cert.Issuer.Contains("Cloudflare") AndAlso Not cert.Issuer.Contains("Google") AndAlso Not cert.Issuer.Contains("Let's Encrypt")) OrElse errors <> SslPolicyErrors.None Then
                Diagnostics.Crash("SSL assertion failed. Possible MITM or proxy.")
                Return False
            End If
            Return True
        End Function
    End Class

    Friend NotInheritable Class VaultixCrypto
        Public Shared Function Verify(payload As String, serverSig As String, type As String, key As String) As Boolean
            If type = "log" OrElse type = "file" Then Return True
            If String.IsNullOrEmpty(serverSig) Then Return False

            Dim signingKey As String = If(type = "init", key.Substring(17, 64), key)
            Dim localSig As String = GenerateHmac(signingKey, payload)
            Return CryptographicEquals(localSig, serverSig)
        End Function

        Private Shared Function GenerateHmac(key As String, data As String) As String
            Using hmac As New HMACSHA256(Encoding.UTF8.GetBytes(key))
                Dim hash As Byte() = hmac.ComputeHash(Encoding.UTF8.GetBytes(data))
                Return BitConverter.ToString(hash).Replace("-", "").ToLower()
            End Using
        End Function

        Private Shared Function CryptographicEquals(a As String, b As String) As Boolean
            If a.Length <> b.Length Then Return False
            Dim r As Integer = 0
            For i As Integer = 0 To a.Length - 1
                r = r Or (AscW(a(i)) Xor AscW(b(i)))
            Next
            Return r = 0
        End Function

        Public Shared Function FileHash(path As String) As String
            Using sha = SHA256.Create()
                Using fs = File.OpenRead(path)
                    Return BitConverter.ToString(sha.ComputeHash(fs)).Replace("-", "").ToLower()
                End Using
            End Using
        End Function
    End Class

    Friend NotInheritable Class HardwareIdentifier
        Public Shared Function Fetch() As String
            Dim raw As String = String.Join("|", Environment.MachineName, Environment.UserName, Environment.UserDomainName, Environment.OSVersion.VersionString, If(Environment.Is64BitOperatingSystem, "x64", "x86"), Environment.Version.ToString(), CultureInfo.CurrentCulture.Name, WindowsIdentity.GetCurrent().User.Value)
            Using sha = SHA256.Create()
                Dim bytes As Byte() = sha.ComputeHash(Encoding.UTF8.GetBytes(raw))
                Dim sb As New StringBuilder()
                For Each b As Byte In bytes
                    sb.Append(b.ToString("X2"))
                Next
                Dim hex As String = sb.ToString()

                Dim formatted As New StringBuilder()
                For i As Integer = 0 To hex.Length - 1 Step 4
                    If i > 0 Then formatted.Append("-")
                    formatted.Append(hex.Substring(i, Math.Min(4, hex.Length - i)))
                Next
                Return formatted.ToString()
            End Using
        End Function
    End Class

    Friend NotInheritable Class Diagnostics
        <DllImport("kernel32.dll")>
        Private Shared Function AllocConsole() As Boolean
        End Function

        Public Shared Sub Crash(exceptionDetail As String)
            Try
                File.AppendAllText("auth_diagnostics.txt", $"[{DateTime.Now}] FATAL: {exceptionDetail}" & vbLf)
            Catch
            End Try

            AllocConsole()
            Dim stdOut = Console.OpenStandardOutput()
            Using writer As New StreamWriter(stdOut) With {.AutoFlush = True}
                Console.SetOut(writer)
                Console.SetError(writer)
                Console.Title = "System Halt"
                Console.ForegroundColor = ConsoleColor.DarkRed
                Console.WriteLine("=======================================")
                Console.WriteLine("SUBSYSTEM FAILURE")
                Console.WriteLine(exceptionDetail)
                Console.WriteLine("=======================================")
                Console.ResetColor()
                Thread.Sleep(3000)
            End Using
            Environment.Exit(1)
        End Sub
    End Class

    Friend Class DtoBasic
        <JsonProperty("success")> Public Property Success As Boolean
        <JsonProperty("message")> Public Property Msg As String
    End Class

    Friend Class DtoInit
        Inherits DtoBasic
        <JsonProperty("sessionid")> Public Property SessId As String
        <JsonProperty("appinfo")> Public Property AppInfo As AppInfo
    End Class

    Friend Class DtoAuth
        Inherits DtoBasic
        <JsonProperty("info")> Public Property Profile As UserInfo
        <JsonProperty("sessionid")> Public Property SessId As String
    End Class

    Friend Class DtoData
        Inherits DtoBasic
        <JsonProperty("contents")> Public Property B64Data As String
    End Class

    Friend Class DtoVar
        Inherits DtoBasic
        <JsonProperty("response")> Public Property VarData As String
    End Class

    Friend Class DtoOnline
        Inherits DtoBasic
        <JsonProperty("users")> Public Property UserList As List(Of OnlineUser)
    End Class

    Friend Class DtoChat
        Inherits DtoBasic
        <JsonProperty("code")> Public Property ErrCode As Integer
        <JsonProperty("remaining_seconds")> Public Property RemainingSec As Integer
        <JsonProperty("muted_until")> Public Property MutedTime As String
        <JsonProperty("remaining_human")> Public Property MutedHuman As String
    End Class

    Friend Class DtoChatHistory
        Inherits DtoBasic
        <JsonProperty("messages")> Public Property Log As List(Of ChatMessage)
    End Class

    Friend Class DtoUpgrade
        Inherits DtoBasic
        <JsonProperty("users")> Public Property Upgraded As List(Of UpgradeUser)
    End Class
End Namespace

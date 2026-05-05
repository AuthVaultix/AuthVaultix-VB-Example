Imports Microsoft.Win32
Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms



Namespace AuthVaultix
    Public Class AuthVaultixClient
        Private Const URL As String = "https://authvaultix.com/api/v1/"

        Private AppName As String
        Private OwnerId As String
        Private Secret As String
        Private ApiUrl As String
        Private Version As String

        Public Sub New(appName As String, ownerId As String, secret As String, version As String)

            If String.IsNullOrWhiteSpace(appName) OrElse
            String.IsNullOrWhiteSpace(ownerId) OrElse
            String.IsNullOrWhiteSpace(secret) OrElse
            String.IsNullOrWhiteSpace(version) Then
                MsgBox("Fields cannot be empty")
                Exit Sub
            End If

            If ownerId.Length <> 10 Then
                KillNow("Application not setup correctly.\nPlease watch the YouTube video for setup.")
                Exit Sub
            End If

            If secret.Length <> 64 Then
                KillNow("Application not setup correctly.\nPlease watch the YouTube video for setup.")
                Exit Sub
            End If

            Me.AppName = appName
            Me.OwnerId = ownerId
            Me.Secret = secret
            Me.Version = version
            Me.ApiUrl = URL

        End Sub

        Public Property RisponceCollection As String = ""

        Public Function Init() As Boolean
            RisponceCollection = "Initialization failed1"

            If Initialized Then Return True

            Dim sentKey As String = GenerateIV()
            EncKey = sentKey & "-" & Secret

            Dim exePath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim exeHash As String = GetFileHash(exePath)

            Dim data As New NameValueCollection From {
                {"type", "init"},
                {"name", AppName},
                {"ownerid", OwnerId},
                {"ver", Version},
                {"enckey", sentKey},
                {"hash", exeHash}
            }

            Dim response As String = Request(ApiUrl, data, Nothing)

            If response = "Authvaultix_Invalid" Then
                KillNow("App not found")
            End If

            Dim json = JsonConvert.DeserializeObject(Of InitResponse)(response)

            If json Is Nothing Then
                KillNow("Invalid JSON")
            End If

            If Not json.success Then
                KillNow(json.message)
            End If

            SessionId = json.sessionid
            Initialized = True

            Console.WriteLine("Session Initialized: " & SessionId)
            Return True
        End Function

        ' ======================
        ' LOGIN
        ' ======================
        Public Function Login(username As String, password As String) As Boolean
            RisponceCollection = Nothing
            InitGuard.EnsureInitialized(Initialized)

            Dim hwid As String = SID.Get()

            Dim data As New NameValueCollection From {
                {"type", "login"},
                {"username", username},
                {"pass", password},
                {"hwid", hwid},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim response As String = Request(ApiUrl, data, Nothing)
            Dim json = JsonConvert.DeserializeObject(Of Response)(response)

            If Not json.success Then
                RisponceCollection = If(json.message, "Login failed")
                Return False
            End If

            CurrentUser = json.info

            If Not String.IsNullOrWhiteSpace(json.sessionid) Then
                SessionId = json.sessionid
            End If

            Return True
        End Function

        ' ======================
        ' CHECK SESSION
        ' ======================
        Public Function Check() As Boolean
            RisponceCollection = Nothing
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                KillNow("Session missing")
            End If

            Dim data As New NameValueCollection From {
                {"type", "check"},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If response Is Nothing Then
                KillNow("Connection failed")
            End If

            If String.IsNullOrWhiteSpace(response) OrElse response(0) <> "{"c Then
                KillNow("Invalid response format")
            End If

            Dim json = JsonConvert.DeserializeObject(Of CheckResponse)(response)

            If json Is Nothing Then
                KillNow("Invalid JSON")
            End If

            If Not json.success Then
                KillNow(If(json.message, "Session check failed"))
            End If

            RisponceCollection = json.message
            LastMessage = RisponceCollection
            LastMessage1 = RisponceCollection

            Return True
        End Function

        Private Sub KillNow(reason As String)
            MessageBox.Show(reason, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Environment.Exit(0)
        End Sub

        ' ======================
        ' REGISTER
        ' ======================
        Public Function Register(username As String, password As String, licenseKey As String, Optional email As String = "") As Boolean
            RisponceCollection = Nothing
            InitGuard.EnsureInitialized(Initialized)

            Dim hwid As String = SID.Get()

            Dim data As New NameValueCollection From {
                {"type", "register"},
                {"username", username},
                {"pass", password},
                {"key", licenseKey},
                {"email", email},
                {"hwid", hwid},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim response As String = Request(ApiUrl, data, Nothing)
            Dim json = JsonConvert.DeserializeObject(Of LoginResponse)(response)

            If Not json.success Then
                RisponceCollection = json.message
                Return False
            End If

            CurrentUser = json.info

            If Not String.IsNullOrWhiteSpace(json.sessionid) Then
                SessionId = json.sessionid
            End If

            Return True
        End Function

        ' ======================
        ' LICENSE LOGIN/AUTO REGISTER
        ' ======================
        Public Function LicenseLogin(licenseKey As String) As Boolean
            InitGuard.EnsureInitialized(Initialized)

            Dim hwid As String = SID.Get()

            Dim data As New NameValueCollection From {
                {"type", "license"},
                {"key", licenseKey},
                {"hwid", hwid},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim response As String = Request(ApiUrl, data, Nothing)
            Dim json = JsonConvert.DeserializeObject(Of LoginResponse)(response)

            If Not json.success Then
                RisponceCollection = json.message
                Return False
            End If

            CurrentUser = json.info

            If Not String.IsNullOrWhiteSpace(json.sessionid) Then
                SessionId = json.sessionid
            End If

            Return True
        End Function

        ' ======================
        ' LOG
        ' ======================
        Public Function Log(message As String, ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            Dim data As New NameValueCollection From {
                {"type", "log"},
                {"message", message},
                {"pcuser", Environment.UserName},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If String.IsNullOrWhiteSpace(response) Then
                serverMessage = "Log request failed (no response)."
                Return False
            End If

            If response(0) <> "{"c Then
                serverMessage = response.Trim()
                Return False
            End If

            Dim json = JsonConvert.DeserializeObject(Of BasicResponse)(response)

            If json Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If

            If Not json.success Then
                serverMessage = If(json.message, "Log failed")
                Return False
            End If

            LastMessage = json.message
            serverMessage = json.message
            Return True
        End Function

        ' ======================
        ' DOWNLOAD FILE
        ' ======================
        Public Function Download(fileId As String, ByRef fileBytes As Byte(), ByRef serverMessage As String) As Boolean
            fileBytes = Nothing
            serverMessage = Nothing
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            If String.IsNullOrWhiteSpace(fileId) Then
                serverMessage = "Invalid file id."
                Return False
            End If

            Dim data As New NameValueCollection From {
                {"type", "file"},
                {"fileid", fileId},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If String.IsNullOrWhiteSpace(response) Then
                serverMessage = "Download request failed (no response)."
                Return False
            End If

            If response(0) <> "{"c Then
                serverMessage = response.Trim()
                Return False
            End If

            Dim json = JsonConvert.DeserializeObject(Of DownloadResponse)(response)

            If json Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If

            LastMessage = json.message
            LastMessage1 = json.message

            If Not json.success Then
                serverMessage = If(json.message, "Download failed")
                Return False
            End If

            If String.IsNullOrWhiteSpace(json.contents) Then
                serverMessage = "File content missing"
                Return False
            End If

            Try
                fileBytes = Convert.FromBase64String(json.contents)
                serverMessage = If(json.message, "Download successful")
                Return True
            Catch ex As FormatException
                serverMessage = "Invalid file encoding (base64)"
                Return False
            End Try
        End Function

        ' ======================
        ' FETCH ONLINE USERS
        ' ======================
        Public Function FetchOnline(ByRef users As List(Of OnlineUser), ByRef serverMessage As String) As Boolean
            users = Nothing
            serverMessage = Nothing
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            Dim data As New NameValueCollection From {
                {"type", "fetchonline"},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If String.IsNullOrWhiteSpace(response) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If

            If response(0) <> "{"c Then
                serverMessage = response.Trim()
                Return False
            End If

            Dim json = JsonConvert.DeserializeObject(Of FetchOnlineResponse)(response)

            If json Is Nothing Then
                serverMessage = "Invalid server response."
                Return False
            End If

            If Not json.success Then
                serverMessage = If(json.message, "Failed to fetch online users.")
                Return False
            End If

            users = If(json.users, New List(Of OnlineUser)())
            serverMessage = If(json.message, "OK")
            Return True
        End Function

        ' ======================
        ' BAN USER (SELF BAN / CURRENT SESSION)
        ' ======================
        Public Function Ban(reason As String, ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            If String.IsNullOrWhiteSpace(reason) Then reason = "No reason provided"

            Dim data As New NameValueCollection From {
                {"type", "ban"},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId},
                {"reason", reason}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If String.IsNullOrWhiteSpace(response) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If

            If response(0) <> "{"c Then
                serverMessage = response.Trim()
                Return False
            End If

            Dim json = JsonConvert.DeserializeObject(Of BanResponse)(response)

            If json Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If

            LastMessage = json.message
            LastMessage1 = json.message

            If Not json.success Then
                serverMessage = If(json.message, "Ban failed")
                Return False
            End If

            serverMessage = If(json.message, "Banned")
            Return True
        End Function

        ' ======================
        ' LOGOUT
        ' ======================
        Public Sub Logout()
            InitGuard.EnsureInitialized(Initialized)

            Dim data As New NameValueCollection From {
                {"type", "logout"},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)
            Dim json = JsonConvert.DeserializeObject(Of LogoutResponse)(response)

            If Not json.success Then
                Throw New Exception(json.message)
            End If

            SessionId = Nothing
            Initialized = False

            Console.WriteLine("Logged out successfully")
        End Sub

        ' ======================
        ' CHANGE USERNAME
        ' ======================
        Public Sub ChangeUsername(newUsername As String)
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(newUsername) Then
                Throw New Exception("New username cannot be empty")
            End If

            Dim data As New NameValueCollection From {
                {"type", "changeusername"},
                {"sessionid", SessionId},
                {"newUsername", newUsername},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)
            Dim json = JsonConvert.DeserializeObject(Of ChangeUsernameResponse)(response)

            If Not json.success Then
                Throw New Exception(json.message)
            End If

            SessionId = Nothing
            Initialized = False

            Console.WriteLine("Username changed successfully, user logged out.")
        End Sub

        ' ======================
        ' CHECK BLACKLIST
        ' ======================
        Public Function CheckBlacklist(ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                serverMessage = "Session missing. Please login again."
                Return False
            End If

            Dim hwid As String = SID.Get()

            Dim data As New NameValueCollection From {
                {"type", "checkblacklist"},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId},
                {"hwid", hwid}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If String.IsNullOrWhiteSpace(response) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If

            If response(0) <> "{"c Then
                serverMessage = response.Trim()
                Return False
            End If

            Dim json = JsonConvert.DeserializeObject(Of BlacklistResponse)(response)

            If json Is Nothing Then
                serverMessage = "Invalid server response"
                Return False
            End If

            LastMessage = json.message
            LastMessage1 = json.message

            If Not json.success Then
                serverMessage = If(json.message, "Client is blacklisted")
                Return False
            End If

            serverMessage = If(json.message, "Client is not blacklisted")
            Return True
        End Function

        ' ======================
        ' FORGOT PASSWORD
        ' ======================
        Public Function ForgotPassword(username As String, email As String) As Boolean
            InitGuard.EnsureInitialized(Initialized)

            Dim data As New NameValueCollection From {
                {"type", "forgot"},
                {"username", username},
                {"email", email},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim response As String = Request(ApiUrl, data, Nothing)
            Dim json = JsonConvert.DeserializeObject(Of ForgotPasswordResponse)(response)

            If Not json.success Then
                RisponceCollection = json.message
                Return False
            End If

            Console.WriteLine("Reset email sent successfully")
            Return True
        End Function

        ' ======================
        ' UPGRADE
        ' ======================
        Public Function Upgrade(username As String, licenseKey As String) As Boolean
            InitGuard.EnsureInitialized(Initialized)

            Dim data As New NameValueCollection From {
                {"type", "upgrade"},
                {"username", username},
                {"key", licenseKey},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim response As String = Request(ApiUrl, data, Nothing)
            Dim json = JsonConvert.DeserializeObject(Of UpgradeResponse)(response)

            If Not json.success Then
                RisponceCollection = json.message
                Return False
            End If

            Console.WriteLine("Upgrade successful: " & json.users(0).name)
            Return True
        End Function

        ' ======================
        ' GET GLOBAL VAR
        ' ======================
        Public Function GetGlobalVar(varKey As String) As String
            RisponceCollection = ""
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                RisponceCollection = "Session missing. Please login again."
                Return Nothing
            End If

            If String.IsNullOrWhiteSpace(varKey) Then
                RisponceCollection = "Invalid variable key."
                Return Nothing
            End If

            Dim data As New NameValueCollection From {
                {"type", "var"},
                {"sessionid", SessionId},
                {"varid", varKey},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim sig As String = Nothing
            Dim response As String = Request(ApiUrl, data, sig)

            If String.IsNullOrWhiteSpace(response) OrElse response(0) <> "{"c Then
                RisponceCollection = "Invalid server response."
                Return Nothing
            End If

            Dim json = JsonConvert.DeserializeObject(Of GlobalVarResponse)(response)

            If json Is Nothing OrElse Not json.success Then
                RisponceCollection = If(json?.message, "Failed to fetch variable.")
                Return Nothing
            End If

            RisponceCollection = "OK"
            Return json.message
        End Function

        ' ======================
        ' GET USER VARIABLE
        ' ======================
        Public Function GetVar(varName As String) As String
            RisponceCollection = ""
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                RisponceCollection = "Session missing. Please login again."
                Return Nothing
            End If

            If String.IsNullOrWhiteSpace(varName) Then
                RisponceCollection = "Invalid variable name."
                Return Nothing
            End If

            Dim data As New NameValueCollection From {
                {"type", "getvar"},
                {"var", varName},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim sig As String = Nothing
            Dim response As String = Request(ApiUrl, data, sig)

            If String.IsNullOrWhiteSpace(response) OrElse response(0) <> "{"c Then
                RisponceCollection = If(response?.Trim(), "Request failed.")
                Return Nothing
            End If

            Dim json = JsonConvert.DeserializeObject(Of GetVarResponse)(response)

            If json Is Nothing OrElse Not json.success Then
                RisponceCollection = If(json?.message, "Failed to get variable.")
                Return Nothing
            End If

            RisponceCollection = If(json.message, "OK")
            Return json.response
        End Function

        ' ======================
        ' SET USER VARIABLE
        ' ======================
        Public Function SetVar(varName As String, value As String) As Boolean
            RisponceCollection = ""
            InitGuard.EnsureInitialized(Initialized)

            If String.IsNullOrWhiteSpace(SessionId) Then
                RisponceCollection = "Session missing. Please login again."
                Return False
            End If

            If String.IsNullOrWhiteSpace(varName) Then
                RisponceCollection = "Invalid variable name."
                Return False
            End If

            Dim data As New NameValueCollection From {
                {"type", "setvar"},
                {"var", varName},
                {"data", If(value, String.Empty)},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim sig As String = Nothing
            Dim response As String = Request(ApiUrl, data, sig)

            If String.IsNullOrWhiteSpace(response) OrElse response(0) <> "{"c Then
                RisponceCollection = If(response?.Trim(), "Request failed.")
                Return False
            End If

            Dim json = JsonConvert.DeserializeObject(Of BasicResponse)(response)

            If json Is Nothing Then
                RisponceCollection = "Invalid server response."
                Return False
            End If

            RisponceCollection = If(json.message, If(json.success, "OK", "Failed"))
            LastMessage = RisponceCollection
            LastMessage1 = RisponceCollection

            Return json.success
        End Function

        ' ======================
        ' CHAT SEND
        ' ======================
        Public Function ChatSend(message As String, channel As String, ByRef serverMessage As String) As Boolean
            serverMessage = Nothing
            InitGuard.EnsureInitialized(Initialized)

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

            Dim data As New NameValueCollection From {
                {"type", "chatsend"},
                {"message", message},
                {"channel", channel},
                {"sessionid", SessionId},
                {"name", AppName},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If String.IsNullOrWhiteSpace(response) Then
                serverMessage = "Request failed. Please try again."
                Return False
            End If

            If response(0) <> "{"c Then
                serverMessage = response.Trim()
                LastResponseMessage = serverMessage
                Return False
            End If

            Dim json = JsonConvert.DeserializeObject(Of ChatResponse)(response)

            If json Is Nothing Then
                serverMessage = "Invalid server response."
                Return False
            End If

            LastResponseMessage = json.message

            If Not json.success Then
                If json.code = 403 AndAlso json.remaining_seconds > 0 Then
                    serverMessage = $"Muted till {json.muted_until} (wait {json.remaining_human})"
                    LastResponseMessage = serverMessage
                    Return False
                End If

                serverMessage = If(json.message, "Failed to send message.")
                Return False
            End If

            serverMessage = If(json.message, "Message sent.")
            Return True
        End Function

        Public Function ChatFetch(channel As String) As Task(Of List(Of ChatMessage))
            InitGuard.EnsureInitialized(Initialized)
            LastResponseMessage = Nothing

            If String.IsNullOrWhiteSpace(SessionId) Then
                LastResponseMessage = "Session missing. Please login again."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            If String.IsNullOrWhiteSpace(channel) Then
                LastResponseMessage = "Invalid channel."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            Dim data As New NameValueCollection From {
                {"type", "chatfetch"},
                {"channel", channel},
                {"sessionid", SessionId},
                {"ownerid", OwnerId}
            }

            Dim signature As String = Nothing
            Dim response As String = Request(ApiUrl, data, signature)

            If String.IsNullOrWhiteSpace(response) Then
                LastResponseMessage = "Request failed. Please try again."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            If response(0) <> "{"c Then
                LastResponseMessage = response.Trim()
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            Dim json = JsonConvert.DeserializeObject(Of ChatFetchResponse)(response)

            If json Is Nothing Then
                LastResponseMessage = "Invalid server response."
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            If Not json.success Then
                LastResponseMessage = If(json.message, "Failed to fetch chat messages.")
                Return Task.FromResult(New List(Of ChatMessage)())
            End If

            LastResponseMessage = If(json.message, "OK")
            Return Task.FromResult(If(json.messages, New List(Of ChatMessage)()))
        End Function

        Private Function Request(url As String, data As NameValueCollection, ByRef signature As String) As String
            Try
                Using client As New WebClient()
                    client.Proxy = Nothing
                    client.Headers.Add("User-Agent", "AuthVaultixClient/1.0")

                    ' ✅ FIXED SSL BYPASS
                    ServicePointManager.ServerCertificateValidationCallback =
        Function(sender, cert, chain, sslPolicyErrors) True

                    Dim raw As Byte() = client.UploadValues(url, data)
                    Dim response As String = Encoding.UTF8.GetString(raw)
                    signature = client.ResponseHeaders("signature")

                    ' remove handler (important)


                    Dim type As String = data("type")
                    If Not VerifySignature(response, signature, type) Then
                        ErrorHandler.Error("Signature verification failed. Request tampered")
                        Return Nothing
                    End If

                    Return response
                End Using

            Catch webex As WebException
                If TypeOf webex.Response Is HttpWebResponse Then
                    Dim resp = DirectCast(webex.Response, HttpWebResponse)

                    If resp.StatusCode = CType(429, HttpStatusCode) Then
                        ErrorHandler.Error("You're connecting too fast, slow down.")
                    Else
                        ErrorHandler.Error("Connection failure. Please try again later.")
                    End If
                Else
                    ErrorHandler.Error("Network error. Check internet or firewall.")
                End If

                signature = Nothing
                Return Nothing
            End Try
        End Function

        Private Shared Function AssertSSL(sender As Object, certificate As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
            If (Not certificate.Issuer.Contains("Cloudflare") AndAlso Not certificate.Issuer.Contains("Google") AndAlso Not certificate.Issuer.Contains("Let's Encrypt")) OrElse sslPolicyErrors <> System.Net.Security.SslPolicyErrors.None Then
                ErrorHandler.Error("SSL assertion failed. Possible MITM or proxy.")
                Return False
            End If
            Return True
        End Function

        Private Function VerifySignature(body As String, serverSignature As String, type As String) As Boolean
            If type = "log" OrElse type = "file" Then Return True
            If String.IsNullOrEmpty(serverSignature) Then Return False

            Dim signKey As String = If(type = "init", EncKey.Substring(17, 64), EncKey)
            Dim localSig As String = HashHMAC(signKey, body)
            Return FixedTimeEquals.CheckStringsFixedTime(localSig, serverSignature)
        End Function

        Public Class ErrorHandler
            <DllImport("kernel32.dll")>
            Private Shared Function AllocConsole() As Boolean
            End Function

            Private Shared ReadOnly LogFile As String = "error_logs.txt"

            Public Shared Sub [Error](msg As String)
                AllocConsole()
                Dim stdOut = Console.OpenStandardOutput()
                Dim writer As New StreamWriter(stdOut) With {.AutoFlush = True}
                Console.SetOut(writer)
                Console.SetError(writer)

                Try
                    Dim log As String = "==============================" & vbLf & "TIME: " & DateTime.Now.ToString() & vbLf & "ERROR: " & msg & vbLf & "==============================" & vbLf & vbLf
                    File.AppendAllText(LogFile, log)
                Catch
                End Try

                Console.Title = "AuthVaultix - Error"
                Console.ForegroundColor = ConsoleColor.Red

                Console.WriteLine("=======================================")
                Console.WriteLine("AUTHVAULTIX ERROR")
                Console.WriteLine("=======================================")
                Console.WriteLine(msg)
                Console.WriteLine("=======================================")
                Console.ResetColor()
                Console.WriteLine()

                For i As Integer = 4 To 1 Step -1
                    Console.Write(vbCr & "Exiting in " & i & " seconds...")
                    Thread.Sleep(1000)
                Next

                Environment.Exit(0)
            End Sub
        End Class

        Public Class FixedTimeEquals
            Public Shared Function CheckStringsFixedTime(str1 As String, str2 As String) As Boolean
                If str1.Length <> str2.Length Then Return False

                Dim result As Integer = 0
                For i As Integer = 0 To str1.Length - 1
                    result = result Or (AscW(str1(i)) Xor AscW(str2(i)))
                Next

                Return result = 0
            End Function
        End Class

        Private Shared Function HashHMAC(key As String, data As String) As String
            Using hmac As New HMACSHA256(Encoding.UTF8.GetBytes(key))
                Dim hash As Byte() = hmac.ComputeHash(Encoding.UTF8.GetBytes(data))
                Return BitConverter.ToString(hash).Replace("-", "").ToLower()
            End Using
        End Function

        Private Shared Function GenerateIV() As String
            Return Guid.NewGuid().ToString("N").Substring(0, 16)
        End Function

        Public Class InitGuard
            Public Shared Sub EnsureInitialized(initialized As Boolean)
                If Not initialized Then
                    ErrorHandler.Error("SDK not initialized." & vbLf & "Call Client.Init() before using any API.")
                End If
            End Sub
        End Class

        Public Shared Function GetFileHash(filePath As String) As String
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
            Using sha256 As System.Security.Cryptography.SHA256 = System.Security.Cryptography.SHA256.Create()
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
                Using stream = File.OpenRead(filePath)
                    Dim hash = sha256.ComputeHash(stream)
                    Return BitConverter.ToString(hash).Replace("-", "").ToLower()
                End Using
            End Using
        End Function

        Public Property LastMessage1 As String
        Public Property CurrentUser As UserInfo
        Public Property LastMessage As String
        Public Property LastResponseMessage As String
        Public Property UserData As UserInfo
        Public Property UseFullKey As Boolean
        Public Property SessionId As String
        Public Property Initialized As Boolean

        Private EncKey As String
    End Class

    Public Class BasicResponse
        Public Property success As Boolean
        Public Property message As String
    End Class

    Public Class DownloadResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property contents As String
    End Class

    Public Class CheckResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property role As String
    End Class

    Public Class GetVarResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property response As String
    End Class

    Public Class FetchOnlineResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property users As List(Of OnlineUser)
    End Class

    Public Class OnlineUser
        Public Property credential As String
    End Class

    Public Class BanResponse
        Public Property success As Boolean
        Public Property message As String
    End Class

    Public Class LogoutResponse
        Public Property success As Boolean
        Public Property message As String
    End Class

    Public Class ChangeUsernameResponse
        Public Property success As Boolean
        Public Property message As String
    End Class

    Public Class BlacklistResponse
        Public Property success As Boolean
        Public Property message As String
    End Class

    Public Class ForgotPasswordResponse
        Public Property success As Boolean
        Public Property message As String
    End Class

    Public Class UpgradeResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property users As UpgradeUser()
    End Class

    Public Class UpgradeUser
        Public Property name As String
    End Class

    Public Class GlobalVarResponse
        Public Property success As Boolean
        Public Property message As String
    End Class

    Public Class ChatResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property code As Integer
        Public Property ownerid As String
        Public Property muted_until As String
        Public Property muted_until_ts As Long
        Public Property remaining_seconds As Integer
        Public Property remaining_minutes As Integer
        Public Property remaining_human As String
        Public Property server_time As String
        Public Property server_time_ts As Long
    End Class

    Public Class Response
        Public Property success As Boolean
        Public Property message As String
        Public Property info As UserInfo
        Public Property sessionid As String
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
                Return If(CreationDate?.ToString("dd/MM/yyyy hh:mm tt"), "Invalid date")
            End Get
        End Property

        Public ReadOnly Property LastLoginFormatted As String
            Get
                Return If(LastLoginDate?.ToString("dd/MM/yyyy hh:mm tt"), "Invalid date")
            End Get
        End Property
    End Class

    Public Class Subscription
        Public Property subscription As String
        Public Property key As String
        Public Property expiry As String
        Public Property timeleft As Long

        Private ReadOnly Property ExpiryTimestamp As Long?
            Get
                Dim ts As Long
                If Long.TryParse(expiry, ts) Then Return ts
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property ExpiryDate As DateTime?
            Get
                If ExpiryTimestamp Is Nothing Then Return Nothing
                Return DateTimeOffset.FromUnixTimeSeconds(ExpiryTimestamp.Value).LocalDateTime
            End Get
        End Property

        Public ReadOnly Property ExpiryFormatted As String
            Get
                Return If(ExpiryDate?.ToString("dd/MM/yyyy hh:mm tt"), "Invalid date")
            End Get
        End Property

        Public ReadOnly Property TimeLeft_String As String
            Get
                If ExpiryDate Is Nothing Then Return "N/A"
                Dim diff = ExpiryDate.Value - DateTime.Now
                If diff.TotalSeconds <= 0 Then Return "Expired"
                Return $"{diff.Days}d {diff.Hours}h {diff.Minutes}m {diff.Seconds}s"
            End Get
        End Property
    End Class

    Public Class LoginResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property info As UserInfo
        Public Property sessionid As String
    End Class

    Public Class ChatMessage
        Public Property author As String
        Public Property role As String
        Public Property message As String
        Public Property timestamp As Long
    End Class

    Public Class ChatFetchResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property messages As List(Of ChatMessage)
    End Class

    Public Class InitResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property sessionid As String
        Public Property ownerid As String
        Public Property appinfo As AppInfo
    End Class

    Public Class AppInfo
        Public Property version As String
        Public Property customerPanelLink As String
    End Class
End Namespace

Public NotInheritable Class SID
    Private Sub New()
    End Sub

    Public Shared Function [Get]() As String
        Dim machine As String = Environment.MachineName
        Dim user As String = Environment.UserName
        Dim domain As String = Environment.UserDomainName
        Dim os As String = Environment.OSVersion.VersionString
        Dim arch As String = If(Environment.Is64BitOperatingSystem, "x64", "x86")
        Dim clr As String = Environment.Version.ToString()
        Dim culture As String = CultureInfo.CurrentCulture.Name
        Dim sidVal As String = WindowsIdentity.GetCurrent().User.Value
        Dim raw As String = String.Join("|", machine, user, domain, os, arch, clr, culture, sidVal)
        Return Sha256Pretty(raw)
    End Function

    Private Shared Function Sha256Pretty(input As String) As String
        Using sha = SHA256.Create()
            Dim bytes As Byte() = sha.ComputeHash(Encoding.UTF8.GetBytes(input))
            Dim sb As New StringBuilder()

            For Each b As Byte In bytes
                sb.Append(b.ToString("X2"))
            Next

            Return SplitEvery(sb.ToString(), 4, "-")
        End Using
    End Function

    Private Shared Function SplitEvery(text As String, size As Integer, separator As String) As String
        Dim sb As New StringBuilder()
        For i As Integer = 0 To text.Length - 1 Step size
            If i > 0 Then sb.Append(separator)
            sb.Append(text.Substring(i, Math.Min(size, text.Length - i)))
        Next
        Return sb.ToString()
    End Function
End Class

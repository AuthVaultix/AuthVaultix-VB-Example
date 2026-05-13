# 🔐 AuthVaultix VB.NET SDK

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Framework: .NET](https://img.shields.io/badge/.NET-Framework%204.7.2%2B-blueviolet)](https://dotnet.microsoft.com/)
[![Language: VB.NET](https://img.shields.io/badge/Language-VB.NET-blue)](https://docs.microsoft.com/en-us/dotnet/visual-basic/)

**AuthVaultix** is a premium, high-performance authentication SDK for VB.NET. It provides a secure bridge between your Windows application and the AuthVaultix cloud, featuring encrypted communication, HWID protection, and a real-time dashboard.

---

## 📂 Project Structure

- `Authvaultix.vb` - Core SDK logic and API client.
- `Form1.vb` - Example Login, Register, and Upgrade interface.
- `Form2.vb` - Full-featured User Dashboard example.
- `Drag.vb` - Utility for creating modern, borderless draggable UI.

---

## ✨ Key Features

- 🚀 **Asynchronous Chat** - Real-time global chat using `Async/Await`.
- 🛡️ **HWID Locking** - Automatic hardware identification to prevent account sharing.
- 🔒 **AES Encryption** - All data packets are encrypted for maximum security.
- 📊 **Variable Control** - Fetch global settings or user-specific data remotely.
- 📂 **Secure File Delivery** - Download files (DLLs, EXEs) directly to memory or disk.
- 🚫 **Security Suite** - Integrated Blacklist and Banning systems.

---

## 🛠️ Getting Started

### 1. Requirements
- **Visual Studio 2019/2022**
- **.NET Framework 4.7.2** or higher.
- **Newtonsoft.Json** (Required for API parsing).

### 2. Integration
Add the following files to your project:
1. `Authvaultix.vb`
2. `Drag.vb` (Optional, for UI)

Install the dependency via NuGet:
```powershell
Install-Package Newtonsoft.Json
```

---

## 🚀 Usage Examples

### Initialization
Define your credentials in your startup form (e.g., `Form1.vb`):

```vbnet
Public Shared Client As New AuthVaultix.AuthVaultixClient(
    "YourAppName",
    "YourOwnerId",
    "YourAppSecret",
    "1.0"
)

Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    If Not Client.Init() Then
        MessageBox.Show("Initialization failed!")
    End If
End Sub
```

### User Login & Session Check
```vbnet
If Client.Login(username, password) Then
    ' Open Dashboard
    Dim dashboard As New Form2()
    dashboard.Show()
Else
    MessageBox.Show("Error: " & Client.RisponceCollection)
End If
```

---

## 🖥️ User Dashboard Features (Form2)

The SDK allows for deep integration with your application's UI.

### User Data & Profile
Access the `CurrentUser` object to display detailed information about the logged-in user:
```vbnet
Dim user = Client.CurrentUser

lblUsername.Text = "Welcome, " & user.username
lblIP.Text = "Your IP: " & user.ip
lblExpiry.Text = "Expires: " & user.subscriptions(0).ExpiryFormatted
lblTimeLeft.Text = "Time Remaining: " & user.subscriptions(0).TimeLeft
```

### Online Users List
Fetch the list of currently active users:
```vbnet
Dim users As List(Of OnlineUser) = Nothing
Dim msg As String = ""

If Client.FetchOnline(users, msg) Then
    For Each u In users
        Console.WriteLine("Online: " & u.credential)
    Next
End If
```

### Real-Time Chat
```vbnet
' Fetch messages asynchronously
Dim messages = Await Client.ChatFetch("general")
For Each m In messages
    Console.WriteLine($"[{m.author}]: {m.message}")
Next
```

### Remote Variables
```vbnet
' Get a global variable (e.g., app version, status)
Dim status = Client.GetGlobalVar("status")

' Get a user-specific variable
Dim expiry = Client.GetVar("expiry")
```

### Security Actions
```vbnet
' Check if the user is blacklisted before launching features
If Not Client.CheckBlacklist(msg) Then
    Application.Exit()
End If

' Ban a user for cheating
Client.Ban("Violation of terms", msg)
```

---

## 📖 API Reference

### Authentication & Session
| Method | Return Type | Description |
| :--- | :--- | :--- |
| `Init()` | `Boolean` | Initializes the secure session with the API. |
| `Login(username, password)` | `Boolean` | Authenticates the user and binds HWID. |
| `Register(username, password, key, email?)` | `Boolean` | Creates a new user with a license key. |
| `LicenseLogin(licenseKey)` | `Boolean` | Authenticates directly via license key. |
| `Check()` | `Boolean` | Validates if the current session is still active. |
| `Logout()` | `Void` | Invalidates current session and logs out. |

### Account Management
| Method | Return Type | Description |
| :--- | :--- | :--- |
| `Upgrade(username, licenseKey)` | `Boolean` | Upgrades user's account/subscription. |
| `ForgotPassword(username, email)` | `Boolean` | Triggers a password reset email. |
| `ChangeUsername(newUsername)` | `Void` | Changes the current user's username. |

### Security & Blacklist
| Method | Return Type | Description |
| :--- | :--- | :--- |
| `Ban(reason, ByRef serverMessage)` | `Boolean` | Bans the currently authenticated user. |
| `CheckBlacklist(ByRef serverMessage)` | `Boolean` | Checks if the machine HWID is blacklisted. |
| `Log(message, ByRef serverMessage)` | `Boolean` | Sends a log to the AuthVaultix dashboard. |

### Variables & Data
| Method | Return Type | Description |
| :--- | :--- | :--- |
| `GetGlobalVar(varKey)` | `String` | Fetches a global server-side variable. |
| `GetVar(varName)` | `String` | Fetches a user-specific server-side variable. |
| `SetVar(varName, value)` | `Boolean` | Sets a user-specific variable. |
| `Download(fileId, ByRef fileBytes, ByRef msg)` | `Boolean` | Securely downloads a file into a Byte Array. |

### Communication
| Method | Return Type | Description |
| :--- | :--- | :--- |
| `FetchOnline(ByRef users, ByRef msg)` | `Boolean` | Gets a list of currently online users. |
| `ChatSend(message, channel, ByRef msg)` | `Boolean` | Sends a chat message to a specific channel. |
| `ChatFetch(channel)` | `Task(Of List(Of ChatMessage))` | Asynchronously retrieves chat history. |

---

## 🎨 UI Enhancements
Use the included `Drag.vb` to make any WinForm draggable without borders:
```vbnet
Drag.MakeDraggable(Me)
```

---

## 📜 License
Distributed under the MIT License. See `LICENSE` for more information.

<p align="center">
  Built for developers who demand security and performance.
</p>

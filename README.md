# NetLibCore

<!-- ![https://fengrui358.visualstudio.com/VSTSCI/_apis/build/status/VSTSCI-NetLibCore-Package?branchName=master](https://fengrui358.visualstudio.com/VSTSCI/_apis/build/status/VSTSCI-NetLibCore-Package?branchName=master) -->
[![https://www.nuget.org/packages/FrHello.NetLib.Core/](https://img.shields.io/nuget/dt/FrHello.NetLib.Core)](https://www.nuget.org/packages/FrHello.NetLib.Core/)
[![https://www.nuget.org/packages/FrHello.NetLib.Core/](https://img.shields.io/nuget/v/FrHello.NetLib.Core)](https://www.nuget.org/packages/FrHello.NetLib.Core/)

本项目包含 .net 以及 windows 平台下的一些辅助方法和工具

## 本项目已得到[JetBrains](https://www.jetbrains.com/)的支持

<img src="https://www.jetbrains.com/shop/static/images/jetbrains-logo-inv.svg" height="100">

## NetLib.Core

包含一些常用的基础类型，各种基本操作

## NetLib.Core.Compression

一些压缩算法，ZIP，道格拉斯抽稀等

## NetLib.Core.Framework

### 包含控制台的辅助类

### Excel 操作的辅助方法

读取 Excel 自动填充为一个集合类：

`IEnumerable<T> FillDatas<T>(this ExcelPackage excelPackage)`

保存一个集合数据到 Excel 表：

`Task WriteDatas<T>(IEnumerable<T> rowDatas, string excelPath, bool overWrite = false)`

## NetLib.Core.IO

包含跨局域网访问 Windows 共享目录的方法

## NetLib.Core.Mvx

封装了 `MvvmCross` 的 `ViewModel` 辅助类

## NetLib.Core.Net

### Mail

先设置 `GlobalMailOptions` 里的邮件账号

发送邮件

```csharp
static void Send(string subject, string body, string toAddress)

static void Send(MailMessage mailMessage)
```

### Net

包含 `Ping` 方法，端口检查 `static bool CheckRemotePort(IPAddress address, int port)`，获取本地 IP 地址 `static IPAddress[] GetAllLocalIp(NetworkInterfaceType type, AddressFamily addressFamily = AddressFamily.InterNetwork)`，进度辅助类`ProgressableStreamContent`

## NetLib.Core.Reflection

包括枚举的描述特性

打印类型信息 `static string Output(Type type, bool includeNonPublic = false)`

## NetLib.Core.Security

加密算法集合

## NetLib.Core.Serialization

序列化方法封装，AutoMapper 辅助类

## NetLib.Core.Windows

Windows 平台下使用的一些设备相关方法，如：键盘、鼠标、屏幕

## NetLib.Core.Wpf

基于 `MvvmCross` 的 `WPF` 相关的封装

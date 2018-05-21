rem 供持续集成环境使用的脚本
rem 移除测试项目
dotnet sln NetLibCore.sln remove Tests\NetLib.Core.Test\NetLib.Core.Test.csproj

rem 调整NetLib.Core.Reflection程序集引用
rem 列出包引用
rem dotnet list NetLib.Core.Framework\NetLib.Core.Reflection.csproj reference
rem 移除项目引用
dotnet remove NetLib.Core.Reflection\NetLib.Core.Reflection.csproj reference ..\NetLib.Core\NetLib.Core.csproj
rem 增加包引用
dotnet add NetLib.Core.Reflection\NetLib.Core.Reflection.csproj package FrHello.NetLib.Core

rem 调整NetLib.Core.Framework.csproj程序集引用
rem 列出包引用
rem dotnet list NetLib.Core.Framework\NetLib.Core.Framework.csproj reference
rem 移除项目引用
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.IO\NetLib.Core.IO.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.Serialization\NetLib.Core.Serialization.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.Reflection.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.csproj
rem 增加包引用
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.IO
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Serialization
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Reflection
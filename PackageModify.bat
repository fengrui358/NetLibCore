::NetLib.Core.Framework
::移除测试项目
dotnet sln NetLibCore.sln remove Tests\NetLib.Core.Test\NetLib.Core.Test.csproj

::调整NetLib.Core.Framework.csproj程序集引用
::列出包引用
dotnet list NetLib.Core.Framework\NetLib.Core.Framework.csproj reference
::移除项目引用
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.IO\NetLib.Core.IO.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.Serialization\NetLib.Core.Serialization.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.Reflection.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.csproj
::增加包引用
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.IO
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Serialization
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Reflection
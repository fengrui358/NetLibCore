rem ���������ɻ���ʹ�õĽű�
rem �Ƴ�������Ŀ
dotnet sln NetLibCore.sln remove Tests\NetLib.Core.Test\NetLib.Core.Test.csproj

rem ����NetLib.Core.Reflection��������
rem �г�������
rem dotnet list NetLib.Core.Framework\NetLib.Core.Reflection.csproj reference
rem �Ƴ���Ŀ����
dotnet remove NetLib.Core.Reflection\NetLib.Core.Reflection.csproj reference ..\NetLib.Core\NetLib.Core.csproj
rem ���Ӱ�����
dotnet add NetLib.Core.Reflection\NetLib.Core.Reflection.csproj package FrHello.NetLib.Core

rem ����NetLib.Core.Framework.csproj��������
rem �г�������
rem dotnet list NetLib.Core.Framework\NetLib.Core.Framework.csproj reference
rem �Ƴ���Ŀ����
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.IO\NetLib.Core.IO.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.Serialization\NetLib.Core.Serialization.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.Reflection.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.csproj
rem ���Ӱ�����
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.IO
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Serialization
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Reflection
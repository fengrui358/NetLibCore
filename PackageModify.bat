::NetLib.Core.Framework
::�Ƴ�������Ŀ
dotnet sln NetLibCore.sln remove Tests\NetLib.Core.Test\NetLib.Core.Test.csproj

::����NetLib.Core.Framework.csproj��������
::�г�������
dotnet list NetLib.Core.Framework\NetLib.Core.Framework.csproj reference
::�Ƴ���Ŀ����
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.IO\NetLib.Core.IO.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core.Serialization\NetLib.Core.Serialization.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.Reflection.csproj
dotnet remove NetLib.Core.Framework\NetLib.Core.Framework.csproj reference ..\NetLib.Core\NetLib.Core.csproj
::���Ӱ�����
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.IO
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Serialization
dotnet add NetLib.Core.Framework\NetLib.Core.Framework.csproj package FrHello.NetLib.Core.Reflection
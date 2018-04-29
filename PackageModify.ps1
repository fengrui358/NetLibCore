# 查找解决方案sln文件
$slns = Get-ChildItem -Filter "*.sln" -Recurse
$tests = Get-ChildItem -Filter "*test.csproj" -Recurse
$allAssemblys = Get-ChildItem -Filter "*.csproj" -Recurse | Where-Object {$_.Name -notlike "*test*"}

foreach($sln in $slns)
{
    foreach($test in $tests)
    {
        # 清理测试程序集，移除测试程序集
        dotnet sln $sln.FullName remove $test.FullName
    }
}

foreach($assembly in $allAssemblys)
{
    # 查找所有项目引用，判断引用是否在内部引用范围
    $references = dotnet list $assembly.FullName reference
    if($references -is [array])
    {
        foreach($reference in $references)
        {
            $isLocalAssemby = ($allAssemblys | Where-Object{$_.Name -eq $reference}).Length > 0
            $ea=$isLocalAssemby
        }
    }
}
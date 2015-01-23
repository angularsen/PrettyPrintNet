$root="$PSScriptRoot\.."
$nugetDir="$root\Artifacts\NuGet\"

$list = gci -Path $nugetDir -Filter PrettyPrintNet.*.nupkg | where-object {$_.Name -NotMatch "\.symbols\."}
foreach ($file in $list)
{
  write-host "Publishing nuget: $($file.Name)"
  Invoke-Expression "$root\Tools\nuget.exe push $($file.FullName)"
}
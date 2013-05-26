; requires autoit3 to run/compile: autoitscript.com

#include <Constants.au3>

$ini_file = @ScriptDir & "\versions.ini"

$git_command = "git describe --dirty --always --long --all --abbrev"

$git = Run($git_command, @ScriptDir, default, $STDOUT_CHILD)
$output = StdoutRead($git)
While ProcessExists($git)
	$output &= StdoutRead($git)
WEnd
$full_version = StringStripWS($output, 3)
; heads/csharp-0-gbe12132-dirty
$version = $full_version
$version_array = StringSplit($version, "-")
$dirty = False
If $version_array[$version_array[0]] = "dirty" Then
	$dirty = True
	$version = StringTrimRight($version, StringLen("-dirty"))
	$version_array = StringSplit($version, "-")
EndIf
$commit_hash = StringTrimLeft($version_array[$version_array[0]], 1)
$commits = $version_array[$version_array[0] - 1]
$tag = StringTrimRight($version, StringLen("-" & $commits & "-g" & $commit_hash))

ConsoleWrite("Version: " & $full_version & @CRLF)
ConsoleWrite("Tag: " & $tag & @CRLF)
ConsoleWrite("Commits: " & $commits & @CRLF)
ConsoleWrite("Commit hash: " & $commit_hash & @CRLF)
ConsoleWrite("Dirty: " & $dirty & @CRLF)

If $CmdLine[0] = 0 Then
	$num_projects = IniRead($ini_file, "projects", "num_projects", "1")
	IniWrite($ini_file, "projects", "num_projects", $num_projects)
	For $i = 1 To $num_projects
		$project_name = IniRead($ini_file, "projects", "project_" & $i, "SuperEngine")
		IniWrite($ini_file, "projects", "project_" & $i, $project_name)
		WriteVersion($project_name)
	Next
Else
	For $i = 1 To $CmdLine[0]
		$project_name = $CmdLine[$i]
		WriteVersion($project_name)
	Next
EndIf
ConsoleWrite("Version update complete." & @CRLF)

Func WriteVersion($project)
	ConsoleWrite("Updating version for project " & $project & @CRLF)
	$major = IniRead($ini_file, $project, "major", "1")
	$minor = IniRead($ini_file, $project, "minor", "0")
	$assembly_build = IniRead($ini_file, $project, "assembly_build", "0")
	$assembly_revision = IniRead($ini_file, $project, "assembly_revision", "0")
	$revsion_auto_inc = IniRead($ini_file, $project, "revsion_auto_inc", "1")
	$revision = IniRead($ini_file, $project, "revision", "-1")
	$out_file = IniRead($ini_file, $project, "out_file", $project & "\Properties\AssemblyVersion.cs")

	If $revsion_auto_inc Then
		$revision += 1
	EndIf
	If $revision < 0 Then
		$revision = 0
	EndIf
	If $revsion_auto_inc And Not $dirty Then
		$revision = 0
	EndIf

	ConsoleWrite("Assembly Version: " & $major & "." & $minor & "." & $assembly_build & "." & $assembly_revision & @CRLF)
	ConsoleWrite("Assembly FileVersion: " & $major & "." & $minor & "." & $commits & "." & $revision & @CRLF)
	ConsoleWrite("Assembly Informational Version: " & $full_version & "-" & $revision & @CRLF)

	IniWrite($ini_file, $project, "major", $major)
	IniWrite($ini_file, $project, "minor", $minor)
	IniWrite($ini_file, $project, "assembly_build", $assembly_build)
	IniWrite($ini_file, $project, "assembly_revision", $assembly_revision)
	IniWrite($ini_file, $project, "revsion_auto_inc", $revsion_auto_inc)
	IniWrite($ini_file, $project, "revision", $revision)
	IniWrite($ini_file, $project, "out_file", $out_file)

	FileDelete($out_file)
	FileWriteLine($out_file, 'using System.Reflection;')
	FileWriteLine($out_file, '[assembly: AssemblyVersion("' & $major & "." & $minor & "." & $assembly_build & "." & $assembly_revision & '")]')
	FileWriteLine($out_file, '[assembly: AssemblyFileVersion("' & $major & "." & $minor & "." & $commits & "." & $revision & '")]')
	FileWriteLine($out_file, '[assembly: AssemblyInformationalVersion("' & $full_version & "-" & $revision & '")]')
EndFunc
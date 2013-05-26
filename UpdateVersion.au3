; requires autoit3 to run/compile: autoitscript.com

#include <Constants.au3>

$ini_file = @ScriptDir & "\versions.ini"



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

Func WriteVersion($project, $git_command_all = False)
	If Not $git_command_all Then
		ConsoleWrite("Updating version for project " & $project & @CRLF)
	EndIf

	$git_command = IniRead($ini_file, $project, "git_command", "git describe --dirty --always --long")

	$full_version = ""

	If $git_command_all Then
		$git_command &= " --all"
	Else
		While StringRight($git_command, StringLen(" --all")) = " --all"
			$git_command = StringTrimRight($git_command, StringLen(" --all"))
		WEnd
	EndIf

	$tries = 0
	While $full_version = "" And $tries < 100
		$tries += 1
		$git = Run($git_command, @ScriptDir, default, $STDOUT_CHILD)
		$output = StdoutRead($git)
		While ProcessExists($git)
			$output &= StdoutRead($git)
		WEnd
		$full_version = StringStripWS($output, 3)
	WEnd
	$version = $full_version

	$remove_tag_prefix = IniRead($ini_file, $project, "remove_tag_prefix", "")
	If StringLeft($version, StringLen($remove_tag_prefix)) = $remove_tag_prefix Then
		$version = StringTrimLeft($version, StringLen($remove_tag_prefix))
	EndIf

	$version_array = StringSplit($version, "-")
	$dirty = False
	If $version_array[$version_array[0]] = "dirty" Then
		$dirty = True
		$version = StringTrimRight($version, StringLen("-dirty"))
		$version_array = StringSplit($version, "-")
	EndIf
	$commit_hash = $version_array[$version_array[0]]
	If StringLeft($commit_hash, 1) = "g" Then
		$commit_hash = StringTrimLeft($commit_hash, 1)
	EndIf
	If $version_array[0] = 1 Then
		$commits = 0
		$tag = ""
		If Not $git_command_all Then
			WriteVersion($project, True)
			Return
		EndIf
	Else
		$commits = $version_array[$version_array[0] - 1]
		If StringRight($version, StringLen("-" & $commits & "-g" & $commit_hash)) = "-" & $commits & "-g" & $commit_hash Then
			$tag = StringTrimRight($version, StringLen("-" & $commits & "-g" & $commit_hash))
		Else
			$tag = ""
		EndIf
	EndIf

	ConsoleWrite(@TAB & "Version: " & $full_version & @CRLF)
	ConsoleWrite(@TAB & "Tag: " & $tag & @CRLF)
	ConsoleWrite(@TAB & "Commits: " & $commits & @CRLF)
	ConsoleWrite(@TAB & "Commit hash: " & $commit_hash & @CRLF)
	ConsoleWrite(@TAB & "Dirty: " & $dirty & @CRLF)
	ConsoleWrite(@CRLF)

	$major = IniRead($ini_file, $project, "major", "1")
	$minor = IniRead($ini_file, $project, "minor", "0")
	$assembly_build = IniRead($ini_file, $project, "assembly_build", "0")
	$assembly_revision = IniRead($ini_file, $project, "assembly_revision", "0")
	$revision_auto_inc = IniRead($ini_file, $project, "revision_auto_inc", "1")
	$out_file = IniRead($ini_file, $project, "out_file", $project & "\Properties\AssemblyVersion.cs")

	$revision_line_left = '/* UpdateVersion Revision: '
	$revision_line_right = ' */'
	$revision = FileReadLine($out_file)
	$revision = StringStripWS($revision, 3)
	If StringLeft($revision, StringLen($revision_line_left)) = $revision_line_left Then
		$revision = StringTrimLeft($revision, StringLen($revision_line_left))
		If StringRight($revision, StringLen($revision_line_right)) = $revision_line_right Then
			$revision = StringTrimRight($revision, StringLen($revision_line_right))
		Else
			$revision = 0
		EndIf
	Else
		$revision = 0
	EndIf
	$revision = Int($revision)

	If $revision < -1 Then
		$revision = -1
	EndIf
	If $revision_auto_inc Then
		$revision += 1
	EndIf
	If $revision_auto_inc And Not $dirty Then
		$revision = 0
	EndIf
	$revision = IniRead($ini_file, $project, "revision", $revision)

	ConsoleWrite(@TAB & "Assembly Version: " & $major & "." & $minor & "." & $assembly_build & "." & $assembly_revision & @CRLF)
	ConsoleWrite(@TAB & "Assembly File Version: " & $major & "." & $minor & "." & $commits & "." & $revision & @CRLF)
	ConsoleWrite(@TAB & "Assembly Informational Version: " & $full_version & "-" & $revision & @CRLF)

	While StringRight($git_command, StringLen(" --all")) = " --all"
		$git_command = StringTrimRight($git_command, StringLen(" --all"))
	WEnd

	IniWrite($ini_file, $project, "git_command", $git_command)
	IniWrite($ini_file, $project, "remove_tag_prefix", $remove_tag_prefix)
	IniWrite($ini_file, $project, "major", $major)
	IniWrite($ini_file, $project, "minor", $minor)
	IniWrite($ini_file, $project, "assembly_build", $assembly_build)
	IniWrite($ini_file, $project, "assembly_revision", $assembly_revision)
	IniWrite($ini_file, $project, "revision_auto_inc", $revision_auto_inc)
	IniWrite($ini_file, $project, "out_file", $out_file)

	$out_file_handle = FileOpen($out_file, $FO_OVERWRITE)
	FileWriteLine($out_file_handle, $revision_line_left & $revision & $revision_line_right)
	FileWriteLine($out_file_handle, 'using System.Reflection;')
	FileWriteLine($out_file_handle, '[assembly: AssemblyVersion("' & $major & "." & $minor & "." & $assembly_build & "." & $assembly_revision & '")]')
	FileWriteLine($out_file_handle, '[assembly: AssemblyFileVersion("' & $major & "." & $minor & "." & $commits & "." & $revision & '")]')
	FileWriteLine($out_file_handle, '[assembly: AssemblyInformationalVersion("' & $full_version & "-" & $revision & '")]')
	FileClose($out_file_handle)
	ConsoleWrite(@CRLF)
EndFunc
$Script:UseWriteHost = $true
#http://patorjk.com/software/taag/#p=display&f=Standard&t=Build (standard font)

$Global:asciiArtBuild = @"
  ____        _ _     _  
 |  _ \      (_) |   | | 
 | |_) |_   _ _| | __| | 
 |  _ <| | | | | |/ _  | 
 | |_) | |_| | | | (_| | 
 |____/ \__,_|_|_|\__,_| 
 
"@


$Global:asciiArtNugetPack = @"
  _   _                  _     ____            _    
 | \ | |_   _  __ _  ___| |_  |  _ \ __ _  ___| | __
 |  \| | | | |/ _` |/ _ \ __| | |_) / _` |/ __| |/ /
 | |\  | |_| | (_| |  __/ |_  |  __/ (_| | (__|   < 
 |_| \_|\__,_|\__, |\___|\__| |_|   \__,_|\___|_|\_\
              |___/                                 
"@

$Global:asciiArtRestore = @"
  ____           _                 
 |  _ \ ___  ___| |_ ___  _ __ ___ 
 | |_) / _ \/ __| __/ _ \| '__/ _ \
 |  _ <  __/\__ \ || (_) | | |  __/
 |_| \_\___||___/\__\___/|_|  \___| 
"@

if(!$Global:ColorScheme) {
    $Global:ColorScheme = @{
        "Banner"=[ConsoleColor]::Cyan
        "RuntimeName"=[ConsoleColor]::Yellow
        "Help_Header"=[ConsoleColor]::Yellow
        "Help_Switch"=[ConsoleColor]::Green
        "Help_Argument"=[ConsoleColor]::Cyan
        "Help_Optional"=[ConsoleColor]::Gray
        "Help_Command"=[ConsoleColor]::DarkYellow
        "Help_Executable"=[ConsoleColor]::DarkYellow
        "ParameterName"=[ConsoleColor]::Cyan
        "Warning" = [ConsoleColor]::Yellow
    }
}

function _WriteOut {
    param(
        [Parameter(Mandatory=$false, Position=0, ValueFromPipeline=$true)][string]$msg,
        [Parameter(Mandatory=$false)][ConsoleColor]$ForegroundColor,
        [Parameter(Mandatory=$false)][ConsoleColor]$BackgroundColor,
        [Parameter(Mandatory=$false)][switch]$NoNewLine)

    if($__TestWriteTo) {
        $cur = Get-Variable -Name $__TestWriteTo -ValueOnly -Scope Global -ErrorAction SilentlyContinue
        $val = $cur + "$msg"
        if(!$NoNewLine) {
            $val += [Environment]::NewLine
        }
        Set-Variable -Name $__TestWriteTo -Value $val -Scope Global -Force
        return
    }

    if(!$Script:UseWriteHost) {
        if(!$msg) {
            $msg = ""
        }
        if($NoNewLine) {
            [Console]::Write($msg)
        } else {
            [Console]::WriteLine($msg)
        }
    }
    else {
        try {
            if(!$ForegroundColor) {
                $ForegroundColor = $host.UI.RawUI.ForegroundColor
            }
            if(!$BackgroundColor) {
                $BackgroundColor = $host.UI.RawUI.BackgroundColor
            }

            Write-Host $msg -ForegroundColor:$ForegroundColor -BackgroundColor:$BackgroundColor -NoNewLine:$NoNewLine
        } catch {
            $Script:UseWriteHost = $false
            _WriteOut $msg
        }
    }
}

function _WriteConfig{
param(
        [Parameter(Mandatory=$true,Position=0)]$name,
        [Parameter(Mandatory=$true,Position=1)]$value)
		
	_WriteOut -NoNewline -ForegroundColor $Global:ColorScheme.ParameterName "${name}: "
	_WriteOut "$value"

}

function _WriteDiagnostics(){

	_WriteConfig "PATH" "$env:PATH"

}
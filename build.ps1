. ".\build.common.ps1"

$thisFolder = (Get-Item -Path ".\" -Verbose).FullName

function restore{
    _WriteOut -ForegroundColor $ColorScheme.Banner $asciiArtRestore
	dotnet restore $project
}

function build{

    $projectsToBuild = @(
		".\src\server";
        ".\src\common";
	    ".\src\jig";
        ".\client";
    )

    _WriteOut -ForegroundColor $ColorScheme.Banner $asciiArtBuild
    
    foreach($project in $projectsToBuild) {
            
        _WriteConfig "Build" "$project"
        dotnet build $project
	
	    if (!$LastExitCode -eq 0)
	    {
		    Write-Host "Build failed for '$project'"
		    exit 1
	    }
    }
	
}

restore
build
. ".\build.common.ps1"

$thisFolder = (Get-Item -Path ".\" -Verbose).FullName

function build{

    $projectsToBuild = @(
		".\src\server";
	    ".\src\jig";
        ".\client";
    )

    _WriteOut -ForegroundColor $ColorScheme.Banner $asciiArtBuild
    
    foreach($project in $projectsToBuild) {
    
	    _WriteConfig "Restore" "$project"
        dotnet restore $project
        
        _WriteConfig "Build" "$project"
        dotnet build $project
	
	    if (!$LastExitCode -eq 0)
	    {
		    Write-Host "Build failed for '$project'"
		    exit 1
	    }
    }
	
}

build
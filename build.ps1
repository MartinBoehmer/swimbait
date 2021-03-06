. ".\build.common.ps1"

$thisFolder = (Get-Item -Path ".\" -Verbose).FullName

function restore{
    _WriteOut -ForegroundColor $ColorScheme.Banner $asciiArtRestore
	dotnet restore $project
}

function nugetPack{
    _WriteOut -ForegroundColor $ColorScheme.Banner $asciiArtNugetPack
	
	$projectsToPack = @(
        ".\src\musiccast.api";
    )
	
	foreach($project in $projectsToPack) {
            
        _WriteConfig "Build" "$project"
        dotnet pack $project
	
	    if (!$LastExitCode -eq 0)
	    {
		    Write-Host "Build failed for '$project'"
		    exit 1
	    }
    }
}

function build{

    $projectsToBuild = @(
		".\src\swimbait.server";
	    ".\src\swimbait.console";
        ".\src\musiccast.console";
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
nugetPack
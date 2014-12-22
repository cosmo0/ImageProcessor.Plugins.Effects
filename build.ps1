Properties {
	$LOCAL_PATH = Resolve-Path "."
	$BUILD_PATH = Join-Path $LOCAL_PATH "build"
	$TESTS_PATH = Join-Path $BUILD_PATH "tests"
	$NUGET_OUTPUT = Join-Path $BUILD_PATH "nuget"
	$TEST_RESULTS = Join-Path $LOCAL_PATH "tests"
	$NUGET_FOLDER = Join-Path $LOCAL_PATH "src\.nuget"
	$PACKAGES_PATH = Join-Path $LOCAL_PATH "packages"

	[xml]$PROJECTS = Get-Content (Join-Path $LOCAL_PATH "build.xml")
	$NUGET = Join-Path $NUGET_FOLDER "nuget.exe"
	$UNIT_PATH = Join-Path $PACKAGES_PATH "xunit.runners.1.9.2\tools\"
}

$ErrorActionPreference = "Stop"

Framework "4.0x86"
FormatTaskName "-------- {0} --------"

task default -depends Restore-Packages, Publish-Solution, Run-Tests, New-Nugets

# restores Nuget packages
task Restore-Packages {
	# restore tools
	& $NUGET install "$NUGET_FOLDER\packages.config" -OutputDirectory $PACKAGES_PATH

	# restore solution packages
	$PROJECTS.projects.solution | % {
		& $NUGET restore (Join-Path $LOCAL_PATH $_.path)
	}
}

# builds solution
task Publish-Solution -depends Restore-Packages {
	$PROJECTS.projects.solution | % {
		Exec {
			msbuild (Join-Path $LOCAL_PATH $_.path) /t:Build /p:Configuration=Release /p:Warnings=true /v:Normal /nologo /clp:WarningsOnly`;ErrorsOnly`;Summary`;PerformanceSummary
		}
	}
}

# runs unit tests
task Run-Tests -depends Publish-Solution {
	if (-not (Test-Path $TEST_RESULTS)) {
		mkdir $TEST_RESULTS | Out-Null
	}

	$PROJECTS.projects.test | % {
		$dll = Join-Path $TESTS_PATH $_.path
		& (Join-Path $UNIT_PATH $_.runner) $dll /html (Join-Path $TEST_RESULTS $_.results) /noshadow
	}
}

# generates a Nuget package
task New-Nugets -depends Publish-Solution {
	Write-Host "Generating Nuget packages for each project"

	# Nuget doesn't create the output dir automatically...
	if (-not (Test-Path $NUGET_OUTPUT)) {
		Write-Host "Creating folder $NUGET_OUTPUT"
		mkdir $NUGET_OUTPUT | Out-Null
	}

	# Package the nuget
	$PROJECTS.projects.solution.nuget.folder | % {
		Write-Host "Building Nuget package from $_"

		# pack the nuget
		Start-Process -FilePath $NUGET -WorkingDirectory (Join-Path $LOCAL_PATH $_) -ArgumentList "Pack -OutputDirectory $NUGET_OUTPUT" -Wait
	}
}

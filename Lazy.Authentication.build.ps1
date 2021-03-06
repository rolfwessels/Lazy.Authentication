Framework "4.0"

#
# properties
#

properties {
    $buildConfiguration = 'debug'
    $buildDirectory = 'build'
    $buildReportsDirectory =  Join-Path $buildDirectory 'reports'
    $buildPackageDirectory =  Join-Path $buildDirectory 'packages'
    $buildDistDirectory =  Join-Path $buildDirectory 'dist'

    $buildContants = ''

    $srcDirectory = 'src'
    $srcSolution = Join-Path $srcDirectory 'Lazy.Authentication.sln'

    $codeCoverRequired = 70

    $versionMajor = 0
    $versionMinor = 0
    $versionBuild = 1
    $versionRevision = 0

    $vsVersion = "12.0"

    $msdeploy = 'C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe';
    $deployServiceDest = "computerName='xxxx',userName='xxx',password='xxxx',includeAcls='False',tempAgent='false',dirPath='d:\server\temp'"
    $deployApiDest = 'auto,includeAcls="False",tempAgent="false"'
}

#
# task
#

task default -depends build  -Description "By default it just builds"
task clean -depends build.clean,build.cleanbin -Description "Removes build folder"
task build -depends build.cleanbin,version,build.compile -Description "Cleans bin/object and builds the project placing binaries in build directory"
task test -depends build,test.run  -Description "Builds and runs part cover tests"
task full -depends test,deploy.zip -Description "Versions builds and creates distributions"
task package -depends version,build,build.nugetPackages -Description "Creates packages that could be user for deployments"
task publish -depends publish.nuget -Description "Deploy the files to webserver using msdeploy"

#
# task depends
#

task build.clean {
    remove-item -force -recurse $buildDirectory -ErrorAction SilentlyContinue
    $binFolders = Get-ChildItem ($srcDirectory + '\*\*') | where { $_.name -eq 'bin' -or $_.name -eq 'obj'} | Foreach-Object {$_.fullname}
    if ($binFolders -ne $null)
    {
        remove-item $binFolders -force -recurse -ErrorAction SilentlyContinue
    }
}

task build.cleanbin {
    remove-item -force -recurse $buildReportsDirectory -ErrorAction SilentlyContinue
    remove-item -force -recurse (buildConfigDirectory) -ErrorAction SilentlyContinue
    $binFolders = Get-ChildItem $srcDirectory -include bin,obj  | Foreach-Object {$_.fullname}
    if ($binFolders -ne $null)
    {
        remove-item $binFolders -force -recurse -ErrorAction SilentlyContinue
    }
}

task build.compile {
    'Compile '+$buildConfiguration+' version '+(srcBinFolder)
    msbuild  $srcSolution /t:rebuild /p:Configuration=$buildConfiguration /p:VisualStudioVersion=$vsVersion /v:q
}

task version {
    $commonAssemblyInfo = Join-Path $srcDirectory 'Lazy.Authentication.Core/Properties/CommonAssemblyInfo.cs'
    $regEx = 'AssemblyVersion\(.*\)'
    $replace = 'AssemblyVersion("' + (fullversionrev) + '")'
    $replace
    'Set the version in ' +$commonAssemblyInfo
    (gc  $commonAssemblyInfo )  -replace $regEx, $replace |sc $commonAssemblyInfo
}

task nuget.restore {
    ./src/.nuget/NuGet.exe install src\.nuget\packages.config -OutputDirectory lib
}

#-depend build 
task build.nugetPackages {
    $packagesFolder =  $buildDistDirectory
    mkdir $packagesFolder -ErrorAction SilentlyContinue
    ./src/.nuget/NuGet.exe pack src\Lazy.Authentication.OAuth2\Lazy.Authentication.OAuth2.csproj -Prop Configuration=$buildConfiguration
    #copy src\Lazy.Authentication.OAuth2\Lazy.Authentication.OAuth2.nuspec src\Lazy.Authentication.OAuth2.Dal\Lazy.Authentication.OAuth2.Dal.nuspec
    ./src/.nuget/NuGet.exe pack src\Lazy.Authentication.OAuth2.Dal\Lazy.Authentication.OAuth2.Dal.csproj -Prop Configuration=$buildConfiguration
    Move-Item -force *.nupkg $packagesFolder
 }
 
 
task publish.nuget {
    $packagesFolder =  $buildDistDirectory
    mkdir $packagesFolder -ErrorAction SilentlyContinue

   ./src/.nuget/NuGet.exe push  ( Join-Path $packagesFolder ('Lazy.Authentication.OAuth2.'+(fullversionrev)+'.nupkg'))
   ./src/.nuget/NuGet.exe push  ( Join-Path $packagesFolder ('Lazy.Authentication.OAuth2.Dal.'+(fullversionrev)+'.nupkg'))
}

task clean.database {
   $mongoDbLocations = 'D:\Var\mongodb\bin\mongo.exe','C:\mongodb\bin\mongo.exe','C:\bin\MongoDB\bin\mongo.exe','g:\mongodb\bin\mongo.exe'
   $mongo = $mongoDbLocations | Where-Object {Test-Path $_} | Select-Object -first 1
   $database = 'Lazy_Authentication_Develop'
   'Use '+ $mongo + ' to drop the database '+$database
   exec { &($mongo) $database  --eval 'db.dropDatabase()' }
}

task test.run -depend  clean.database, nuget.restore -precondition { return $buildConfiguration -eq 'debug' } {
    mkdir $buildReportsDirectory -ErrorAction SilentlyContinue

    $currentPath = resolve-path '.'
    $partcoverDirectory = resolve-path 'lib\OpenCover.4.6.166\tools'
    $nunitDirectory =  resolve-path 'lib\NUnit.Runners.2.6.4\tools\nunit-console.exe'
    $reportGenerator = 'lib\ReportGenerator.2.3.2.0\tools'

    $partcoverExe = Join-Path $partcoverDirectory 'OpenCover.Console.exe'



    $runTestsTimeout = '60000'
    $runTestsDirectory = '.Tests'
    $runTestsSettings = '/exclude:Unstable /timeout:' + $runTestsTimeout

    $nunit2failed = 'false'
    $hasFailure = $FALSE
    $testFolders = Get-ChildItem $srcDirectory '*.Tests' -Directory
    foreach ($testFolder in $testFolders) {

        $runTestsFolder = Join-Path $testFolder.FullName (srcBinFolder)
        $runTestsFolderDll = Join-Path $runTestsFolder ($testFolder.Name + '.dll')

        $buildReportsDirectoryResolved = '..\..\..\'+ $buildReportsDirectory;
        $runTestsFolderResult =  Join-Path $buildReportsDirectoryResolved ($testFolder.Name + '.xml')
        $runTestsFolderOut =  Join-Path $buildReportsDirectoryResolved ($testFolder.Name + '.txt')
        $runTestsFolderPartResult =  Join-Path $buildReportsDirectoryResolved ($testFolder.Name + '.part.xml')
        '----------------------------------------------'
        $testFolder.Name

        Set-Location $partcoverDirectory

        $target = '-targetargs:'+$runTestsFolderDll+' /nologo /noshadow /out:'+$runTestsFolderOut +' /xml:'+$runTestsFolderResult
        $runTestsFolder

        ./OpenCover.Console.exe -target:$nunitDirectory $target   -register:user -output:$runTestsFolderPartResult -log:Warn
        [xml]$Xml = Get-Content $runTestsFolderResult
        [int]$result= $Xml.'test-results'.failures
        $hasFailure =  $hasFailure -or $result -gt 0

    }

    if ($hasFailure)
    {
        throw "Tests have failed"
    }

    write-host 'Generate report' -foreground "magenta"
    Set-Location $currentPath
    Set-Location $reportGenerator
    $buildReportsDirectoryRelative = Join-Path '..\..\..\' $buildReportsDirectory
    $reports = Join-Path  $buildReportsDirectoryRelative '*.Tests.part.xml'
    $targetdir = Join-Path  $buildReportsDirectoryRelative 'CodeCoverage'
    $reporttypes = 'HTML;HTMLSummary;XMLSummary'
    $filters = '+Lazy.Authentication*;-Lazy.Authentication*Tests';

    ./ReportGenerator.exe -reports:$reports -targetdir:$targetdir -reporttypes:$reporttypes -filters:$filters -verbosity:Error
    Set-Location $currentPath
    write-host 'Validate code coverage' -foreground "magenta"

    $codeCoverSummary = Join-Path $buildReportsDirectory 'CodeCoverage\Summary.xml'
    [xml]$Xml = Get-Content $codeCoverSummary
    [int]$codeCover = $Xml.CoverageReport.Summary.LineCoverage -replace '%', ''
    if ($codeCover -lt $codeCoverRequired) {
        throw 'The solution currently has '+$codeCover+'% coverage, less than the required '+$codeCoverRequired+'%'
    }
}

task deploy.zip {
    mkdir $buildDistDirectory -ErrorAction SilentlyContinue
    $folders = Get-ChildItem (buildConfigDirectory) -Directory
    foreach ($folder in $folders) {
        $version = fullversion
        $zipname = Join-Path $buildDistDirectory ($folder.name + '.v.'+ $version+'.'+$buildConfiguration+'.zip' )
        write-host ('Create '+$zipname)
        ZipFiles $zipname $folder.fullname
    }
}

task deploy.package {
    $version = fullversion
    $mkdirResult = mkdir $buildPackageDirectory  -ErrorAction SilentlyContinue
    $toFolder = Join-Path ( resolve-path $buildPackageDirectory ) "$buildConfiguration.Lazy.Authentication.Api.v.$version.zip"
    $configuration = $buildConfiguration+';Platform=AnyCPU;AutoParameterizationWebConfigConnectionStrings=false;PackageLocation=' + $toFolder + ';EnableNuGetPackageRestore=true'
    $project = Join-Path $srcDirectory 'Lazy.Authentication.Api\Lazy.Authentication.Api.csproj'
    msbuild /v:q  /t:restorepackages  /T:Package  /p:VisualStudioVersion=$vsVersion /p:Configuration=$configuration  /p:PackageTempRootDir=c:\temp  $project
}


task deploy.api -depends deploy.package {
    $deployWebsiteName = 'Default Web Site/Lazy.Authentication.Api.'+$buildConfiguration
    $version = fullversion
    $toFolder = Join-Path ( resolve-path $buildPackageDirectory ) "$buildConfiguration.Lazy.Authentication.Api.v.$version.zip"
    $skip = 'skipAction=Delete,objectName=filePath,absolutePath=Logs'
    $setParam = 'ApplicationPath='+$deployWebsiteName+''
    &($msdeploy) -source:package=$toFolder -dest:$deployApiDest -verb:sync -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -skip:$skip
}

task deploy.service {
    $source = 'dirPath='+( resolve-path (Join-Path (buildConfigDirectory) 'Lazy.Authentication.Console'))
    &($msdeploy) -verb:sync -allowUntrusted -source:$source -dest:$deployServiceDest
    # &($msdeploy) -verb:sync -preSync:runCommand='D:\Dir\on\remote\server\stop-service.cmd',waitInterval=30000 -source:dirPath='C:\dir\of\files\to\be\copied\on\build\server ' -dest:computerName='xx.xx.xx.xx',userName='xx.xx.xx.xx',password='xxxxxxxxxxxxxxx',includeAcls='False',tempAgent='false',dirPath='D:\Dir\on\remote\server\'  -allowUntrusted -postSync:runCommand='D:\Dir\on\remote\server\start-service.cmd',waitInterval=30000
}

task ? -Description "Helper to display task info" {
	WriteDocumentation
}

#
# functions
#

function fullversion() {
    $version = $versionBuild
    if ($env:BUILD_NUMBER) {
        $version = $env:BUILD_NUMBER
    }
    return "$versionMajor.$versionMajor.$version"
}

function fullversionrev() {
    return  (fullversion) + ".$versionRevision"
}


function srcBinFolder() {
    return  Join-Path bin $buildConfiguration
}

function buildConfigDirectory() {
    Join-Path $buildDirectory $buildConfiguration
}



function global:copy-files($source,$destination,$include=@(),$exclude=@()){
    $sourceFullName = resolve-path $source
    $relativePath = Get-Item $source | Resolve-Path -Relative
    $mkdirResult = mkdir $destination -ErrorAction SilentlyContinue
    $files = Get-ChildItem $source -include $include -Recurse -Exclude $exclude
     foreach ($file in $files) {
       $relativePathOfFile = Get-Item $file.FullName | Resolve-Path -Relative
       $tofile = Join-Path $destination $relativePathOfFile.Substring($relativePath.length)
       Copy-Item -Force $relativePathOfFile $tofile
     }
}

function ZipFiles( $zipfilename, $sourcedir )
{
   del $zipfilename -ErrorAction SilentlyContinue
   Add-Type -Assembly System.IO.Compression.FileSystem
   $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
   [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir,
        $zipfilename, $compressionLevel, $false)
}

function WriteDocumentation() {
    $currentContext = $psake.context.Peek()

    if ($currentContext.tasks.default) {
        $defaultTaskDependencies = $currentContext.tasks.default.DependsOn
    } else {
        $defaultTaskDependencies = @()
    }


    $docs = $currentContext.tasks.Keys | foreach-object {
        if ($_ -eq "default" -or $_ -eq "?") {
            return
        }

        if ($_ -contains '.') {
            return
        }

        $task = $currentContext.tasks.$_
        new-object PSObject -property @{
            Name = $task.Name;
            Description = $task.Description;
        }
    }

    $docs | where {-not [string]::IsNullOrEmpty($_.Description)} | sort 'Name' | sort 'Description' -Descending | format-table -autoSize -wrap -property Name,Description

    'Examples:'
    '----------'
    ''
    'Clean build directory before executing build:'
    'go clean,build'
    ''
    ''
    'Qa build:'
    'go build -properties @{''buildConfiguration''=''Qa''}'
     ''
    'Package to nuget:'
    'go package -properties @{''buildConfiguration''=''Release''}'
    ''
    'Staging deploy to sepecified folder:'
    'go deploy -properties @{buildConfiguration=''Staging'';deployServiceDest =''computerName=''''xxxx'''',userName=''''xxx'''',password=''''xxxx'''',includeAcls=''''False'''',tempAgent=''''false'''',dirPath=''''d:\server\temp'''''' }'

}


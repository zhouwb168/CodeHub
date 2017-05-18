path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfGps.Services"
appcmd delete site "Wodeyun.Project.WoodPowerOfGps.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodPowerOfGps.Services"
appcmd set apppool "Wodeyun.Project.WoodPowerOfGps.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPowerOfGps.Services"^
                /bindings:http/*:16016:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPowerOfGps.Services"
appcmd set site "Wodeyun.Project.WoodPowerOfGps.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPowerOfGps.Services"

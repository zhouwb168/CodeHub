path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfGps.Web"
appcmd delete site "Wodeyun.Project.WoodPowerOfGps.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodPowerOfGps.Web"
appcmd set apppool "Wodeyun.Project.WoodPowerOfGps.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPowerOfGps.Web"^
                /bindings:http/*:17016:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPowerOfGps.Web"
appcmd set site "Wodeyun.Project.WoodPowerOfGps.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPowerOfGps.Web"

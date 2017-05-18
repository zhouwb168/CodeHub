path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodGps.Services"
appcmd delete site "Wodeyun.Project.WoodGps.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodGps.Services"
appcmd set apppool "Wodeyun.Project.WoodGps.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodGps.Services"^
                /bindings:http/*:16015:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodGps.Services"
appcmd set site "Wodeyun.Project.WoodGps.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodGps.Services"

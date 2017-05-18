path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmOrigin.Services"
appcmd delete site "Wodeyun.Project.GsmOrigin.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmOrigin.Services"
appcmd set apppool "Wodeyun.Project.GsmOrigin.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmOrigin.Services"^
                /bindings:http/*:14002:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmOrigin.Services"
appcmd set site "Wodeyun.Project.GsmOrigin.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmOrigin.Services"

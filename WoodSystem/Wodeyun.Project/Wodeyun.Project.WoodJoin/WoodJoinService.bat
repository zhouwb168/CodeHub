path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodJoin.Services"
appcmd delete site "Wodeyun.Project.WoodJoin.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodJoin.Services"
appcmd set apppool "Wodeyun.Project.WoodJoin.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodJoin.Services"^
                /bindings:http/*:16012:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodJoin.Services"
appcmd set site "Wodeyun.Project.WoodJoin.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodJoin.Services"

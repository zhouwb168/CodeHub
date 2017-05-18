path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Wood.Services"
appcmd delete site "Wodeyun.Project.Wood.Services"

appcmd add apppool /name:"Wodeyun.Project.Wood.Services"
appcmd set apppool "Wodeyun.Project.Wood.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Wood.Services"^
                /bindings:http/*:16002:^
                /physicalPath:"%cd%\Wodeyun.Project.Wood.Services"
appcmd set site "Wodeyun.Project.Wood.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.Wood.Services"

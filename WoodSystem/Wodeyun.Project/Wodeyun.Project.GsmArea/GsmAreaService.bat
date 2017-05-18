path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmArea.Services"
appcmd delete site "Wodeyun.Project.GsmArea.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmArea.Services"
appcmd set apppool "Wodeyun.Project.GsmArea.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmArea.Services"^
                /bindings:http/*:14001:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmArea.Services"
appcmd set site "Wodeyun.Project.GsmArea.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmArea.Services"

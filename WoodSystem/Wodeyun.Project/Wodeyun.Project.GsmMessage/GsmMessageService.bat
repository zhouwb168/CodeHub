path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmMessage.Services"
appcmd delete site "Wodeyun.Project.GsmMessage.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmMessage.Services"
appcmd set apppool "Wodeyun.Project.GsmMessage.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmMessage.Services"^
                /bindings:http/*:14006:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmMessage.Services"
appcmd set site "Wodeyun.Project.GsmMessage.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmMessage.Services"

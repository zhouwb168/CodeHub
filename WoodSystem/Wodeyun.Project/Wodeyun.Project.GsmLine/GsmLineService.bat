path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmLine.Services"
appcmd delete site "Wodeyun.Project.GsmLine.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmLine.Services"
appcmd set apppool "Wodeyun.Project.GsmLine.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmLine.Services"^
                /bindings:http/*:14003:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmLine.Services"
appcmd set site "Wodeyun.Project.GsmLine.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmLine.Services"

path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Recover.Services"
appcmd delete site "Wodeyun.Project.Recover.Services"

appcmd add apppool /name:"Wodeyun.Project.Recover.Services"
appcmd set apppool "Wodeyun.Project.Recover.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Recover.Services"^
                /bindings:http/*:16004:^
                /physicalPath:"%cd%\Wodeyun.Project.Recover.Services"
appcmd set site "Wodeyun.Project.Recover.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.Recover.Services"

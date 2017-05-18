path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Check.Services"
appcmd delete site "Wodeyun.Project.Check.Services"

appcmd add apppool /name:"Wodeyun.Project.Check.Services"
appcmd set apppool "Wodeyun.Project.Check.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Check.Services"^
                /bindings:http/*:16003:^
                /physicalPath:"%cd%\Wodeyun.Project.Check.Services"
appcmd set site "Wodeyun.Project.Check.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.Check.Services"

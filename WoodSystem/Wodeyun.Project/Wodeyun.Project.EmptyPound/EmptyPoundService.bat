path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.EmptyPound.Services"
appcmd delete site "Wodeyun.Project.EmptyPound.Services"

appcmd add apppool /name:"Wodeyun.Project.EmptyPound.Services"
appcmd set apppool "Wodeyun.Project.EmptyPound.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.EmptyPound.Services"^
                /bindings:http/*:16006:^
                /physicalPath:"%cd%\Wodeyun.Project.EmptyPound.Services"
appcmd set site "Wodeyun.Project.EmptyPound.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.EmptyPound.Services"

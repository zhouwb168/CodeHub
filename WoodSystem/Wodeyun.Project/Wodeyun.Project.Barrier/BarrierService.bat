path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Barrier.Services"
appcmd delete site "Wodeyun.Project.Barrier.Services"

appcmd add apppool /name:"Wodeyun.Project.Barrier.Services"
appcmd set apppool "Wodeyun.Project.Barrier.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Barrier.Services"^
                /bindings:http/*:16001:^
                /physicalPath:"%cd%\Wodeyun.Project.Barrier.Services"
appcmd set site "Wodeyun.Project.Barrier.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.Barrier.Services"

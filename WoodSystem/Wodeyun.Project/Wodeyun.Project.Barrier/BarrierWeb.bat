path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Barrier.Web"
appcmd delete site "Wodeyun.Project.Barrier.Web"

appcmd add apppool /name:"Wodeyun.Project.Barrier.Web"
appcmd set apppool "Wodeyun.Project.Barrier.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Barrier.Web"^
                /bindings:http/*:17001:^
                /physicalPath:"%cd%\Wodeyun.Project.Barrier.Web"
appcmd set site "Wodeyun.Project.Barrier.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.Barrier.Web"

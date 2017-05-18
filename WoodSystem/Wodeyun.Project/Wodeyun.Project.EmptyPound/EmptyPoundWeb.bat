path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.EmptyPound.Web"
appcmd delete site "Wodeyun.Project.EmptyPound.Web"

appcmd add apppool /name:"Wodeyun.Project.EmptyPound.Web"
appcmd set apppool "Wodeyun.Project.EmptyPound.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.EmptyPound.Web"^
                /bindings:http/*:17006:^
                /physicalPath:"%cd%\Wodeyun.Project.EmptyPound.Web"
appcmd set site "Wodeyun.Project.EmptyPound.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.EmptyPound.Web"

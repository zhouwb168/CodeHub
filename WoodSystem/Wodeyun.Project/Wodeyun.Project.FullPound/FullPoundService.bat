path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.FullPound.Services"
appcmd delete site "Wodeyun.Project.FullPound.Services"

appcmd add apppool /name:"Wodeyun.Project.FullPound.Services"
appcmd set apppool "Wodeyun.Project.FullPound.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.FullPound.Services"^
                /bindings:http/*:16005:^
                /physicalPath:"%cd%\Wodeyun.Project.FullPound.Services"
appcmd set site "Wodeyun.Project.FullPound.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.FullPound.Services"

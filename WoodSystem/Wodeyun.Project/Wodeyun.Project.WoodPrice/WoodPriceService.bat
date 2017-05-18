path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPrice.Services"
appcmd delete site "Wodeyun.Project.WoodPrice.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodPrice.Services"
appcmd set apppool "Wodeyun.Project.WoodPrice.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPrice.Services"^
                /bindings:http/*:16023:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPrice.Services"
appcmd set site "Wodeyun.Project.WoodPrice.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPrice.Services"

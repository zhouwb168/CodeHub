path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPackBox.Services"
appcmd delete site "Wodeyun.Project.WoodPackBox.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodPackBox.Services"
appcmd set apppool "Wodeyun.Project.WoodPackBox.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPackBox.Services"^
                /bindings:http/*:16008:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPackBox.Services"
appcmd set site "Wodeyun.Project.WoodPackBox.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPackBox.Services"

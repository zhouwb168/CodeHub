path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodCarPhoto.Services"
appcmd delete site "Wodeyun.Project.WoodCarPhoto.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodCarPhoto.Services"
appcmd set apppool "Wodeyun.Project.WoodCarPhoto.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodCarPhoto.Services"^
                /bindings:http/*:16017:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodCarPhoto.Services"
appcmd set site "Wodeyun.Project.WoodCarPhoto.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodCarPhoto.Services"

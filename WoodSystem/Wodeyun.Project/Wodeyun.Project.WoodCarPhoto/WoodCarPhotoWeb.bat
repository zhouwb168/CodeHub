path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodCarPhoto.Web"
appcmd delete site "Wodeyun.Project.WoodCarPhoto.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodCarPhoto.Web"
appcmd set apppool "Wodeyun.Project.WoodCarPhoto.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodCarPhoto.Web"^
                /bindings:http/*:17017:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodCarPhoto.Web"
appcmd set site "Wodeyun.Project.WoodCarPhoto.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodCarPhoto.Web"

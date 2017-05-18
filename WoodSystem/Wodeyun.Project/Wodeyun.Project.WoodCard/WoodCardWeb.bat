path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodCard.Web"
appcmd delete site "Wodeyun.Project.WoodCard.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodCard.Web"
appcmd set apppool "Wodeyun.Project.WoodCard.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodCard.Web"^
                /bindings:http/*:17018:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodCard.Web"
appcmd set site "Wodeyun.Project.WoodCard.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodCard.Web"

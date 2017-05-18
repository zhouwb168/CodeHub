path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodCard.Services"
appcmd delete site "Wodeyun.Project.WoodCard.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodCard.Services"
appcmd set apppool "Wodeyun.Project.WoodCard.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodCard.Services"^
                /bindings:http/*:16018:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodCard.Services"
appcmd set site "Wodeyun.Project.WoodCard.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodCard.Services"

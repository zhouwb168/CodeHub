path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodDataList.Services"
appcmd delete site "Wodeyun.Project.WoodDataList.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodDataList.Services"
appcmd set apppool "Wodeyun.Project.WoodDataList.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodDataList.Services"^
                /bindings:http/*:16021:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodDataList.Services"
appcmd set site "Wodeyun.Project.WoodDataList.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodDataList.Services"

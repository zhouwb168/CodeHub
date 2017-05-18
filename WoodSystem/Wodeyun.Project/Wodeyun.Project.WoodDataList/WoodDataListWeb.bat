path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodDataList.Web"
appcmd delete site "Wodeyun.Project.WoodDataList.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodDataList.Web"
appcmd set apppool "Wodeyun.Project.WoodDataList.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodDataList.Web"^
                /bindings:http/*:17021:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodDataList.Web"
appcmd set site "Wodeyun.Project.WoodDataList.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodDataList.Web"

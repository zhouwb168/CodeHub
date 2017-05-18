path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodReport.Web"
appcmd delete site "Wodeyun.Project.WoodReport.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodReport.Web"
appcmd set apppool "Wodeyun.Project.WoodReport.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodReport.Web"^
                /bindings:http/*:17011:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodReport.Web"
appcmd set site "Wodeyun.Project.WoodReport.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodReport.Web"

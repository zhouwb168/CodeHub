path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodReport.Services"
appcmd delete site "Wodeyun.Project.WoodReport.Services"

appcmd delete apppool "Wodeyun.Project.WoodReport.Web"
appcmd delete site "Wodeyun.Project.WoodReport.Web"

path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodDataList.Services"
appcmd delete site "Wodeyun.Project.WoodDataList.Services"

appcmd delete apppool "Wodeyun.Project.WoodDataList.Web"
appcmd delete site "Wodeyun.Project.WoodDataList.Web"
